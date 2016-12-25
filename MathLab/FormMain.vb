Public Class FormMain

    Private Sub DataCorrelationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataCorrelationsToolStripMenuItem.Click
        Call New FormCorrealtions With {
            .MdiParent = Me
        }.Show()
    End Sub
End Class