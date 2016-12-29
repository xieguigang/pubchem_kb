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

    Dim _file$

    Public Property file As String
        Get
            Return _file
        End Get
        Private Set(value As String)
            _file = value
            Text = value & " - Run Dynamics Model"
        End Set
    End Property

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Using file As New OpenFileDialog With {
            .Filter = "sciBASIC# dynamics script(*.vb)|*.vb"
        }
            If file.ShowDialog = DialogResult.OK Then
                textEditor.Text = file.FileName.ReadAllText
                Me.file = file.FileName
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

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If String.IsNullOrEmpty(file) Then
            Call SaveAs(Nothing, Nothing)
        Else
            Call textEditor.Text.SaveTo(file)
        End If
    End Sub

    Private Sub SaveAs(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Using file As New SaveFileDialog With {
            .Filter = "sciBASIC# dynamics script(*.vb)|*.vb"
        }
            If file.ShowDialog = DialogResult.OK Then
                Call textEditor.Text.SaveTo(file.FileName)
                Me.file = file.FileName
            End If
        End Using
    End Sub
End Class