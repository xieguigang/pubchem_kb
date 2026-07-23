' ============================================================================
' MetabolicAgent.vb - 基于LLM的天然产物代谢知识库构建Agent核心逻辑
'
' 这个模块实现了agent的完整工作流程：
'   Phase 1: 理解用户研究主题 → 生成搜索关键词 → 调用search_papers搜索文献
'   Phase 2: 对每篇文献调用get_full_text获取全文
'   Phase 3: LLM逐篇阅读全文 → 提取9字段代谢反应信息 → 保存为JSON文件
'
' 工作流程说明：
'   1. 用户通过命令行参数提供自然语言研究主题
'   2. LLM理解主题后生成英文搜索关键词
'   3. LLM通过function calling调用search_papers搜索PubMed本地镜像数据库
'   4. 对搜索到的每篇文献，直接调用get_full_text获取全文
'   5. LLM逐篇阅读全文，基于研究主题提取代谢反应信息
'   6. 每篇文献提取的代谢反应数组保存为独立的JSON文件
' ============================================================================
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports System.Threading
Imports Ollama
Imports Researcher.MetabolicAgent.Models

Namespace MetabolicAgent

    ''' <summary>
    ''' 基于LLM的天然产物代谢知识库构建Agent
    ''' </summary>
    ''' <remarks>
    ''' 该Agent利用本地Ollama LLM服务，结合PubMed本地镜像MySQL数据库，
    ''' 自动从文献中提取天然产物代谢反应信息，构建结构化的代谢知识库。
    ''' </remarks>
    Public Class MetabolicLLMAgent
        Implements IDisposable

        ' LLM客户端实例，用于与Ollama服务通信
        Private ReadOnly _llmClient As LLMClient

        ' PubMed数据库查询工具实例
        Private ReadOnly _pubmedTool As PubMedQueryTool

        ' 输出目录路径
        Private ReadOnly _outputDir As String

        ' 日志输出动作，默认输出到控制台
        Private _logAction As Action(Of String)

        ''' <summary>
        ''' 创建MetabolicAgent实例
        ''' </summary>
        ''' <param name="llmClient">已初始化的LLM客户端实例</param>
        ''' <param name="connectionString">PubMed MySQL数据库连接字符串</param>
        ''' <param name="outputDir">代谢反应JSON输出目录</param>
        Public Sub New(llmClient As LLMClient, connectionString As String, outputDir As String)
            _llmClient = llmClient
            _pubmedTool = New PubMedQueryTool(connectionString)
            _outputDir = outputDir
            _logAction = Sub(msg) Console.WriteLine(msg)

            ' 确保输出目录存在
            If Not Directory.Exists(_outputDir) Then
                Directory.CreateDirectory(_outputDir)
            End If

            ' 注册PubMed查询工具的函数到LLM，使LLM能够自主调用
            ' 注册search_papers供Phase 1关键词搜索使用
            _llmClient.AddFunction(_pubmedTool, "search_papers")
            ' 注册get_full_text供LLM在需要时获取全文（虽然主流程中我们直接调用）
            _llmClient.AddFunction(_pubmedTool, "get_full_text")
            ' 注册get_database_stats供LLM了解数据库规模
            _llmClient.AddFunction(_pubmedTool, "get_database_stats")
        End Sub

        ''' <summary>
        ''' 设置日志输出回调
        ''' </summary>
        Public Sub SetLogger(logger As Action(Of String))
            _logAction = logger
        End Sub

        ' ====================================================================
        ' Phase 1: 理解研究主题 → 生成关键词 → 搜索文献
        ' ====================================================================

        ''' <summary>
        ''' Phase 1: 理解用户研究主题，生成搜索关键词，搜索PubMed数据库获取相关文献列表
        ''' </summary>
        ''' <param name="researchTopic">用户用自然语言描述的研究主题</param>
        ''' <param name="maxPapers">最大返回文献数量</param>
        ''' <param name="cancellationToken">取消令牌</param>
        ''' <returns>搜索到的文献信息列表</returns>
        Public Async Function SearchPapersAsync(
            researchTopic As String,
            maxPapers As Integer,
            Optional cancellationToken As CancellationToken = Nothing
        ) As Task(Of List(Of PaperInfo))

            Log("="c, 60)
            Log("Phase 1: 理解研究主题并搜索文献")
            Log("="c, 60)
            Log($"研究主题: {researchTopic}")
            Log("")

            ' 构建Phase 1的提示词，要求LLM理解主题、生成关键词、调用search_papers
            Dim phase1Prompt = BuildPhase1Prompt(researchTopic, maxPapers)

            Log("正在与LLM通信，理解研究主题并生成搜索关键词...")
            Log("LLM将自动调用 search_papers 函数搜索PubMed数据库...")
            Log("")

            ' 调用LLM，LLM会自主决定调用search_papers函数
            Dim response = Await _llmClient.Chat(phase1Prompt, cancellationToken)

            ' 输出LLM的思考过程（如果有）
            If Not String.IsNullOrWhiteSpace(response.think) Then
                Log("--- LLM思考过程 ---")
                Log(response.think)
                Log("--- 思考过程结束 ---")
                Log("")
            End If

            ' 从LLM响应中提取JSON格式的文献列表
            Dim papers = ParsePaperListFromResponse(response)

            Log($"")
            Log($"Phase 1完成: 共找到 {papers.Count} 篇相关文献")
            Log("")

            ' 打印文献列表摘要
            For i = 0 To papers.Count - 1
                Dim p = papers(i)
                Log($"  [{i + 1}] PMID:{p.pmid} | {p.year} | {TruncateText(p.title, 80)}")
            Next
            Log("")

            Return papers
        End Function

        ''' <summary>
        ''' 构建Phase 1的提示词
        ''' 要求LLM理解研究主题、生成英文关键词、调用search_papers、返回JSON文献列表
        ''' </summary>
        Private Function BuildPhase1Prompt(researchTopic As String, maxPapers As Integer) As String
            Dim sb As New StringBuilder()
            sb.AppendLine("You are a scientific research assistant specializing in natural product biosynthesis and metabolism research.")
            sb.AppendLine()
            sb.AppendLine("The user is researching the following topic (described in natural language):")
            sb.AppendLine($"""{researchTopic}""")
            sb.AppendLine()
            sb.AppendLine("Your tasks:")
            sb.AppendLine("1. Carefully understand the user's research topic and identify the key scientific concepts.")
            sb.AppendLine("2. Generate 2-5 English search keywords that best represent the research topic for searching the PubMed database.")
            sb.AppendLine("   - Keywords should be specific scientific terms (e.g., compound names, enzyme names, pathway names, organism names)")
            sb.AppendLine("   - Use spaces to separate multiple keywords in a single search")
            sb.AppendLine("3. Call the search_papers function to search the PubMed database with your generated keywords.")
            sb.AppendLine($"   - Set max_results to {maxPapers} to get enough papers")
            sb.AppendLine("   - You may call search_papers multiple times with different keyword combinations if needed")
            sb.AppendLine("4. After getting the search results, compile a deduplicated list of all found papers.")
            sb.AppendLine()
            sb.AppendLine("Finally, return ONLY a JSON array of papers with these fields for each paper:")
            sb.AppendLine("  - pmid: PubMed ID")
            sb.AppendLine("  - title: paper title")
            sb.AppendLine("  - doi: DOI")
            sb.AppendLine("  - year: publication year")
            sb.AppendLine("  - journal: journal name")
            sb.AppendLine("  - authors: authors string")
            sb.AppendLine("  - abstract: abstract text (truncate to 1000 chars if too long)")
            sb.AppendLine("  - mesh_terms: MeSH terms string")
            sb.AppendLine()
            sb.AppendLine("IMPORTANT:")
            sb.AppendLine("- Return ONLY the JSON array, no additional explanation text.")
            sb.AppendLine("- If no papers found, return an empty array [].")
            sb.AppendLine("- Deduplicate papers by PMID.")
            sb.AppendLine()
            sb.AppendLine("Example output format:")
            sb.AppendLine("[")
            sb.AppendLine("  {""pmid"": ""12345678"", ""title"": ""Biosynthesis of..."" , ""doi"": ""10.1038/..."", ""year"": ""2023"", ""journal"": ""Nature"", ""authors"": ""Smith J, et al."", ""abstract"": ""..."", ""mesh_terms"": ""...""}")
            sb.AppendLine("]")
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' 从LLM响应中解析文献列表
        ''' 支持两种格式：
        '''   1. JSON数组: [{"pmid":"...", "title":"..."}, ...]
        '''   2. QueryResult对象: {"count":N, "papers":[...], "query_sql":"..."}
        ''' </summary>
        Private Function ParsePaperListFromResponse(response As LLMsResponse) As List(Of PaperInfo)
            Dim papers As New List(Of PaperInfo)()

            ' 优先使用ExtractJsonFromResponse提取JSON
            Dim jsonStr As String = Nothing
            Try
                jsonStr = response.ExtractJsonFromResponse()
            Catch
                jsonStr = Nothing
            End Try

            ' 如果提取失败，尝试从output中查找JSON数组
            If String.IsNullOrWhiteSpace(jsonStr) AndAlso Not String.IsNullOrWhiteSpace(response.output) Then
                jsonStr = ExtractJsonArrayFromString(response.output)
            End If

            ' 如果还是失败，尝试从output中查找JSON对象（QueryResult格式）
            If String.IsNullOrWhiteSpace(jsonStr) AndAlso Not String.IsNullOrWhiteSpace(response.output) Then
                jsonStr = ExtractJsonObjectFromString(response.output)
            End If

            If String.IsNullOrWhiteSpace(jsonStr) Then
                Log("警告: 无法从LLM响应中提取JSON文献列表")
                Log($"LLM原始输出: {TruncateText(response.output, 500)}")
                Return papers
            End If

            ' 尝试反序列化为PaperInfo列表（JSON数组格式）
            Dim result As List(Of PaperInfo) = Nothing
            If JsonHelper.TryFromJson(jsonStr, result) AndAlso result IsNot Nothing Then
                papers = result.Where(Function(p) Not String.IsNullOrWhiteSpace(p?.pmid)).ToList()
            Else
                ' 尝试从QueryResult格式中提取papers数组
                papers = ExtractPapersFromQueryResult(jsonStr)

                If papers.Count = 0 Then
                    Log("警告: JSON反序列化失败，尝试修复JSON格式...")
                    Dim fixedJson = FixCommonJsonIssues(jsonStr)
                    If JsonHelper.TryFromJson(fixedJson, result) AndAlso result IsNot Nothing Then
                        papers = result.Where(Function(p) Not String.IsNullOrWhiteSpace(p?.pmid)).ToList()
                    Else
                        papers = ExtractPapersFromQueryResult(fixedJson)
                    End If
                End If

                If papers.Count = 0 Then
                    Log($"JSON解析最终失败。原始JSON片段: {TruncateText(jsonStr, 300)}")
                End If
            End If

            Return papers
        End Function

        ''' <summary>
        ''' 从QueryResult格式的JSON中提取papers数组并反序列化
        ''' QueryResult格式: {"count":N, "query_sql":"...", "papers":[{...}]}
        ''' </summary>
        Private Function ExtractPapersFromQueryResult(jsonStr As String) As List(Of PaperInfo)
            Dim papers As New List(Of PaperInfo)()
            Try
                Using doc As JsonDocument = JsonDocument.Parse(jsonStr)
                    Dim papersProp As JsonElement
                    If doc.RootElement.ValueKind = JsonValueKind.Object AndAlso
                       doc.RootElement.TryGetProperty("papers", papersProp) AndAlso
                       papersProp.ValueKind = JsonValueKind.Array Then
                        Dim papersJson = papersProp.GetRawText()
                        Dim result As List(Of PaperInfo) = Nothing
                        If JsonHelper.TryFromJson(papersJson, result) AndAlso result IsNot Nothing Then
                            papers = result.Where(Function(p) Not String.IsNullOrWhiteSpace(p?.pmid)).ToList()
                        End If
                    End If
                End Using
            Catch
            End Try
            Return papers
        End Function

        ' ====================================================================
        ' Phase 2: 获取文献全文
        ' ====================================================================

        ''' <summary>
        ''' Phase 2: 根据PMID获取文献全文
        ''' 直接调用PubMedQueryTool，无需通过LLM
        ''' </summary>
        Public Function GetPaperFullText(paper As PaperInfo) As PaperFullText
            Log($"  正在获取全文: PMID:{paper.pmid}")

            Dim fullTextJson = _pubmedTool.get_full_text(paper.pmid)

            ' 解析返回的JSON
            Dim result As PaperFullText = Nothing

            ' get_full_text返回的是QueryResult结构，需要提取papers数组中的第一个元素
            Dim jsonStr As String = Nothing
            Try
                ' 尝试直接提取JSON
                jsonStr = ExtractJsonFromQueryResult(fullTextJson)
            Catch
            End Try

            If Not String.IsNullOrWhiteSpace(jsonStr) Then
                JsonHelper.TryFromJson(jsonStr, result)
            End If

            ' 如果解析失败，使用摘要作为全文
            If result Is Nothing OrElse String.IsNullOrWhiteSpace(result.full_text) Then
                Log($"    全文获取失败或为空，使用摘要作为替代")
                result = New PaperFullText With {
                    .pmid = paper.pmid,
                    .title = paper.title,
                    .doi = paper.doi,
                    .abstract = paper.abstract,
                    .full_text = paper.abstract,
                    .mesh_terms = paper.mesh_terms
                }
            Else
                ' 补充缺失的字段
                If String.IsNullOrWhiteSpace(result.title) Then result.title = paper.title
                If String.IsNullOrWhiteSpace(result.doi) Then result.doi = paper.doi
                If String.IsNullOrWhiteSpace(result.abstract) Then result.abstract = paper.abstract
            End If

            Log($"    全文长度: {result.full_text?.Length} 字符")
            Return result
        End Function

        ''' <summary>
        ''' 从get_full_text返回的QueryResult JSON中提取单篇文献的JSON
        ''' </summary>
        Private Function ExtractJsonFromQueryResult(queryResultJson As String) As String
            ' QueryResult结构: {count, query_sql, papers: [{...}]}
            ' 我们需要提取papers数组中的第一个元素
            Try
                Using doc As JsonDocument = JsonDocument.Parse(queryResultJson)
                    Dim papersProp As JsonElement
                    If doc.RootElement.TryGetProperty("papers", papersProp) AndAlso
                       papersProp.ValueKind = JsonValueKind.Array AndAlso
                       papersProp.GetArrayLength() > 0 Then
                        Return papersProp(0).GetRawText()
                    End If
                End Using
            Catch
            End Try
            Return Nothing
        End Function

        ' ====================================================================
        ' Phase 3: LLM逐篇阅读全文并提取代谢反应信息
        ' ====================================================================

        ''' <summary>
        ''' Phase 3: LLM阅读单篇文献全文，提取代谢反应信息
        ''' </summary>
        ''' <param name="paper">文献全文信息</param>
        ''' <param name="researchTopic">用户研究主题（用于相关性判断）</param>
        ''' <param name="cancellationToken">取消令牌</param>
        ''' <returns>提取到的代谢反应列表</returns>
        Public Async Function ExtractReactionsAsync(
            paper As PaperFullText,
            researchTopic As String,
            Optional cancellationToken As CancellationToken = Nothing
        ) As Task(Of List(Of MetabolicReaction))

            Log($"  正在LLM阅读并提取代谢反应信息...")

            ' 构建Phase 3的提取提示词
            Dim extractPrompt = BuildPhase3Prompt(paper, researchTopic)

            ' 如果全文过长，进行截断（保留前面部分，通常包含摘要、引言、方法、结果）
            Dim maxContextLength As Integer = 80000
            If paper.full_text?.Length > maxContextLength Then
                Log($"    全文过长({paper.full_text.Length}字符)，截断至{maxContextLength}字符")
                paper.full_text = paper.full_text.Substring(0, maxContextLength) & "...[truncated]"
            End If

            ' 调用LLM提取代谢反应
            Dim response = Await _llmClient.Chat(extractPrompt, cancellationToken)

            ' 输出LLM思考过程
            If Not String.IsNullOrWhiteSpace(response.think) Then
                Log($"    --- LLM思考过程 ---")
                Log(IndentText(TruncateText(response.think, 1000), "      "))
                Log($"    --- 思考过程结束 ---")
            End If

            ' 解析代谢反应JSON数组
            Dim reactions = ParseReactionsFromResponse(response, paper)

            Log($"    提取到 {reactions.Count} 条代谢反应")

            Return reactions
        End Function

        ''' <summary>
        ''' 构建Phase 3的代谢反应提取提示词
        ''' </summary>
        Private Function BuildPhase3Prompt(paper As PaperFullText, researchTopic As String) As String
            Dim sb As New StringBuilder()
            sb.AppendLine("You are a professional biochemistry research assistant specializing in natural product metabolism.")
            sb.AppendLine("Your task is to carefully read the following scientific paper and extract metabolic reactions.")
            sb.AppendLine()
            sb.AppendLine("================================================================")
            sb.AppendLine("USER RESEARCH TOPIC (for relevance filtering):")
            sb.AppendLine("================================================================")
            sb.AppendLine($"""{researchTopic}""")
            sb.AppendLine()
            sb.AppendLine("================================================================")
            sb.AppendLine("PAPER INFORMATION")
            sb.AppendLine("================================================================")
            sb.AppendLine($"PMID: {paper.pmid}")
            sb.AppendLine($"Title: {paper.title}")
            sb.AppendLine($"DOI: {paper.doi}")
            If Not String.IsNullOrWhiteSpace(paper.mesh_terms) Then
                sb.AppendLine($"MeSH Terms: {paper.mesh_terms}")
            End If
            sb.AppendLine()
            sb.AppendLine("================================================================")
            sb.AppendLine("PAPER FULL TEXT")
            sb.AppendLine("================================================================")
            sb.AppendLine(paper.full_text)
            sb.AppendLine()
            sb.AppendLine("================================================================")
            sb.AppendLine("EXTRACTION INSTRUCTIONS")
            sb.AppendLine("================================================================")
            sb.AppendLine("Extract ALL metabolic reactions from this paper that are relevant to the user's research topic.")
            sb.AppendLine("Each metabolic reaction must be a JSON object with EXACTLY these 9 fields:")
            sb.AppendLine()
            sb.AppendLine("1. ""substrates"": Array of substrate English names (e.g., [""glucose"", ""ATP""])")
            sb.AppendLine("   - Use standard biochemical names")
            sb.AppendLine("   - Include cofactors like ATP, NADPH, etc.")
            sb.AppendLine()
            sb.AppendLine("2. ""products"": Array of product English names (e.g., [""glucose-6-phosphate"", ""ADP""])")
            sb.AppendLine("   - Use standard biochemical names")
            sb.AppendLine("   - Include byproducts like ADP, NADP+, etc.")
            sb.AppendLine()
            sb.AppendLine("3. ""reaction_name"": Name of the metabolic reaction (e.g., ""hexokinase reaction"")")
            sb.AppendLine()
            sb.AppendLine("4. ""reaction_description"": Detailed description of the reaction")
            sb.AppendLine("   - Describe what happens in the reaction")
            sb.AppendLine("   - Include conditions, mechanisms if mentioned")
            sb.AppendLine()
            sb.AppendLine("5. ""enzyme"": Object containing enzyme information with 4 sub-fields:")
            sb.AppendLine("   {")
            sb.AppendLine("     ""gene_id"": gene identifier (e.g., ""HGNC:4845"", ""NCBI:2645"", or null if not mentioned),")
            sb.AppendLine("     ""name"": enzyme name (e.g., ""hexokinase"", ""cytochrome P450 3A4""),")
            sb.AppendLine("     ""ec_number"": EC number (e.g., ""2.7.1.1"", or null if not mentioned),")
            sb.AppendLine("     ""protein_domains"": array of protein domain names (e.g., [""Hexokinase_N"", ""Hexokinase_C""])")
            sb.AppendLine("   }")
            sb.AppendLine()
            sb.AppendLine("6. ""pathway"": Metabolic pathway name (e.g., ""glycolysis"", ""mevalonate pathway"")")
            sb.AppendLine()
            sb.AppendLine("7. ""source_organisms"": Array of source organism scientific names")
            sb.AppendLine("   (e.g., [""Homo sapiens"", ""Escherichia coli"", ""Streptomyces coelicolor""])")
            sb.AppendLine()
            sb.AppendLine("8. ""source_doi"": DOI of this paper")
            sb.AppendLine()
            sb.AppendLine("9. ""source_title"": Title of this paper")
            sb.AppendLine()
            sb.AppendLine("================================================================")
            sb.AppendLine("CRITICAL RULES - MUST FOLLOW:")
            sb.AppendLine("================================================================")
            sb.AppendLine("- ONLY extract reactions that are EXPLICITLY described in the paper text.")
            sb.AppendLine("- DO NOT fabricate or invent any information not present in the paper.")
            sb.AppendLine("- If a field is not mentioned in the paper, use null (for single values) or [] (for arrays).")
            sb.AppendLine("- Only extract reactions relevant to the user's research topic.")
            sb.AppendLine("- If NO metabolic reactions are found in this paper, return an empty array [].")
            sb.AppendLine("- Each reaction must have substrates and products (at least one each).")
            sb.AppendLine("- Set source_doi and source_title to this paper's DOI and title.")
            sb.AppendLine()
            sb.AppendLine("================================================================")
            sb.AppendLine("OUTPUT FORMAT")
            sb.AppendLine("================================================================")
            sb.AppendLine("Return ONLY a JSON array of reaction objects. No additional text.")
            sb.AppendLine()
            sb.AppendLine("Example output:")
            sb.AppendLine("[")
            sb.AppendLine("  {")
            sb.AppendLine("    ""substrates"": [""glucose"", ""ATP""],")
            sb.AppendLine("    ""products"": [""glucose-6-phosphate"", ""ADP""],")
            sb.AppendLine("    ""reaction_name"": ""hexokinase reaction"",")
            sb.AppendLine("    ""reaction_description"": ""Phosphorylation of glucose to glucose-6-phosphate using ATP as phosphate donor."",")
            sb.AppendLine("    ""enzyme"": {")
            sb.AppendLine("      ""gene_id"": ""HGNC:4845"",")
            sb.AppendLine("      ""name"": ""hexokinase"",")
            sb.AppendLine("      ""ec_number"": ""2.7.1.1"",")
            sb.AppendLine("      ""protein_domains"": [""Hexokinase_N"", ""Hexokinase_C""]")
            sb.AppendLine("    },")
            sb.AppendLine("    ""pathway"": ""glycolysis"",")
            sb.AppendLine("    ""source_organisms"": [""Homo sapiens""],")
            sb.AppendLine("    ""source_doi"": """ & paper.doi & """,")
            sb.AppendLine("    ""source_title"": """ & paper.title.Replace("""", "\""") & """")
            sb.AppendLine("  }")
            sb.AppendLine("]")
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' 从LLM响应中解析代谢反应列表
        ''' </summary>
        Private Function ParseReactionsFromResponse(
            response As LLMsResponse,
            paper As PaperFullText
        ) As List(Of MetabolicReaction)

            Dim reactions As New List(Of MetabolicReaction)()

            ' 优先使用ExtractJsonFromResponse提取JSON
            Dim jsonStr As String = Nothing
            Try
                jsonStr = response.ExtractJsonFromResponse()
            Catch
                jsonStr = Nothing
            End Try

            ' 如果提取失败，尝试从output中查找JSON数组
            If String.IsNullOrWhiteSpace(jsonStr) AndAlso Not String.IsNullOrWhiteSpace(response.output) Then
                jsonStr = ExtractJsonArrayFromString(response.output)
            End If

            If String.IsNullOrWhiteSpace(jsonStr) Then
                Log($"    警告: 无法从LLM响应中提取JSON")
                Log($"    LLM原始输出: {TruncateText(response.output, 300)}")
                Return reactions
            End If

            ' 尝试反序列化
            Dim result As List(Of MetabolicReaction) = Nothing
            If JsonHelper.TryFromJson(jsonStr, result) AndAlso result IsNot Nothing Then
                reactions = result
            Else
                ' 尝试修复JSON
                Dim fixedJson = FixCommonJsonIssues(jsonStr)
                If JsonHelper.TryFromJson(fixedJson, result) AndAlso result IsNot Nothing Then
                    reactions = result
                Else
                    Log($"    JSON解析失败。原始JSON片段: {TruncateText(jsonStr, 300)}")
                End If
            End If

            ' 后处理：确保每条反应都有source_doi和source_title
            For Each r In reactions
                If String.IsNullOrWhiteSpace(r.source_doi) Then r.source_doi = paper.doi
                If String.IsNullOrWhiteSpace(r.source_title) Then r.source_title = paper.title
                ' 确保列表字段不为Nothing
                If r.substrates Is Nothing Then r.substrates = New List(Of String)()
                If r.products Is Nothing Then r.products = New List(Of String)()
                If r.source_organisms Is Nothing Then r.source_organisms = New List(Of String)()
                If r.enzyme Is Nothing Then r.enzyme = New EnzymeInfo()
                If r.enzyme.protein_domains Is Nothing Then r.enzyme.protein_domains = New List(Of String)()
            Next

            ' 过滤掉无效反应（既没有底物也没有产物的）
            reactions = reactions.Where(Function(r) r.substrates.Count > 0 OrElse r.products.Count > 0).ToList()

            Return reactions
        End Function

        ' ====================================================================
        ' 保存代谢反应到JSON文件
        ' ====================================================================

        ''' <summary>
        ''' 将单篇文献提取的代谢反应保存为JSON文件
        ''' 文件名格式: {PMID}_{sanitized_title}.json
        ''' </summary>
        Public Sub SaveReactionsToFile(paper As PaperFullText, reactions As List(Of MetabolicReaction))
            ' 生成安全的文件名
            Dim safeTitle = SanitizeFileName(paper.title)
            If safeTitle.Length > 60 Then safeTitle = safeTitle.Substring(0, 60)
            Dim fileName = $"{paper.pmid}_{safeTitle}.json"
            Dim filePath = Path.Combine(_outputDir, fileName)

            ' 构建包含元数据的输出对象
            Dim outputObj = New With {
                .pmid = paper.pmid,
                .title = paper.title,
                .doi = paper.doi,
                .reaction_count = reactions.Count,
                .reactions = reactions
            }

            Dim jsonContent = JsonHelper.ToJson(outputObj)
            File.WriteAllText(filePath, jsonContent, Encoding.UTF8)

            Log($"    已保存: {fileName}")
        End Sub

        ' ====================================================================
        ' 完整工作流程
        ' ====================================================================

        ''' <summary>
        ''' 执行完整的agent工作流程
        ''' </summary>
        ''' <param name="researchTopic">用户研究主题</param>
        ''' <param name="maxPapers">最大处理文献数</param>
        ''' <param name="cancellationToken">取消令牌</param>
        Public Async Function RunAsync(
            researchTopic As String,
            maxPapers As Integer,
            Optional cancellationToken As CancellationToken = Nothing
        ) As Task

            Log("")
            Log(New String("="c, 70))
            Log("  天然产物代谢知识库构建Agent - 启动")
            Log(New String("="c, 70))
            Log($"研究主题: {researchTopic}")
            Log($"最大文献数: {maxPapers}")
            Log($"输出目录: {_outputDir}")
            Log("")

            ' Phase 1: 搜索文献
            Dim papers = Await SearchPapersAsync(researchTopic, maxPapers, cancellationToken)

            If papers.Count = 0 Then
                Log("未找到相关文献，程序结束。")
                Return
            End If

            ' 限制处理数量
            If papers.Count > maxPapers Then
                Log($"文献数量({papers.Count})超过最大限制({maxPapers})，仅处理前{maxPapers}篇")
                papers = papers.Take(maxPapers).ToList()
            End If

            ' Phase 2 & 3: 逐篇处理
            Log("")
            Log(New String("="c, 70))
            Log("Phase 2 & 3: 逐篇获取全文并提取代谢反应信息")
            Log(New String("="c, 70))
            Log("")

            Dim totalReactions As Integer = 0
            Dim successCount As Integer = 0
            Dim failCount As Integer = 0

            For i = 0 To papers.Count - 1
                Dim paperInfo = papers(i)
                Log("")
                Log($"--- 处理文献 [{i + 1}/{papers.Count}]: PMID:{paperInfo.pmid} ---")
                Log($"    标题: {TruncateText(paperInfo.title, 100)}")

                Try
                    ' Phase 2: 获取全文
                    Dim fullText = GetPaperFullText(paperInfo)

                    ' Phase 3: LLM提取代谢反应
                    Dim reactions = Await ExtractReactionsAsync(fullText, researchTopic, cancellationToken)

                    ' 保存结果
                    SaveReactionsToFile(fullText, reactions)

                    totalReactions += reactions.Count
                    successCount += 1

                    If reactions.Count > 0 Then
                        Log($"    提取的代谢反应预览:")
                        For Each r In reactions.Take(3)
                            Log($"      • {r.reaction_name}: {String.Join(" + ", r.substrates)} → {String.Join(" + ", r.products)}")
                        Next
                        If reactions.Count > 3 Then
                            Log($"      ... 还有 {reactions.Count - 3} 条")
                        End If
                    End If

                Catch ex As OperationCanceledException
                    Log("任务已取消")
                    Throw
                Catch ex As Exception
                    Log($"    处理文献PMID:{paperInfo.pmid}时出错: {ex.Message}")
                    failCount += 1
                End Try
            Next

            ' 输出汇总报告
            Log("")
            Log(New String("="c, 70))
            Log("  工作流程完成 - 汇总报告")
            Log(New String("="c, 70))
            Log($"  处理文献总数: {papers.Count}")
            Log($"  成功处理: {successCount}")
            Log($"  处理失败: {failCount}")
            Log($"  提取代谢反应总数: {totalReactions}")
            Log($"  输出目录: {Path.GetFullPath(_outputDir)}")
            Log(New String("="c, 70))
            Log("")

        End Function

        ' ====================================================================
        ' 辅助方法
        ' ====================================================================

        ''' <summary>
        ''' 从字符串中提取JSON数组（查找第一个 [ 到最后一个 ]）
        ''' </summary>
        Private Function ExtractJsonArrayFromString(text As String) As String
            If String.IsNullOrWhiteSpace(text) Then Return Nothing

            Dim startIdx = text.IndexOf("["c)
            Dim endIdx = text.LastIndexOf("]"c)

            If startIdx >= 0 AndAlso endIdx > startIdx Then
                Return text.Substring(startIdx, endIdx - startIdx + 1)
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' 从字符串中提取JSON对象（查找第一个 { 到最后一个 }）
        ''' 用于处理LLM返回QueryResult格式的情况
        ''' </summary>
        Private Function ExtractJsonObjectFromString(text As String) As String
            If String.IsNullOrWhiteSpace(text) Then Return Nothing

            Dim startIdx = text.IndexOf("{"c)
            Dim endIdx = text.LastIndexOf("}"c)

            If startIdx >= 0 AndAlso endIdx > startIdx Then
                Return text.Substring(startIdx, endIdx - startIdx + 1)
            End If

            Return Nothing
        End Function

        ''' <summary>
        ''' 修复常见的JSON格式问题
        ''' </summary>
        Private Function FixCommonJsonIssues(json As String) As String
            If String.IsNullOrWhiteSpace(json) Then Return json

            ' 移除可能的markdown代码块标记
            json = Regex.Replace(json, "```json\s*", "", RegexOptions.IgnoreCase)
            json = Regex.Replace(json, "```\s*$", "")

            ' 移除JSON前后的非JSON文本
            Dim startIdx = json.IndexOf("["c)
            Dim endIdx = json.LastIndexOf("]"c)
            If startIdx >= 0 AndAlso endIdx > startIdx Then
                json = json.Substring(startIdx, endIdx - startIdx + 1)
            End If

            Return json.Trim()
        End Function

        ''' <summary>
        ''' 将文件名中的非法字符替换为下划线
        ''' </summary>
        Private Function SanitizeFileName(name As String) As String
            If String.IsNullOrWhiteSpace(name) Then Return "untitled"
            Dim invalid = Path.GetInvalidFileNameChars()
            Dim result = New String(name.Select(Function(c) If(invalid.Contains(c), "_"c, c)).ToArray())
            ' 移除多余的空格和点
            result = result.Trim().Trim("."c).Trim()
            If String.IsNullOrWhiteSpace(result) Then Return "untitled"
            Return result
        End Function

        ''' <summary>
        ''' 截断文本到指定长度
        ''' </summary>
        Private Function TruncateText(text As String, maxLength As Integer) As String
            If String.IsNullOrEmpty(text) Then Return ""
            If text.Length <= maxLength Then Return text
            Return text.Substring(0, maxLength) & "..."
        End Function

        ''' <summary>
        ''' 给文本添加缩进
        ''' </summary>
        Private Function IndentText(text As String, indent As String) As String
            If String.IsNullOrEmpty(text) Then Return ""
            Dim lines = text.Split({vbCr, vbLf}, StringSplitOptions.None)
            Return String.Join(vbCrLf, lines.Select(Function(l) indent & l))
        End Function

        ''' <summary>
        ''' 日志重载：输出重复字符
        ''' </summary>
        Private Sub Log(c As Char, count As Integer)
            _logAction?.Invoke($"[{DateTime.Now:HH:mm:ss}] {New String(c, count)}")
        End Sub

        ''' <summary>
        ''' 日志重载：输出字符串
        ''' </summary>
        Private Sub Log(message As String)
            _logAction?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}")
        End Sub

#Region "IDisposable"

        Private _disposed As Boolean = False

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    ' 释放托管资源
                    Try
                        _llmClient?.Dispose()
                    Catch
                    End Try
                End If
                _disposed = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region

    End Class

End Namespace
