Imports Microsoft.VisualBasic.Data.csv.DocumentExtensions
Imports Microsoft.VisualBasic.Linq

Public Class FormCorrealtions

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFile.Click
        Using file As New OpenFileDialog With {
            .Filter = "Excel Data(*.csv)|*.csv"
        }
            If file.ShowDialog = DialogResult.OK Then
                Dim dataset = LoadData(file.FileName)

                For Each col In dataset
                    Call DataGridView1.Columns.Add(col.Name, col.Name)
                    DataGridView1.Columns(col.Name).ValueType = GetType(Double)
                Next

                For i As Integer = 0 To dataset(Scan0).Value.Length - 1
                    Dim index = i
                    Dim row As Object() = dataset _
                        .ToArray(Function(x) CObj(x.Value(index)))
                    Call DataGridView1.Rows.Add(row)
                Next
            End If
        End Using
    End Sub

    Private Sub PlotScatter_Click(sender As Object, e As EventArgs) Handles PlotScatter.Click

    End Sub
End Class