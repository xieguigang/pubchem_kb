Imports DocumentEngine
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar.Tqdm
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Module Program
    Sub Main(args As String())
        Call researchtest()

        ' Call buildDb()
        ' Call queryTest()

        Pause()
    End Sub

    Private Sub researchtest()
        Dim db As New DocumentDb(App.HOME & "/test_pubmed", [readonly]:=True, in_memory:=True)
        Dim ollama As New Ollama.Ollama("qwen3:30b")
        Dim researcher As New Researcher(ollama, db)
        Dim result = researcher.Ask("introduce Yersinia pestis to me").GetAwaiter.GetResult

        Pause()
    End Sub

    Private Sub queryTest()
        Dim db As New DocumentDb(App.HOME & "/test_pubmed", [readonly]:=True, in_memory:=True)
        Dim result = db.QueryTable("Yersinia pestis ").ToArray

        Pause()
    End Sub

    Private Sub buildDb()
        Dim db As New DocumentDb(App.HOME & "/test_pubmed")

        For Each file As String In "G:\metagenomics-llms\tools\pathgen_pubmed".ListFiles("*.txt")
            Dim articles = PubMed.ParseArticles(file.ReadAllText).ToArray

            For Each article As PubmedArticle In TqdmWrapper.Wrap(articles)
                Call db.Add(article)
            Next
        Next

        Call db.Dispose()
    End Sub
End Module
