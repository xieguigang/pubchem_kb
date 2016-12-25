Public Class FormAbout

    Private Sub FormAbout_Click(sender As Object, e As EventArgs) Handles Me.Click
        Close()
    End Sub

    Private Sub FormAbout_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Call Process.Start("https://github.com/xieguigang/MathLab")
    End Sub
End Class