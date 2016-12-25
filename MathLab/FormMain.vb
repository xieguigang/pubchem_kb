Public Class FormMain

    Private Sub DataCorrelationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataCorrelationsToolStripMenuItem.Click
        Call New FormCorrealtions With {
            .MdiParent = Me
        }.Show()
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem1.Click
        Call New FormAbout().ShowDialog()
    End Sub
End Class