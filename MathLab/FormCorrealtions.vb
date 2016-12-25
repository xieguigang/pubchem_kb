Public Class FormCorrealtions

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Call Close()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Using file As New OpenFileDialog With {
            .Filter = "Excel Data(*.csv)|*.csv"
        }
            If file.ShowDialog = DialogResult.OK Then

            End If
        End Using
    End Sub
End Class