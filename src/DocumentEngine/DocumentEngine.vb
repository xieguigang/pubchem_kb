
' pubmed - {title, abstract, doi}

'           key -> value  
' bucketdb: doi -> pubmed
'
' fulltext: abstract -> doi
'

Imports Darwinism.Repository.BucketDb
Imports LINQ

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

    Public Sub Add()

    End Sub

End Class
