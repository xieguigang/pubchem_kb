Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.MIME.application.json
Imports Microsoft.VisualBasic.MIME.application.json.Javascript
Imports Microsoft.VisualBasic.MIME.application.json.LenientJson
Imports Microsoft.VisualBasic.MIME.application.pdf
Imports Ollama

' ============================================================================
' 注意：请根据你的项目实际情况，取消下面两行 Imports 的注释并填入正确的命名空间。
'   - PDF 类（含 GetText 方法）所在的命名空间
'   - LLMClient / LLMsResponse 类所在的命名空间
' ============================================================================
' Imports YourPdfNamespace
' Imports YourLlmNamespace

''' <summary>
''' 命令行程序：从 PDF 中提取全文，并使用 LLM 提取元数据、参考文献，
''' 以及清理后的全文 Markdown，最终输出为固定格式的 .txt 文件。
'''
''' 输出 .txt 格式（每行含义）：
'''   第 1 行  : title（文献标题）
'''   第 2 行  : 元数据 JSON {"doi":..,"year":..,"journal":..,"keywords":[..]}
'''   第 3 行  : 参考文献数组 JSON [{"title":..,"doi":..,"year":..,"journal":..}, ...]
'''   第 4 行  : 空白行
'''   第 5 行起: 经格式化、清理乱码后的文献全文 markdown 文本内容
''' </summary>
Public Module PDFText

    Public Async Function ExtractCleanText(pdfstream As Stream, llm As LLMClient, ct As CancellationToken) As Task(Of String)
        Dim rawText As String = PDF.GetText(pdfstream).JoinBy(vbCrLf & vbCrLf)

        If String.IsNullOrWhiteSpace(rawText) Then
            Console.Error.WriteLine("[ERROR] PDF 中未提取到任何文本。")
            Return Nothing
        End If
        Console.WriteLine($"[INFO] 已提取 {rawText.Length} 个字符。")
        Console.WriteLine()

        ' ========== Step 2~4: 使用 LLM 提取信息 ==========

        ' --- Step 2: 提取元数据 ---
        Console.WriteLine("[STEP 2] 正在请求 LLM 提取文献元数据（标题、DOI、年份、期刊、关键词）...")
        Dim title As String = ""
        Dim metadataJson As String = "{}"

        Dim metaResult = Await ExtractMetadataAsync(llm, rawText, ct)
        title = metaResult.Title
        metadataJson = metaResult.MetadataJson
        Console.WriteLine($"[INFO] 标题  : {title}")
        Console.WriteLine($"[INFO] 元数据: {metadataJson}")
        Console.WriteLine()

        ' --- Step 3: 提取参考文献 ---
        Console.WriteLine("[STEP 3] 正在请求 LLM 提取参考文献列表...")
        Dim referencesJson As String = "[]"
        referencesJson = Await ExtractReferencesAsync(llm, rawText, ct)
        Console.WriteLine($"[INFO] 参考文献已提取。")

        Console.WriteLine()

        ' --- Step 4: 清理全文为 Markdown ---
        Console.WriteLine("[STEP 4] 正在请求 LLM 清理全文并格式化为 Markdown...")
        Dim markdown As String = rawText
        markdown = Await CleanFullTextAsync(llm, rawText, ct)
        Console.WriteLine($"[INFO] Markdown 全文已生成。")
        ' ========== Step 5: 写入输出文件 ==========
        Console.WriteLine("[STEP 5] 正在写入输出文件...")

        Return WriteOutputFile(title, metadataJson, referencesJson, markdown)
    End Function

    ' =========================================================================
    '  LLM 调用方法
    ' =========================================================================

    ''' <summary>
    ''' 调用 LLM 提取元数据（标题、DOI、年份、期刊、关键词）。
    ''' 返回标题字符串和不含标题的元数据 JSON 字符串。
    ''' </summary>
    Private Async Function ExtractMetadataAsync(
        llm As LLMClient,
        rawText As String,
        ct As CancellationToken
    ) As Task(Of (Title As String, MetadataJson As String))

        Dim prompt As String = BuildMetadataPrompt(rawText)
        Dim resp As LLMsResponse = Await llm.Chat(prompt, ct)
        Dim jsonStr As String = resp.ExtractJsonFromResponse()

        If String.IsNullOrWhiteSpace(jsonStr) Then
            Return ("", "{}")
        End If

        ' 解析 LLM 返回的 JSON
        Dim doc As JsonObject = LenientJsonParser.ParseJSON(jsonStr)
        Dim title As String = doc!title.AsString(True)
        Dim doi As String = doc!doi.AsString(True)
        Dim year As String = doc!year.AsString(True)
        Dim journal As String = doc!journal.AsString(True)

        Dim keywords As New List(Of String)()
        Dim kwEl As JsonArray = doc!keywords
        For Each kw In kwEl
            keywords.Add(kw.AsString(True))
        Next

        ' 构建不含 title 的元数据 JSON（符合输出格式第 2 行要求）
        Dim metaDict As New Dictionary(Of String, Object) From {
            {"doi", doi},
            {"year", year},
            {"journal", journal},
            {"keywords", keywords.ToArray}
        }
        Dim metadataJson As String = metaDict.GetJson

        Return (title, metadataJson)
    End Function

    ''' <summary>
    ''' 调用 LLM 提取参考文献列表，返回规范化的 JSON 数组字符串。
    ''' 每条参考文献仅保留 title / doi / year / journal 四个字段。
    ''' </summary>
    Private Async Function ExtractReferencesAsync(
        llm As LLMClient,
        rawText As String,
        ct As CancellationToken
    ) As Task(Of String)

        Dim prompt As String = BuildReferencesPrompt(rawText)
        Dim resp As LLMsResponse = Await llm.Chat(prompt, ct)
        Dim jsonStr As String = resp.ExtractJsonFromResponse()

        If String.IsNullOrWhiteSpace(jsonStr) Then
            Return "[]"
        End If

        ' 解析并规范化参考文献 JSON

        Dim doc As JsonElement = LenientJsonParser.ParseJSON(jsonStr)
        Dim refs As New List(Of Dictionary(Of String, String))()

        If TypeOf doc Is JsonArray Then
            For Each refEl As JsonObject In DirectCast(doc, JsonArray)
                Dim refDict As New Dictionary(Of String, String) From {
                        {"title", refEl!title.AsString(True)},
                        {"doi", refEl!doi.AsString(True)},
                        {"year", refEl!year.AsString(True)},
                        {"journal", refEl!journal.AsString(True)}
                    }
                refs.Add(refDict)
            Next
        End If

        Return refs.ToArray.GetJson
    End Function

    ''' <summary>
    ''' 调用 LLM 清理全文文本并格式化为 Markdown。
    ''' </summary>
    Private Async Function CleanFullTextAsync(
        llm As LLMClient,
        rawText As String,
        ct As CancellationToken
    ) As Task(Of String)

        Dim prompt As String = BuildFullTextPrompt(rawText)
        Dim resp As LLMsResponse = Await llm.Chat(prompt, ct)
        Dim markdown As String = resp.output

        If String.IsNullOrWhiteSpace(markdown) Then
            Return rawText
        End If

        ' 统一换行符为 LF，去除首尾空白
        markdown = markdown.Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).Trim()
        Return markdown
    End Function

    ' =========================================================================
    '  Prompt 构建方法
    ' =========================================================================

    Private Function BuildMetadataPrompt(rawText As String) As String
        Dim sb As New StringBuilder()
        sb.AppendLine("你是一个科技文献元数据提取助手。请从以下 PDF 全文文本中提取文献的元数据信息。")
        sb.AppendLine()
        sb.AppendLine("需要提取的字段：")
        sb.AppendLine("- title：文献的完整标题")
        sb.AppendLine("- doi：文献的 DOI（如果没有则填空字符串 """"）")
        sb.AppendLine("- year：发表年份（4 位数字字符串，如 ""2023""，如果没有则填空字符串 """"）")
        sb.AppendLine("- journal：期刊名称（如果没有则填空字符串 """"）")
        sb.AppendLine("- keywords：关键词列表（字符串数组，如果没有则为空数组 []）")
        sb.AppendLine()
        sb.AppendLine("请仅返回一个 JSON 对象，不要包含任何其他文字、解释或 markdown 代码块标记。格式如下：")
        sb.AppendLine("{""title"":""..."",""doi"":""..."",""year"":""..."",""journal"":""..."",""keywords"":[""..."",""...""]}")
        sb.AppendLine()
        sb.AppendLine("========== PDF 全文文本开始 ==========")
        sb.AppendLine(rawText)
        sb.AppendLine("========== PDF 全文文本结束 ==========")
        Return sb.ToString()
    End Function

    Private Function BuildReferencesPrompt(rawText As String) As String
        Dim sb As New StringBuilder()
        sb.AppendLine("你是一个科技文献参考文献提取助手。请从以下 PDF 全文文本中找到参考文献列表（References / 参考文献），并提取每一条参考文献的信息。")
        sb.AppendLine()
        sb.AppendLine("需要提取的字段（每条参考文献）：")
        sb.AppendLine("- title：参考文献的标题")
        sb.AppendLine("- doi：DOI（如果没有则填空字符串 """"）")
        sb.AppendLine("- year：发表年份（字符串，如果没有则填空字符串 """"）")
        sb.AppendLine("- journal：期刊名称（如果没有则填空字符串 """"）")
        sb.AppendLine()
        sb.AppendLine("请仅返回一个 JSON 数组，不要包含任何其他文字、解释或 markdown 代码块标记。格式如下：")
        sb.AppendLine("[{""title"":""..."",""doi"":""..."",""year"":""..."",""journal"":""...""}, ...]")
        sb.AppendLine()
        sb.AppendLine("如果没有找到任何参考文献，请返回空数组 []。")
        sb.AppendLine()
        sb.AppendLine("========== PDF 全文文本开始 ==========")
        sb.AppendLine(rawText)
        sb.AppendLine("========== PDF 全文文本结束 ==========")
        Return sb.ToString()
    End Function

    Private Function BuildFullTextPrompt(rawText As String) As String
        Dim sb As New StringBuilder()
        sb.AppendLine("你是一个科技文献全文清理与格式化助手。请对以下从 PDF 提取的原始文本进行清理和格式化，生成干净清爽的 Markdown 格式全文。")
        sb.AppendLine()
        sb.AppendLine("清理与格式化要求：")
        sb.AppendLine("1. 去除乱码、OCR 错误字符、PDF 提取产生的特殊符号和乱码")
        sb.AppendLine("2. 修复断裂的段落和句子，将错误换行处合并为完整段落")
        sb.AppendLine("3. 使用 Markdown 标题层级组织文档结构（# 一级标题、## 二级标题、### 三级标题等）")
        sb.AppendLine("4. 保留原文的章节结构（如 Abstract、Introduction、Methods、Results、Discussion、Conclusion 等）")
        sb.AppendLine("5. 去除页码、页眉、页脚等非正文内容")
        sb.AppendLine("6. 适当格式化公式、表格、图片引用等")
        sb.AppendLine("7. 保留所有原文内容，不要摘要、缩写或省略任何部分")
        sb.AppendLine("8. 输出纯 Markdown 文本，不要包含任何解释性文字或元信息")
        sb.AppendLine()
        sb.AppendLine("请直接输出清理后的 Markdown 全文，不要使用代码块标记包裹。")
        sb.AppendLine()
        sb.AppendLine("========== PDF 原始文本开始 ==========")
        sb.AppendLine(rawText)
        sb.AppendLine("========== PDF 原始文本结束 ==========")
        Return sb.ToString()
    End Function

    ' =========================================================================
    '  输出文件写入
    ' =========================================================================

    ''' <summary>
    ''' 将提取结果写入固定格式的 .txt 文件。
    ''' </summary>
    Private Function WriteOutputFile(
        title As String,
        metadataJson As String,
        referencesJson As String,
        markdown As String
    ) As String
        ' 确保标题为单行（替换所有换行符为空格）
        title = If(title, "").Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()

        ' 确保 JSON 为单行（防止 LLM 返回带换行的 JSON）
        metadataJson = If(metadataJson, "{}").Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()
        referencesJson = If(referencesJson, "[]").Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()

        ' 确保 Markdown 不以空白行开头（保证第 5 行即正文起始）
        markdown = If(markdown, "").Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).TrimStart()


        ' 写入文件（UTF-8 无 BOM）
        Dim sb As New StringBuilder

        Using writer As New StringWriter(sb)
            writer.WriteLine(title)            ' 第 1 行：标题
            writer.WriteLine(metadataJson)     ' 第 2 行：元数据 JSON
            writer.WriteLine(referencesJson)   ' 第 3 行：参考文献 JSON
            writer.WriteLine()                 ' 第 4 行：空白行
            writer.Write(markdown)             ' 第 5 行起：Markdown 全文
            ' 确保文件以换行符结尾
            If Not markdown.EndsWith(vbLf) Then
                writer.WriteLine()
            End If
        End Using

        Return sb.ToString
    End Function
End Module
