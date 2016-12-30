Imports System.ComponentModel

Public Class FormOutputLogs

    Private Sub FormOutputLogs_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        WindowState = FormWindowState.Minimized
        e.Cancel = True
    End Sub
End Class