Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv

Public Class DataImports

    Dim url$

    Public Shared Function GetData(url$) As NamedValue(Of Double())()
        Using dataImports As New DataImports With {
            .url = url
        }
            With dataImports
                .Text = $"[Data Imports] {url}"

                If .ShowDialog() = DialogResult.Cancel Then
                    Return Nothing
                Else
                    Return LoadData(url, .CheckBox1.Checked)
                End If
            End With
        End Using
    End Function
End Class