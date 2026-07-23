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
Imports System.Threading
Imports Ollama
Imports Researcher.MetabolicAgent

Module AgentWorkflow

    ''' <summary>
    ''' 异步执行agent主流程
    ''' </summary>
    Public Async Function RunAgentAsync(topic As String, config As ResearchAgentConfig) As Task(Of Integer)
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
            Console.WriteLine($"  模型: {config.ModelName}")
            Console.WriteLine($"  端点: {config.OllamaEndpoint}")
            Console.WriteLine()

            ' 注意：LLMClient的构造函数签名请根据您的实际Ollama模块进行调整
            ' 这里假设构造函数接受模型名称和端点URL
            Using llmClient As LLMClient = New LLMClient(LLMUrl.Create(config.OllamaEndpoint), config.ModelName, maxRound:=config.MaxRounds)
                ' 创建并运行agent
                Using agent As New MetabolicLLMAgent(
                    llmClient,
                    config.DatabaseConnectionString,
                    config.OutputDirectory)

                    Await agent.RunAsync(
                        topic,
                        config.PapersPerQuery,
                        cts.Token)
                End Using
            End Using

            Return 0
        End Using
    End Function

End Module
