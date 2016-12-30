Imports System.ComponentModel
Imports MathLab.TextEditor
Imports Microsoft.VisualBasic.Logging

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormOutputLogs
    Inherits System.Windows.Forms.Form

    Private components As IContainer = Nothing

    Private fctb As FastColoredTextBox

    Private WithEvents btGotToEnd As Button

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormOutputLogs))
        Me.btGotToEnd = New System.Windows.Forms.Button()
        Me.fctb = New MathLab.TextEditor.FastColoredTextBox()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        CType(Me.fctb, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btGotToEnd
        '
        Me.btGotToEnd.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btGotToEnd.Location = New System.Drawing.Point(0, 472)
        Me.btGotToEnd.Name = "btGotToEnd"
        Me.btGotToEnd.Size = New System.Drawing.Size(1104, 23)
        Me.btGotToEnd.TabIndex = 6
        Me.btGotToEnd.Text = "Go to end"
        Me.btGotToEnd.UseVisualStyleBackColor = True
        '
        'fctb
        '
        Me.fctb.AutoCompleteBracketsList = New Char() {Global.Microsoft.VisualBasic.ChrW(40), Global.Microsoft.VisualBasic.ChrW(41), Global.Microsoft.VisualBasic.ChrW(123), Global.Microsoft.VisualBasic.ChrW(125), Global.Microsoft.VisualBasic.ChrW(91), Global.Microsoft.VisualBasic.ChrW(93), Global.Microsoft.VisualBasic.ChrW(34), Global.Microsoft.VisualBasic.ChrW(34), Global.Microsoft.VisualBasic.ChrW(39), Global.Microsoft.VisualBasic.ChrW(39)}
        Me.fctb.AutoScrollMinSize = New System.Drawing.Size(0, 15)
        Me.fctb.BackBrush = Nothing
        Me.fctb.CharHeight = 15
        Me.fctb.CharWidth = 7
        Me.fctb.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.fctb.DisabledColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer), CType(CType(180, Byte), Integer))
        Me.fctb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fctb.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.fctb.IsReplaceMode = False
        Me.fctb.Location = New System.Drawing.Point(0, 25)
        Me.fctb.Name = "fctb"
        Me.fctb.Paddings = New System.Windows.Forms.Padding(0)
        Me.fctb.ReadOnly = True
        Me.fctb.SelectionColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.fctb.ServiceColors = CType(resources.GetObject("fctb.ServiceColors"), MathLab.TextEditor.ServiceColors)
        Me.fctb.Size = New System.Drawing.Size(1104, 447)
        Me.fctb.TabIndex = 5
        Me.fctb.WordWrap = True
        Me.fctb.Zoom = 100
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1104, 25)
        Me.ToolStrip1.TabIndex = 7
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "Save Log"
        '
        'FormOutputLogs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1104, 495)
        Me.Controls.Add(Me.fctb)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.btGotToEnd)
        Me.Name = "FormOutputLogs"
        Me.Text = "Output Logger"
        CType(Me.fctb, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Private infoStyle As TextStyle = New TextStyle(Brushes.Black, Nothing, FontStyle.Regular)
    Private warningStyle As TextStyle = New TextStyle(Brushes.BurlyWood, Nothing, FontStyle.Regular)
    Private errorStyle As TextStyle = New TextStyle(Brushes.Red, Nothing, FontStyle.Regular)

    Public Sub Log(text As String, type As Logging.MSG_TYPES)
        Dim style As Style

        Select Case type
            Case MSG_TYPES.DEBUG, MSG_TYPES.INF
                style = infoStyle
            Case MSG_TYPES.ERR
                style = errorStyle
            Case MSG_TYPES.WRN
                style = warningStyle
            Case Else
                style = infoStyle
        End Select

        Me.fctb.BeginUpdate()
        Me.fctb.Selection.BeginUpdate()
        Dim userSelection As Range = Me.fctb.Selection.Clone()
        Me.fctb.Selection.Start = If(Me.fctb.LinesCount > 0, New Place(Me.fctb(Me.fctb.LinesCount - 1).Count, Me.fctb.LinesCount - 1), New Place(0, 0))
        Me.fctb.InsertText(text, style)
        If Not userSelection.IsEmpty OrElse userSelection.Start.iLine < Me.fctb.LinesCount - 2 Then
            Me.fctb.Selection.Start = userSelection.Start
            Me.fctb.Selection.[End] = userSelection.[End]
        Else
            Me.fctb.DoCaretVisible()
        End If
        Me.fctb.Selection.EndUpdate()
        Me.fctb.EndUpdate()
    End Sub

    Private Sub btGotToEnd_Click(sender As Object, e As EventArgs) Handles btGotToEnd.Click
        Me.fctb.GoEnd()
    End Sub

    Private Sub FormOutputLogs_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not this.Closing Then
            WindowState = FormWindowState.Minimized
            e.Cancel = True
        End If
    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButton1 As ToolStripButton

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Using file As New OpenFileDialog With {
            .Filter = "Logging text file(*.log)|*.log"
        }
            If file.ShowDialog = DialogResult.OK Then
                Call fctb.Text.SaveTo(file.FileName)
            End If
        End Using
    End Sub
End Class