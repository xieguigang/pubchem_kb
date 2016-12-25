Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv

Public Class DataImports

    Dim url$

    Public Shared Function GetData(url$, ByRef x#()) As NamedValue(Of Double())()
        Using dataImports As New DataImports With {
            .url = url
        }
            With dataImports
                .Text = $"[Data Imports] {url}"

                If .ShowDialog() = DialogResult.Cancel Then
                    Return Nothing
                Else
                    Dim data As New List(Of NamedValue(Of Double()))(LoadData(url))

                    If .CheckBox1.Checked Then
                        x = data.First.Value
                        data.RemoveAt(Scan0)
                        Return data.ToArray
                    Else
                        x = data.First.Value.Sequence.Select(Function(n) CDbl(n)).ToArray
                        Return data.ToArray
                    End If
                End If
            End With
        End Using
    End Function
End Class