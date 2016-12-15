Imports Microsoft.VisualBasic.Imaging.Drawing3D

Public Class Form3DViewer

    Dim model As (sf As Surface, c As Double())()

    Private Sub LoadMatrixToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadMatrixToolStripMenuItem.Click
        Using open As New OpenFileDialog With {
            .Filter = "Excel Data(*.csv)|(*.csv)"
        }
            If open.ShowDialog = DialogResult.OK Then

            End If
        End Using
    End Sub
End Class
