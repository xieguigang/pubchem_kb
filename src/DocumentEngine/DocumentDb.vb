
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

Public Class DocumentDb : Implements IDisposable

    Dim documentDb As Buckets
    Dim fts As InvertedIndex
    Dim dir As String

    Private disposedValue As Boolean

    Private ReadOnly Property indexfile As String
        Get
            Return $"{dir}/fulltext.dat"
        End Get
    End Property

    Sub New(db As String, Optional [readonly] As Boolean = False, Optional in_memory As Boolean = False)
        dir = db
        documentDb = New Buckets(db, [readonly]:=[readonly], in_memory:=in_memory)
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

    Private Function Save() As Boolean
        Using s As Stream = indexfile.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False)
            Call documentDb.Flush()
            Call FullTextBuffer.WriteIndex(fts, {}, s)
        End Using

        Return Nothing
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                Call Save()
                Call documentDb.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
