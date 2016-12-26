<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCorrealtions
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormCorrealtions))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabDataDesigner = New System.Windows.Forms.TabPage()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.tabCorrelationTable = New System.Windows.Forms.TabPage()
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        Me.tabScatterPlot = New System.Windows.Forms.TabPage()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.tabHeatmap = New System.Windows.Forms.TabPage()
        Me.pHeatMap = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.tabTNetwork = New System.Windows.Forms.TabPage()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.OpenFile = New System.Windows.Forms.ToolStripButton()
        Me.TabControl1.SuspendLayout()
        Me.tabDataDesigner.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabCorrelationTable.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabScatterPlot.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabHeatmap.SuspendLayout()
        CType(Me.pHeatMap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabDataDesigner)
        Me.TabControl1.Controls.Add(Me.tabCorrelationTable)
        Me.TabControl1.Controls.Add(Me.tabScatterPlot)
        Me.TabControl1.Controls.Add(Me.tabHeatmap)
        Me.TabControl1.Controls.Add(Me.tabTNetwork)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 25)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1076, 649)
        Me.TabControl1.TabIndex = 0
        '
        'tabDataDesigner
        '
        Me.tabDataDesigner.Controls.Add(Me.DataGridView1)
        Me.tabDataDesigner.Location = New System.Drawing.Point(4, 22)
        Me.tabDataDesigner.Name = "tabDataDesigner"
        Me.tabDataDesigner.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDataDesigner.Size = New System.Drawing.Size(1068, 623)
        Me.tabDataDesigner.TabIndex = 0
        Me.tabDataDesigner.Text = "Data Designer"
        Me.tabDataDesigner.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1062, 617)
        Me.DataGridView1.TabIndex = 0
        '
        'tabCorrelationTable
        '
        Me.tabCorrelationTable.Controls.Add(Me.DataGridView2)
        Me.tabCorrelationTable.Location = New System.Drawing.Point(4, 22)
        Me.tabCorrelationTable.Name = "tabCorrelationTable"
        Me.tabCorrelationTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tabCorrelationTable.Size = New System.Drawing.Size(1068, 623)
        Me.tabCorrelationTable.TabIndex = 1
        Me.tabCorrelationTable.Text = "Correlations"
        Me.tabCorrelationTable.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToOrderColumns = True
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView2.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(1062, 617)
        Me.DataGridView2.TabIndex = 0
        '
        'tabScatterPlot
        '
        Me.tabScatterPlot.Controls.Add(Me.Chart1)
        Me.tabScatterPlot.Controls.Add(Me.Label1)
        Me.tabScatterPlot.Controls.Add(Me.FlowLayoutPanel1)
        Me.tabScatterPlot.Location = New System.Drawing.Point(4, 22)
        Me.tabScatterPlot.Name = "tabScatterPlot"
        Me.tabScatterPlot.Size = New System.Drawing.Size(1068, 623)
        Me.tabScatterPlot.TabIndex = 4
        Me.tabScatterPlot.Text = "Scatter Plot"
        Me.tabScatterPlot.UseVisualStyleBackColor = True
        '
        'Chart1
        '
        Me.Chart1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(114, 3)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Size = New System.Drawing.Size(946, 612)
        Me.Chart1.TabIndex = 3
        Me.Chart1.Text = "Chart1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select Variables"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(13, 46)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(95, 550)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'tabHeatmap
        '
        Me.tabHeatmap.Controls.Add(Me.pHeatMap)
        Me.tabHeatmap.Controls.Add(Me.PictureBox1)
        Me.tabHeatmap.Location = New System.Drawing.Point(4, 22)
        Me.tabHeatmap.Name = "tabHeatmap"
        Me.tabHeatmap.Size = New System.Drawing.Size(1068, 623)
        Me.tabHeatmap.TabIndex = 2
        Me.tabHeatmap.Text = "Heatmap"
        Me.tabHeatmap.UseVisualStyleBackColor = True
        '
        'pHeatMap
        '
        Me.pHeatMap.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pHeatMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.pHeatMap.Location = New System.Drawing.Point(210, 3)
        Me.pHeatMap.Name = "pHeatMap"
        Me.pHeatMap.Size = New System.Drawing.Size(850, 612)
        Me.pHeatMap.TabIndex = 1
        Me.pHeatMap.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1068, 623)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'tabTNetwork
        '
        Me.tabTNetwork.Location = New System.Drawing.Point(4, 22)
        Me.tabTNetwork.Name = "tabTNetwork"
        Me.tabTNetwork.Size = New System.Drawing.Size(1068, 623)
        Me.tabTNetwork.TabIndex = 3
        Me.tabTNetwork.Text = "Topology Network"
        Me.tabTNetwork.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenFile})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1076, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'OpenFile
        '
        Me.OpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenFile.Image = CType(resources.GetObject("OpenFile.Image"), System.Drawing.Image)
        Me.OpenFile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenFile.Name = "OpenFile"
        Me.OpenFile.Size = New System.Drawing.Size(23, 22)
        Me.OpenFile.Text = "Open File"
        '
        'FormCorrealtions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1076, 674)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "FormCorrealtions"
        Me.Text = "Data Correlation Analysis"
        Me.TabControl1.ResumeLayout(False)
        Me.tabDataDesigner.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabCorrelationTable.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabScatterPlot.ResumeLayout(False)
        Me.tabScatterPlot.PerformLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabHeatmap.ResumeLayout(False)
        CType(Me.pHeatMap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tabDataDesigner As TabPage
    Friend WithEvents tabCorrelationTable As TabPage
    Friend WithEvents tabHeatmap As TabPage
    Friend WithEvents tabTNetwork As TabPage
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents DataGridView2 As DataGridView
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents OpenFile As ToolStripButton
    Friend WithEvents tabScatterPlot As TabPage
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents Chart1 As DataVisualization.Charting.Chart
    Friend WithEvents Label1 As Label
    Friend WithEvents pHeatMap As PictureBox
End Class
