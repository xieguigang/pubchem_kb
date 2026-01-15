
' pubmed - {title, abstract, pmid}

'           key -> value  
' bucketdb: doi -> pubmed
'
' fulltext: abstract -> pmid
'           title -> pmid

Imports Darwinism.Repository.BucketDb
Imports LINQ
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Public Class DocumentEngine

    Dim documentDb As Buckets
    Dim index As InvertedIndex
    Dim dir As String

    Sub New(db As String)
        dir = db
        documentDb = New Buckets(db)
        index = LoadIndex()
    End Sub

    Private Function LoadIndex() As InvertedIndex
        Dim indexfile As String = $"{dir}/fulltext.dat"

        If indexfile.FileLength > 0 Then
            Return FullTextBuffer.ReadIndex(indexfile.OpenReadonly, Nothing)
        Else
            Return New InvertedIndex
        End If
    End Function

    Public Sub Add(article As PubmedArticle)
        Dim abstract As String = article.GetAbstractText
        Dim title As String = article.GetTitle
        Dim id As Integer = CInt(article.PMID)

        Call index.Add(title, id)
        Call index.Add(abstract, id)

        Call documentDb.Put(id, article.GetXml)
    End Sub

    Public Iterator Function Query(term As String) As IEnumerable(Of PubmedArticle)
        Dim q = index.Search(term)

        If q Is Nothing Then
            Return
        End If

        For Each pmid As Integer In q
            Dim key_str As String = pmid.ToString
            Dim xml As String = documentDb.GetString(key_str)
            Dim article As PubmedArticle = xml.LoadFromXml(Of PubmedArticle)

            Yield article
        Next
    End Function

End Class
