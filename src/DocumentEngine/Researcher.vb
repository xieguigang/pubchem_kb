Imports System.ComponentModel
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Ollama
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Public Class Researcher

    ReadOnly ollama As Ollama.Ollama
    ReadOnly pubmed As DocumentDb

    Sub New(ollama As Ollama.Ollama, pubmed As DocumentDb)
        Me.ollama = ollama
        Me.pubmed = pubmed
        Me.ollama.AddFunction(Me, NameOf(query))
    End Sub

    Public Async Function Ask(question As String) As Task(Of DeepSeekResponse)
        Dim prompt As String = $"你需要根据知识库中真实存在的知识信息来组织对我给出的问题进行回答，并且需要你在回答中附带上从知识库中得到的知识引用信息在你的答案末尾，以方便我来进行来源信息的查证。
你对我的问题所做出来的回答应该是按照下面的格式进行组织的，以方便后面可能进行的自动化脚本解析工作：

-----------------------------

<回答内容的文本>

## 引用文献

1. 文献1标题, 文献1杂志名称, 发表年份, doi:文献1的doi编号 [PMID:文献1的pubmed_id]
2. 文献2标题, 文献2杂志名称, 发表年份, doi:文献2的doi编号 [PMID:文献2的pubmed_id]
3. 文献3标题, 文献3杂志名称, 发表年份, doi:文献3的doi编号 [PMID:文献3的pubmed_id]
...

-----------------------------

下面为你需要进行回答的问题：{question}
"
        Return Await ollama.Clear.Chat(prompt)
    End Function

    <Description("使用这个函数进行知识词条的查询，当你遇到知识库中不存在的问题的时候，应该尽量使用这个函数进行查询。这个函数会以json列表的形式返回目前的文献库中的一些结论知识供你做参考。请注意这个函数仅能够用于做给定的一个名词做知识查询，这个函数不能够以自然语言的方式进行查询输入。")>
    <Argument("term", False, CLITypes.String, Description:="需要进行查询的知识词条，这个参数应该尽量是一个名词来的。")>
    Private Function query(term As String) As String
        Dim table = pubmed.QueryTable(term).ToArray
        Dim output As New List(Of Dictionary(Of String, String))

        For Each ref As PubMedTextTable In table
            Call output.Add(New Dictionary(Of String, String) From {
                {"doi", ref.doi},
                {"pubmed_id", ref.pmid},
                {"title", ref.articletitle},
                {"auchors", ref.articleauth},
                {"abstract", ref.articleabstract},
                {"year", ref.articlepubdate},
                {"journal", ref.articlejourname}
            })
        Next

        Return output.ToArray.GetJson
    End Function

End Class
