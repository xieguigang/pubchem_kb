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
        Call ollama.Clear()
        Call ollama.AddSystemPrompt("你需要根据知识库中真实存在的知识信息来组织对用户问题的回答，并附带上从知识库中得到的知识引用信息在你的答案中，以方便用户进行来源信息的查证。")

        Return Await ollama.Chat(question)
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
