Imports System.Windows.Forms.DataVisualization.Charting
Imports MathLab.Scripting
Imports MathLab.TextEditor
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
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

    Dim data As ODEsOut
    Dim names As New List(Of String)

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim model As Dynamics = Parser.LoadScript(textEditor.Text)
        Dim result As ODEsOut = model.RunTest

        disableTrigger = True

        Call CheckedListBox1.Items.Clear()
        Call names.Clear()

        For Each y In result.y0.Keys.SeqIterator
            Call CheckedListBox1.Items.Add(y.value)
            Call CheckedListBox1.SetItemChecked(y.i, True)
            Call names.Add(y.value)
        Next

        data = result
        disableTrigger = False

        Call Plot()
    End Sub

    Public Sub Plot()
        Dim vars As New List(Of NamedValue(Of Double()))

        For i As Integer = 0 To CheckedListBox1.CheckedIndices.Count - 1
            vars.Add(data.y(names(CheckedListBox1.CheckedIndices(i))))
        Next

        Call Chart1.Series.Clear()

        For Each y In vars
            Dim s = Chart1.Series.Add(y.Name)
            s.ChartType = SeriesChartType.Line

            For Each x In data.x.SeqIterator
                Call s.Points.AddXY(x.value, y.Value(x))
            Next
        Next
    End Sub

    Dim disableTrigger As Boolean

    Private Sub CheckedListBox1_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        If Not disableTrigger Then
            Call Plot()
        End If
    End Sub

    'Private Sub TabControl1_Selected(sender As Object, e As TabControlEventArgs) Handles TabControl1.Selected
    '    If e.TabPage.Equals(TabPage2) Then
    '        Dim model As Dynamics = Parser.LoadScript(textEditor.Text)

    '    End If
    'End Sub
End Class