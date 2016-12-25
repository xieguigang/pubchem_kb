Imports MathLab.TextEditor

Public Class FormRunModel

    Dim WithEvents textEditor As FastColoredTextBox

    Private Sub FormRunModel_Load(sender As Object, e As EventArgs) Handles Me.Load
        textEditor = New FastColoredTextBox With {
            .Dock = DockStyle.Fill,
            .Language = Language.VB
        }
        TabPage1.Controls.Add(textEditor)
    End Sub
End Class