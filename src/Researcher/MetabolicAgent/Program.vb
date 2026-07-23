' ============================================================================
' Program.vb - 天然产物代谢知识库构建Agent 命令行程序入口
'
' 程序功能：
'   用户通过命令行参数以自然语言描述研究主题，程序利用本地Ollama LLM服务
'   理解研究主题，自动从PubMed本地镜像MySQL数据库中检索相关文献，
'   逐篇阅读全文并提取9字段代谢反应信息，构建结构化的代谢知识库。
'
' 使用方法：
'   MetabolicAgent.exe --topic "研究主题" [选项]
'
' 命令行参数：
'   --topic       (必需) 研究主题的自然语言描述
'   --model       (可选) Ollama模型名称，默认 "qwen2.5:14b"
'   --endpoint    (可选) Ollama服务地址，默认 "http://localhost:11434"
'   --mysql       (可选) MySQL连接字符串
'   --output      (可选) 输出目录，默认 "./output"
'   --max-papers  (可选) 最大处理文献数，默认 20
'   --help        显示帮助信息
'
' 示例：
'   MetabolicAgent.exe --topic "青蒿素生物合成途径及相关酶" --model "qwen2.5:14b" --max-papers 30
' ============================================================================
Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports MetabolicAgent
Imports Ollama

Module Program

    ' 默认配置值
    Private Const DEFAULT_MODEL As String = "qwen2.5:14b"
    Private Const DEFAULT_ENDPOINT As String = "http://localhost:11434"
    Private Const DEFAULT_OUTPUT As String = "./output"
    Private Const DEFAULT_MAX_PAPERS As Integer = 20
    Private Const DEFAULT_MYSQL As String = "server=localhost;database=pubmed;uid=root;pwd=1234;Charset=utf8mb4;"

    ''' <summary>
    ''' 程序主入口
    ''' </summary>
    Function Main(args As String()) As Integer
        ' 解析命令行参数
        Dim config = ParseCommandLineArgs(args)

        ' 显示帮助信息
        If config.showHelp Then
            ShowHelp()
            Return 0
        End If

        ' 验证必需参数
        If String.IsNullOrWhiteSpace(config.topic) Then
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("错误: 缺少必需参数 --topic")
            Console.ResetColor()
            Console.WriteLine()
            Console.WriteLine("使用 --help 查看帮助信息")
            Return 1
        End If

        ' 异步执行主流程
        Try
            Return RunAgentAsync(config).GetAwaiter().GetResult()
        Catch ex As OperationCanceledException
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine(vbCrLf & "任务已取消")
            Console.ResetColor()
            Return 130
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(vbCrLf & $"程序运行出错: {ex.Message}")
            Console.WriteLine(ex.StackTrace)
            Console.ResetColor()
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' 异步执行agent主流程
    ''' </summary>
    Private Async Function RunAgentAsync(config As AppConfig) As Task(Of Integer)
        ' 创建CancellationTokenSource，支持Ctrl+C取消
        Using cts As New CancellationTokenSource()
            ' 注册Ctrl+C处理
            AddHandler Console.CancelKeyPress, Sub(sender, e)
                                                   e.Cancel = True
                                                   cts.Cancel()
                                                   Console.WriteLine(vbCrLf & "正在取消任务，请稍候...")
                                               End Sub

            ' 初始化LLM客户端
            Console.WriteLine($"正在初始化LLM客户端...")
            Console.WriteLine($"  模型: {config.model}")
            Console.WriteLine($"  端点: {config.endpoint}")
            Console.WriteLine()

            ' 注意：LLMClient的构造函数签名请根据您的实际Ollama模块进行调整
            ' 这里假设构造函数接受模型名称和端点URL
            Using llmClient As LLMClient = New LLMClient(LLMUrl.Create(config.endpoint))
                ' 创建并运行agent
                Using agent As New MetabolicAgent(
                    llmClient,
                    config.mysqlConnectionString,
                    config.outputDir)

                    Await agent.RunAsync(
                        config.topic,
                        config.maxPapers,
                        cts.Token)
                End Using
            End Using

            Return 0
        End Using
    End Function

    ' ========================================================================
    ' 命令行参数解析
    ' ========================================================================

    ''' <summary>
    ''' 应用配置类
    ''' </summary>
    Private Class AppConfig
        Public Property topic As String = ""
        Public Property model As String = DEFAULT_MODEL
        Public Property endpoint As String = DEFAULT_ENDPOINT
        Public Property mysqlConnectionString As String = DEFAULT_MYSQL
        Public Property outputDir As String = DEFAULT_OUTPUT
        Public Property maxPapers As Integer = DEFAULT_MAX_PAPERS
        Public Property showHelp As Boolean = False
    End Class

    ''' <summary>
    ''' 解析命令行参数
    ''' 支持 --key value 和 --key=value 两种格式
    ''' </summary>
    Private Function ParseCommandLineArgs(args As String()) As AppConfig
        Dim config As New AppConfig()

        ' 如果第一个参数不是以--开头，则视为topic
        If args.Length > 0 AndAlso Not args(0).StartsWith("-") Then
            config.topic = String.Join(" ", args.TakeWhile(Function(a) Not a.StartsWith("-")))
            args = args.SkipWhile(Function(a) Not a.StartsWith("-")).ToArray()
        End If

        Dim i As Integer = 0
        While i < args.Length
            Dim arg = args(i)

            Select Case arg.ToLower()
                Case "--help", "-h", "/?"
                    config.showHelp = True

                Case "--topic", "-t"
                    If i + 1 < args.Length Then
                        config.topic = args(i + 1)
                        i += 1
                    End If

                Case "--model", "-m"
                    If i + 1 < args.Length Then
                        config.model = args(i + 1)
                        i += 1
                    End If

                Case "--endpoint", "-e"
                    If i + 1 < args.Length Then
                        config.endpoint = args(i + 1)
                        i += 1
                    End If

                Case "--mysql"
                    If i + 1 < args.Length Then
                        config.mysqlConnectionString = args(i + 1)
                        i += 1
                    End If

                Case "--output", "-o"
                    If i + 1 < args.Length Then
                        config.outputDir = args(i + 1)
                        i += 1
                    End If

                Case "--max-papers"
                    If i + 1 < args.Length Then
                        Integer.TryParse(args(i + 1), config.maxPapers)
                        i += 1
                    End If

                Case Else
                    ' 处理 --key=value 格式
                    If arg.Contains("="c) Then
                        Dim eqIdx = arg.IndexOf("="c)
                        Dim key = arg.Substring(0, eqIdx).ToLower()
                        Dim value = arg.Substring(eqIdx + 1)

                        Select Case key
                            Case "--topic", "-t" : config.topic = value
                            Case "--model", "-m" : config.model = value
                            Case "--endpoint", "-e" : config.endpoint = value
                            Case "--mysql" : config.mysqlConnectionString = value
                            Case "--output", "-o" : config.outputDir = value
                            Case "--max-papers" : Integer.TryParse(value, config.maxPapers)
                        End Select
                    End If
            End Select

            i += 1
        End While

        ' 确保maxPapers在合理范围
        config.maxPapers = Math.Max(1, Math.Min(config.maxPapers, 100))

        Return config
    End Function

    ''' <summary>
    ''' 显示帮助信息
    ''' </summary>
    Private Sub ShowHelp()
        Console.WriteLine()
        Console.WriteLine("天然产物代谢知识库构建Agent")
        Console.WriteLine(New String("="c, 60))
        Console.WriteLine()
        Console.WriteLine("功能描述:")
        Console.WriteLine("  用户通过自然语言描述研究主题，程序利用本地Ollama LLM服务")
        Console.WriteLine("  理解研究主题，自动从PubMed本地镜像MySQL数据库检索文献，")
        Console.WriteLine("  逐篇阅读全文并提取9字段代谢反应信息，构建结构化代谢知识库。")
        Console.WriteLine()
        Console.WriteLine("用法:")
        Console.WriteLine("  MetabolicAgent.exe --topic ""研究主题"" [选项]")
        Console.WriteLine()
        Console.WriteLine("参数说明:")
        Console.WriteLine("  --topic       (必需) 研究主题的自然语言描述")
        Console.WriteLine("  --model       (可选) Ollama模型名称")
        Console.WriteLine($"                  默认: {DEFAULT_MODEL}")
        Console.WriteLine("  --endpoint    (可选) Ollama服务地址")
        Console.WriteLine($"                  默认: {DEFAULT_ENDPOINT}")
        Console.WriteLine("  --mysql       (可选) MySQL连接字符串")
        Console.WriteLine($"                  默认: {DEFAULT_MYSQL}")
        Console.WriteLine("  --output      (可选) 代谢反应JSON输出目录")
        Console.WriteLine($"                  默认: {DEFAULT_OUTPUT}")
        Console.WriteLine("  --max-papers  (可选) 最大处理文献数 (1-100)")
        Console.WriteLine($"                  默认: {DEFAULT_MAX_PAPERS}")
        Console.WriteLine("  --help        显示本帮助信息")
        Console.WriteLine()
        Console.WriteLine("提取的代谢反应包含9个字段:")
        Console.WriteLine("  1. substrates        - 底物英文名称列表")
        Console.WriteLine("  2. products          - 产物英文名称列表")
        Console.WriteLine("  3. reaction_name     - 反应名称")
        Console.WriteLine("  4. reaction_description - 反应描述")
        Console.WriteLine("  5. enzyme            - 酶信息(基因id/名称/ec_number/结构域)")
        Console.WriteLine("  6. pathway           - 代谢通路名称")
        Console.WriteLine("  7. source_organisms  - 来源物种学名列表")
        Console.WriteLine("  8. source_doi        - 来源文献DOI")
        Console.WriteLine("  9. source_title      - 来源文献标题")
        Console.WriteLine()
        Console.WriteLine("示例:")
        Console.WriteLine("  MetabolicAgent.exe --topic ""青蒿素生物合成途径及相关酶"" --max-papers 30")
        Console.WriteLine()
        Console.WriteLine("  MetabolicAgent.exe --topic ""紫杉醇生物合成"" --model ""llama3.1:8b"" --output ""./taxol_reactions""")
        Console.WriteLine()
        Console.WriteLine("输出:")
        Console.WriteLine("  每篇文献提取的代谢反应数组保存为独立的JSON文件，")
        Console.WriteLine("  文件名格式: {PMID}_{文献标题}.json")
        Console.WriteLine()
    End Sub

End Module
