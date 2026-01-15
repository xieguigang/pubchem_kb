
' pubmed - {title, abstract, pmid}

'           key -> value  
' bucketdb: doi -> pubmed
'
' fulltext: abstract -> pmid
'           title -> pmid

Imports System.IO
Imports Darwinism.Repository.BucketDb
Imports LINQ
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports SMRUCC.genomics.GCModeller.Workbench.Knowledge_base.NCBI.PubMed

Public Class DocumentDb

    Dim documentDb As Buckets
    Dim fts As InvertedIndex
    Dim dir As String

    Private ReadOnly Property indexfile As String
        Get
            Return $"{dir}/fulltext.dat"
        End Get
    End Property

    Sub New(db As String)
        dir = db
        documentDb = New Buckets(db)
        fts = LoadIndex()
    End Sub

    Private Function LoadIndex() As InvertedIndex
        Dim indexfile As String = Me.indexfile

        If indexfile.FileLength > 0 Then
            Return FullTextBuffer.ReadIndex(indexfile.OpenReadonly, Nothing)
        Else
            Return New InvertedIndex
        End If
    End Function

    Public Sub Add(article As PubmedArticle)
        Dim abstract As String = article.GetAbstractText
        Dim title As String = article.GetTitle
        Dim id As Integer = CLng(article.PMID) + Integer.MinValue

        Call fts.Add(title, id)
        Call fts.Add(abstract, id)

        Call documentDb.Put(article.PMID, article.GetXml)
    End Sub

    Public Iterator Function Query(term As String) As IEnumerable(Of PubmedArticle)
        Dim q = fts.Search(term)

        If q Is Nothing Then
            Return
        End If

        For Each pmid As Integer In q
            Dim key_str As String = (CLng(pmid) - Integer.MinValue).ToString
            Dim xml As String = documentDb.GetString(key_str)
            Dim article As PubmedArticle = xml.LoadFromXml(Of PubmedArticle)

            Yield article
        Next
    End Function

    Public Function QueryTable(term As String) As PubMedTextTable()
        Dim list As PubmedArticle() = Query(term).ToArray
        Dim table As PubMedTextTable() = list _
            .Select(Function(a)
                        Return New PubMedTextTable With {
                            .pmid = a.PMID,
                            .articleabstract = a.GetAbstractText,
                            .articletitle = a.GetTitle,
                            .articlejourname = a.GetJournal,
                            .doi = a.GetArticleDoi,
                            .articleauth = a.GetAuthors.JoinBy("; "),
                            .meshheadings = a.GetMeshTerms.Keys
                        }
                    End Function) _
            .ToArray

        Return table
    End Function

    Public Function Save() As Boolean
        Using s As Stream = indexfile.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
            Call documentDb.Flush()
            Call FullTextBuffer.WriteIndex(fts, {}, s)
        End Using

        Return Nothing
    End Function

End Class
