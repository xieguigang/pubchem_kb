Imports System
Imports DocumentEngine
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Module Program
    Sub Main(args As String())
        Call buildDb()

        Pause()
    End Sub

    Private Sub buildDb()
        Dim articles = PubMed.ParseArticles("G:\metagenomics-llms\tools\pathgen_pubmed\pubmed-SevereAcut-set.txt".ReadAllText).ToArray
        Dim db As New DocumentDb(App.HOME & "/test_pubmed")

        For Each article As PubmedArticle In articles
            Call db.Add(article)
        Next

        Call db.Save()
    End Sub
End Module
