Imports MathLab.Scripting
Imports MathLab.TextEditor
Imports Microsoft.VisualBasic.Mathematical.Calculus

Public Class FormRunModel

    Dim WithEvents textEditor As FastColoredTextBox

    Private Sub FormRunModel_Load(sender As Object, e As EventArgs) Handles Me.Load
        textEditor = New FastColoredTextBox With {
            .Dock = DockStyle.Fill,
            .Language = Language.VB
        }
        TabPage1.Controls.Add(textEditor)
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Using file As New OpenFileDialog With {
            .Filter = "sciBASIC# dynamics script(*.vb)|*.vb"
        }
            If file.ShowDialog = DialogResult.OK Then
                textEditor.Text = file.FileName.ReadAllText
            End If
        End Using
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim model As Dynamics = Parser.LoadScript(textEditor.Text)
        Dim result As ODEsOut = model.RunTest

    End Sub
End Class