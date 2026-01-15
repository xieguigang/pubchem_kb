Imports DocumentEngine
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Module Program
    Sub Main(args As String())
        Call buildDb()
        Call queryTest()

        Pause()
    End Sub

    Private Sub queryTest()
        Dim db As New DocumentDb(App.HOME & "/test_pubmed")
        Dim result = db.QueryTable("SARS-CoV-2").ToArray

        Pause()
    End Sub

    Private Sub buildDb()
        Dim articles = PubMed.ParseArticles("G:\metagenomics-llms\tools\pathgen_pubmed\pubmed-SevereAcut-set.txt".ReadAllText).ToArray
        Dim db As New DocumentDb(App.HOME & "/test_pubmed")

        For Each article As PubmedArticle In TqdmWrapper.Wrap(articles)
            Call db.Add(article)
        Next

        Call db.Dispose()
    End Sub
End Module
