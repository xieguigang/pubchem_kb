Public Class FormAbout

    Private Sub FormAbout_Click(sender As Object, e As EventArgs) Handles Me.Click, PictureBox1.Click
        Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked, PictureBox2.Click
        Call Process.Start("https://github.com/xieguigang/MathLab")
    End Sub
End Class