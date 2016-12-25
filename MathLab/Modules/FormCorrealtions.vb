Imports Microsoft.VisualBasic.Data.csv.DocumentExtensions
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.VisualBasic.Mathematical.Correlations
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Windows.Forms
Imports Microsoft.VisualBasic.Data.csv.DocumentStream
Imports Microsoft.VisualBasic.Data.visualize.Network
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream

Public Class FormCorrealtions

    Dim X#()

    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenFile.Click
        Using file As New OpenFileDialog With {
            .Filter = "Excel Data(*.csv)|*.csv"
        }
            If file.ShowDialog = DialogResult.OK Then
                Dim dataset = DataImports.GetData(url:=file.FileName, x:=X)

                If dataset Is Nothing Then
                    Return
                Else
                    Call DataGridView1.Clear()
                    Call FlowLayoutPanel1.Controls.Clear()
                End If

                For Each col In dataset
                    Call DataGridView1.Columns.Add(col.Name, col.Name)
                    DataGridView1.Columns(col.Name).ValueType =
                        GetType(Double)

                    Dim check As New CheckBox With {
                        .Checked = True,
                        .Text = col.Name
                    }

                    AddHandler check.CheckStateChanged, AddressOf tabScatterPlot_GotFocus

                    Call FlowLayoutPanel1.Controls.Add(check)
                Next

                For i As Integer = 0 To dataset(Scan0).Value.Length - 1
                    Dim index = i
                    Dim row As Object() = dataset _
                        .ToArray(Function(x) CObj(x.Value(index)))
                    Call DataGridView1.Rows.Add(row)
                Next
            End If
        End Using
    End Sub

    Private Sub TabControl1_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControl1.Selecting
        If e.TabPage.Equals(tabScatterPlot) Then
            Call tabScatterPlot_GotFocus()
        ElseIf e.TabPage.Equals(tabHeatmap) Then
            Call PlotHeatmap()
        ElseIf e.TabPage.Equals(tabCorrelationTable) Then
            Call DisplayMatrix()
        ElseIf e.TabPage.Equals(tabTNetwork) Then
            Call DisplayNetwork()
        End If
    End Sub

    Private Sub DisplayNetwork()
        canvas.Graph = PopulateData(False) _
            .Data _
            .Values _
            .CorrelationMatrix _
            .Select(Function(entity) New DataSet With {
                .Identifier = entity.Name,
                .Properties = entity.Value
            }).FromCorrelations(trim:=True) _
              .CreateGraph
    End Sub

    Private Sub DisplayMatrix()
        Dim popData = PopulateData(False).Data.Values.ToArray
        Dim correlates = popData.CorrelationMatrix
        Dim keys = correlates.First.Value.Keys.ToArray

        Call DataGridView2.Clear()

        For Each k In keys
            Call DataGridView2.Columns.Add(k, k)
        Next

        For Each row In correlates
            Dim rowData As Object() = keys _
                .ToArray(Function(x) CObj(row.Value(x)))
            Call DataGridView2.AddRowData(row.Name, rowData)
        Next
    End Sub

    Private Sub PlotHeatmap()
        Dim popData = PopulateData(False).Data.Values.ToArray
        Dim correlates = popData.CorrelationMatrix

        pHeatMap.BackgroundImage = Heatmap.Plot(correlates)
    End Sub

    Private Function PopulateData(trim As Boolean) As (Data As Dictionary(Of NamedValue(Of Double())), names As List(Of NamedValue(Of Integer)))
        Dim names As New List(Of NamedValue(Of Integer))
        Dim data As New Dictionary(Of NamedValue(Of Double()))

        For Each var As CheckBox In FlowLayoutPanel1.Controls
            If trim AndAlso Not var.Checked Then Continue For

            names += New NamedValue(Of Integer) With {
                    .Name = var.Text,
                    .Value = DataGridView1.Columns(var.Text).Index
                }
            data(var.Text) = New NamedValue(Of Double()) With {
                .Name = var.Text,
                .Value = New Double(DataGridView1.RowCount - 1) {}
            }

        Next

        Dim i%

        For Each row As DataGridViewRow In DataGridView1.Rows
            For Each v In names
                data(v.Name).Value(i) = CDbl(row.Cells(v.Value).Value)
            Next

            i += 1
        Next

        Return (data, names)
    End Function

    Private Sub tabScatterPlot_GotFocus() '(sender As Object, e As EventArgs)
        Dim popData = PopulateData(True)
        Dim data = popData.Data

        Call Chart1.Series.Clear()

        For Each serial In data.Values
            Dim s = Chart1.Series.Add(serial.Name)
            s.ChartType = SeriesChartType.Line

            For Each X As SeqValue(Of Double) In Me.X.SeqIterator
                Call s.Points.AddXY(+X, serial.Value(X))
            Next
        Next
    End Sub

    Dim WithEvents canvas As Microsoft.VisualBasic.Data.visualize.Network.Canvas.Canvas

    Private Sub FormCorrealtions_Load(sender As Object, e As EventArgs) Handles Me.Load
        canvas = New Microsoft.VisualBasic.Data.visualize.Network.Canvas.Canvas With {
            .Dock = DockStyle.Fill
        }
        tabTNetwork.Controls.Add(canvas)
    End Sub
End Class