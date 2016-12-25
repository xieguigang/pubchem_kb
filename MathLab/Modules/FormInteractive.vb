Imports Microsoft.VisualBasic.Mathematical

Public Class FormInteractive

    Dim engine As New Expression

    Private Sub ConsoleControl1_LineEntered(sender As Object, line As String) Handles ConsoleControl1.LineEntered
        Call ConsoleControl1.Write(vbTab & "= " & engine.Evaluation(line))
        Call ShowPrompt(True)
    End Sub

    Private Sub FormInteractive_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call ShowPrompt(False)
    End Sub

    Private Sub ShowPrompt(newLine As Boolean)
        If newLine Then
            Call ConsoleControl1.Write()
        End If
        ConsoleControl1.Write("# ")
    End Sub
End Class