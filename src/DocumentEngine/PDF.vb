Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Text.Encodings.Web
Imports System.Threading
Imports System.Threading.Tasks

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
Module Program

    ' JSON 序列化选项：不转义非 ASCII 字符（如中文），不缩进（单行输出）
    Private ReadOnly JsonOpts As New JsonSerializerOptions With {
        .Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        .WriteIndented = False
    }

    ' 全局取消令牌（支持 Ctrl+C 取消）
    Private _cts As New CancellationTokenSource()

    ' =========================================================================
    '  入口
    ' =========================================================================

    Function Main(args As String()) As Integer
        ' 处理 Ctrl+C 取消
        AddHandler Console.CancelKeyPress, Sub(s, e)
                                               e.Cancel = True
                                               _cts.Cancel()
                                           End Sub

        Try
            Return MainAsync(args, _cts.Token).GetAwaiter().GetResult()
        Catch ex As OperationCanceledException
            Console.Error.WriteLine("[CANCELLED] 操作已被用户取消。")
            Return -2
        Catch ex As Exception
            Console.Error.WriteLine($"[FATAL] {ex.Message}")
            Console.Error.WriteLine(ex.StackTrace)
            Return -1
        End Try
    End Function

    Private Async Function MainAsync(args As String(), ct As CancellationToken) As Task(Of Integer)
        ' ========== 解析命令行参数 ==========
        If args.Length < 1 Then
            PrintUsage()
            Return -1
        End If

        Dim pdfPath As String = args(0)
        Dim outPath As String = If(args.Length >= 2,
                                   args(1),
                                   Path.ChangeExtension(pdfPath, ".txt"))

        If Not File.Exists(pdfPath) Then
            Console.Error.WriteLine($"[ERROR] PDF 文件不存在: {pdfPath}")
            Return -1
        End If

        Console.WriteLine($"[INFO] 输入 PDF : {pdfPath}")
        Console.WriteLine($"[INFO] 输出文件 : {outPath}")
        Console.WriteLine()

        ' ========== Step 1: 从 PDF 提取全文 ==========
        Console.WriteLine("[STEP 1] 正在从 PDF 提取全文文本...")
        Dim rawText As String
        Using fs As New FileStream(pdfPath, FileMode.Open, FileAccess.Read, FileShare.Read)
            rawText = PDF.GetText(fs)
        End Using

        If String.IsNullOrWhiteSpace(rawText) Then
            Console.Error.WriteLine("[ERROR] PDF 中未提取到任何文本。")
            Return -1
        End If
        Console.WriteLine($"[INFO] 已提取 {rawText.Length} 个字符。")
        Console.WriteLine()

        ' ========== Step 2~4: 使用 LLM 提取信息 ==========
        Using llm As New LLMClient()

            ' --- Step 2: 提取元数据 ---
            Console.WriteLine("[STEP 2] 正在请求 LLM 提取文献元数据（标题、DOI、年份、期刊、关键词）...")
            Dim title As String = ""
            Dim metadataJson As String = "{}"
            Try
                Dim metaResult = Await ExtractMetadataAsync(llm, rawText, ct)
                title = metaResult.Title
                metadataJson = metaResult.MetadataJson
                Console.WriteLine($"[INFO] 标题  : {title}")
                Console.WriteLine($"[INFO] 元数据: {metadataJson}")
            Catch ex As Exception
                Console.Error.WriteLine($"[WARN] 元数据提取失败: {ex.Message}")
            End Try
            Console.WriteLine()

            ' --- Step 3: 提取参考文献 ---
            Console.WriteLine("[STEP 3] 正在请求 LLM 提取参考文献列表...")
            Dim referencesJson As String = "[]"
            Try
                referencesJson = Await ExtractReferencesAsync(llm, rawText, ct)
                Console.WriteLine($"[INFO] 参考文献已提取。")
            Catch ex As Exception
                Console.Error.WriteLine($"[WARN] 参考文献提取失败: {ex.Message}")
            End Try
            Console.WriteLine()

            ' --- Step 4: 清理全文为 Markdown ---
            Console.WriteLine("[STEP 4] 正在请求 LLM 清理全文并格式化为 Markdown...")
            Dim markdown As String = rawText
            Try
                markdown = Await CleanFullTextAsync(llm, rawText, ct)
                Console.WriteLine($"[INFO] Markdown 全文已生成。")
            Catch ex As Exception
                Console.Error.WriteLine($"[WARN] 全文清理失败，将使用原始文本: {ex.Message}")
            End Try
            Console.WriteLine()

            ' ========== Step 5: 写入输出文件 ==========
            Console.WriteLine("[STEP 5] 正在写入输出文件...")
            WriteOutputFile(outPath, title, metadataJson, referencesJson, markdown)
            Console.WriteLine($"[DONE] 输出已写入: {outPath}")
        End Using

        Return 0
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
        Using doc As JsonDocument = JsonDocument.Parse(jsonStr)
            Dim root = doc.RootElement
            Dim title As String = GetStringProperty(root, "title")
            Dim doi As String = GetStringProperty(root, "doi")
            Dim year As String = GetStringProperty(root, "year")
            Dim journal As String = GetStringProperty(root, "journal")

            Dim keywords As New List(Of String)()
            Dim kwEl As JsonElement
            If root.TryGetProperty("keywords", kwEl) AndAlso kwEl.ValueKind = JsonValueKind.Array Then
                For Each kw In kwEl.EnumerateArray()
                    If kw.ValueKind = JsonValueKind.String Then
                        keywords.Add(kw.GetString())
                    End If
                Next
            End If

            ' 构建不含 title 的元数据 JSON（符合输出格式第 2 行要求）
            Dim metaDict As New Dictionary(Of String, Object) From {
                {"doi", doi},
                {"year", year},
                {"journal", journal},
                {"keywords", keywords}
            }
            Dim metadataJson As String = JsonSerializer.Serialize(metaDict, JsonOpts)

            Return (title, metadataJson)
        End Using
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
        Try
            Using doc As JsonDocument = JsonDocument.Parse(jsonStr)
                Dim refs As New List(Of Object)()
                If doc.RootElement.ValueKind = JsonValueKind.Array Then
                    For Each refEl In doc.RootElement.EnumerateArray()
                        Dim refDict As New Dictionary(Of String, String) From {
                            {"title", GetStringProperty(refEl, "title")},
                            {"doi", GetStringProperty(refEl, "doi")},
                            {"year", GetStringProperty(refEl, "year")},
                            {"journal", GetStringProperty(refEl, "journal")}
                        }
                        refs.Add(refDict)
                    Next
                End If
                Return JsonSerializer.Serialize(refs, JsonOpts)
            End Using
        Catch ex As Exception
            Console.Error.WriteLine($"[WARN] 参考文献 JSON 解析失败: {ex.Message}")
            Return "[]"
        End Try
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
    Private Sub WriteOutputFile(
        outPath As String,
        title As String,
        metadataJson As String,
        referencesJson As String,
        markdown As String
    )
        ' 确保标题为单行（替换所有换行符为空格）
        title = If(title, "").Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()

        ' 确保 JSON 为单行（防止 LLM 返回带换行的 JSON）
        metadataJson = If(metadataJson, "{}").Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()
        referencesJson = If(referencesJson, "[]").Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Trim()

        ' 确保 Markdown 不以空白行开头（保证第 5 行即正文起始）
        markdown = If(markdown, "").Replace(vbCrLf, vbLf).Replace(vbCr, vbLf).TrimStart()

        ' 确保输出目录存在
        Dim dir As String = Path.GetDirectoryName(outPath)
        If Not String.IsNullOrEmpty(dir) AndAlso Not Directory.Exists(dir) Then
            Directory.CreateDirectory(dir)
        End If

        ' 写入文件（UTF-8 无 BOM）
        Using writer As New StreamWriter(outPath, False, New UTF8Encoding(False))
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
    End Sub

    ' =========================================================================
    '  辅助方法
    ' =========================================================================

    ''' <summary>
    ''' 从 JsonElement 中安全地获取字符串属性，处理字符串、数字等类型。
    ''' </summary>
    Private Function GetStringProperty(root As JsonElement, name As String) As String
        Dim el As JsonElement
        If root.TryGetProperty(name, el) Then
            Select Case el.ValueKind
                Case JsonValueKind.String
                    Return el.GetString()
                Case JsonValueKind.Number
                    Return el.GetRawText()
                Case JsonValueKind.True
                    Return "true"
                Case JsonValueKind.False
                    Return "false"
                Case Else
                    Return ""
            End Select
        End If
        Return ""
    End Function

    Private Sub PrintUsage()
        Console.WriteLine("PDF 文献提取工具")
        Console.WriteLine()
        Console.WriteLine("用法:")
        Console.WriteLine("  PdfExtractor <输入PDF路径> [输出TXT路径]")
        Console.WriteLine()
        Console.WriteLine("参数:")
        Console.WriteLine("  输入PDF路径  要处理的 PDF 文件路径")
        Console.WriteLine("  输出TXT路径  输出的 .txt 文件路径（可选，默认与 PDF 同名 .txt）")
        Console.WriteLine()
        Console.WriteLine("输出 .txt 格式:")
        Console.WriteLine("  第 1 行  : title（文献标题）")
        Console.WriteLine("  第 2 行  : 元数据 JSON {""doi"":..,""year"":..,""journal"":..,""keywords"":[..]}")
        Console.WriteLine("  第 3 行  : 参考文献数组 JSON [{""title"":..,""doi"":..,""year"":..,""journal"":..}, ...]")
        Console.WriteLine("  第 4 行  : 空白行")
        Console.WriteLine("  第 5 行起: 经格式化、清理乱码后的文献全文 markdown 文本内容")
    End Sub

End Module
