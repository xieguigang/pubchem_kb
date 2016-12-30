Imports System.ComponentModel
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class FormMain
    Implements IDisposable

    Private Sub DataCorrelationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DataCorrelationsToolStripMenuItem.Click
        Call New FormCorrealtions With {
            .MdiParent = Me
        }.Show()
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem1.Click
        Call New FormAbout().ShowDialog()
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        this.Logger = New FormOutputLogs() With {
            .MdiParent = Me
        }
        Call this.Logger.Show()
        Call this.Logging(App.GetAppVariables.GetJson)
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
        this.Closing = True
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

    Private Sub FormMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        this.Closing = True
    End Sub

    Private Sub NetworkVisualizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NetworkVisualizeToolStripMenuItem.Click
        Call New FormNetworkCanvas With {
            .MdiParent = Me
        }.Show()
    End Sub
End Class