Public Class FormMain

    Private Sub DataCorrelationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataCorrelationsToolStripMenuItem.Click
        Call New FormCorrealtions With {
            .MdiParent = Me
        }.Show()
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem1.Click
        Call New FormAbout().ShowDialog()
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call New FormInteractive() With {
            .MdiParent = Me
        }.Show()
    End Sub

    Private Sub InterativeWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InterativeWindowsToolStripMenuItem.Click
        Call New FormInteractive() With {
            .MdiParent = Me
        }.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub DynamicsAnalysisToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DynamicsAnalysisToolStripMenuItem.Click
        Call New FormRunModel() With {
            .MdiParent = Me
        }.Show()
    End Sub

    Private Sub ParameterHelperToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ParameterHelperToolStripMenuItem.Click
        Call New FormRunModelAssembly With {
            .MdiParent = Me
        }.Show()
    End Sub
End Class