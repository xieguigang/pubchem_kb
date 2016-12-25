Partial Class MainForm
	''' <summary>
	''' Required designer variable.
	''' </summary>
	Private components As System.ComponentModel.IContainer = Nothing

	''' <summary>
	''' Clean up any resources being used.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(disposing As Boolean)
		If disposing AndAlso (components IsNot Nothing) Then
			components.Dispose()
		End If
		MyBase.Dispose(disposing)
	End Sub

	#Region "Windows Form Designer generated code"

	''' <summary>
	''' Required method for Designer support - do not modify
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()
		Me.btnClearConsole = New System.Windows.Forms.Button()
		Me.groupBox1 = New System.Windows.Forms.GroupBox()
		Me.groupBox2 = New System.Windows.Forms.GroupBox()
		Me.rbNone = New System.Windows.Forms.RadioButton()
		Me.rbBlock = New System.Windows.Forms.RadioButton()
		Me.rbUnderline = New System.Windows.Forms.RadioButton()
		Me.btnMoveCursor = New System.Windows.Forms.Button()
		Me.txtCursorColumn = New System.Windows.Forms.TextBox()
		Me.txtCursorRow = New System.Windows.Forms.TextBox()
		Me.label2 = New System.Windows.Forms.Label()
		Me.label1 = New System.Windows.Forms.Label()
		Me.label3 = New System.Windows.Forms.Label()
		Me.txtMessage = New System.Windows.Forms.TextBox()
		Me.btnWriteMessage = New System.Windows.Forms.Button()
		Me.label4 = New System.Windows.Forms.Label()
		Me.pnlBackgroundColor = New System.Windows.Forms.Panel()
		Me.pnlForegroundColor = New System.Windows.Forms.Panel()
		Me.label5 = New System.Windows.Forms.Label()
		Me.colorDialog1 = New System.Windows.Forms.ColorDialog()
		Me.consoleControl2 = New ConsoleControl.ConsoleControl()
		Me.consoleControl1 = New ConsoleControl.ConsoleControl()
		Me.groupBox1.SuspendLayout()
		Me.groupBox2.SuspendLayout()
		Me.SuspendLayout()
		' 
		' btnClearConsole
		' 
		Me.btnClearConsole.Font = New System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
		Me.btnClearConsole.Location = New System.Drawing.Point(11, 388)
		Me.btnClearConsole.Name = "btnClearConsole"
		Me.btnClearConsole.Size = New System.Drawing.Size(625, 31)
		Me.btnClearConsole.TabIndex = 1
		Me.btnClearConsole.Text = "Clear Console Screen"
		Me.btnClearConsole.UseVisualStyleBackColor = True
		AddHandler Me.btnClearConsole.Click, New System.EventHandler(AddressOf Me.btnClearConsole_Click)
		' 
		' groupBox1
		' 
		Me.groupBox1.Controls.Add(Me.groupBox2)
		Me.groupBox1.Controls.Add(Me.btnMoveCursor)
		Me.groupBox1.Controls.Add(Me.txtCursorColumn)
		Me.groupBox1.Controls.Add(Me.txtCursorRow)
		Me.groupBox1.Controls.Add(Me.label2)
		Me.groupBox1.Controls.Add(Me.label1)
		Me.groupBox1.Location = New System.Drawing.Point(12, 425)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(226, 157)
		Me.groupBox1.TabIndex = 2
		Me.groupBox1.TabStop = False
		Me.groupBox1.Text = "Cursor"
		' 
		' groupBox2
		' 
		Me.groupBox2.Controls.Add(Me.rbNone)
		Me.groupBox2.Controls.Add(Me.rbBlock)
		Me.groupBox2.Controls.Add(Me.rbUnderline)
		Me.groupBox2.Location = New System.Drawing.Point(13, 95)
		Me.groupBox2.Name = "groupBox2"
		Me.groupBox2.Size = New System.Drawing.Size(197, 50)
		Me.groupBox2.TabIndex = 5
		Me.groupBox2.TabStop = False
		Me.groupBox2.Text = "Cursor Type"
		' 
		' rbNone
		' 
		Me.rbNone.AutoSize = True
		Me.rbNone.Location = New System.Drawing.Point(138, 21)
		Me.rbNone.Name = "rbNone"
		Me.rbNone.Size = New System.Drawing.Size(51, 17)
		Me.rbNone.TabIndex = 2
		Me.rbNone.Text = "None"
		Me.rbNone.UseVisualStyleBackColor = True
		AddHandler Me.rbNone.CheckedChanged, New System.EventHandler(AddressOf Me.rbNone_CheckedChanged)
		' 
		' rbBlock
		' 
		Me.rbBlock.AutoSize = True
		Me.rbBlock.Location = New System.Drawing.Point(84, 21)
		Me.rbBlock.Name = "rbBlock"
		Me.rbBlock.Size = New System.Drawing.Size(52, 17)
		Me.rbBlock.TabIndex = 1
		Me.rbBlock.Text = "Block"
		Me.rbBlock.UseVisualStyleBackColor = True
		AddHandler Me.rbBlock.CheckedChanged, New System.EventHandler(AddressOf Me.rbBlock_CheckedChanged)
		' 
		' rbUnderline
		' 
		Me.rbUnderline.AutoSize = True
		Me.rbUnderline.Checked = True
		Me.rbUnderline.Location = New System.Drawing.Point(12, 20)
		Me.rbUnderline.Name = "rbUnderline"
		Me.rbUnderline.Size = New System.Drawing.Size(70, 17)
		Me.rbUnderline.TabIndex = 0
		Me.rbUnderline.TabStop = True
		Me.rbUnderline.Text = "Underline"
		Me.rbUnderline.UseVisualStyleBackColor = True
		AddHandler Me.rbUnderline.CheckedChanged, New System.EventHandler(AddressOf Me.rbUnderline_CheckedChanged)
		' 
		' btnMoveCursor
		' 
		Me.btnMoveCursor.Location = New System.Drawing.Point(13, 66)
		Me.btnMoveCursor.Name = "btnMoveCursor"
		Me.btnMoveCursor.Size = New System.Drawing.Size(103, 23)
		Me.btnMoveCursor.TabIndex = 4
		Me.btnMoveCursor.Text = "Move Cursor"
		Me.btnMoveCursor.UseVisualStyleBackColor = True
		AddHandler Me.btnMoveCursor.Click, New System.EventHandler(AddressOf Me.btnMoveCursor_Click)
		' 
		' txtCursorColumn
		' 
		Me.txtCursorColumn.Location = New System.Drawing.Point(85, 43)
		Me.txtCursorColumn.Name = "txtCursorColumn"
		Me.txtCursorColumn.Size = New System.Drawing.Size(31, 20)
		Me.txtCursorColumn.TabIndex = 3
		' 
		' txtCursorRow
		' 
		Me.txtCursorRow.Location = New System.Drawing.Point(85, 21)
		Me.txtCursorRow.Name = "txtCursorRow"
		Me.txtCursorRow.Size = New System.Drawing.Size(31, 20)
		Me.txtCursorRow.TabIndex = 2
		' 
		' label2
		' 
		Me.label2.AutoSize = True
		Me.label2.Location = New System.Drawing.Point(10, 46)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(75, 13)
		Me.label2.TabIndex = 1
		Me.label2.Text = "Column (0-79):"
		' 
		' label1
		' 
		Me.label1.AutoSize = True
		Me.label1.Location = New System.Drawing.Point(22, 24)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(62, 13)
		Me.label1.TabIndex = 0
		Me.label1.Text = "Row (0-24):"
		' 
		' label3
		' 
		Me.label3.AutoSize = True
		Me.label3.Location = New System.Drawing.Point(293, 480)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(31, 13)
		Me.label3.TabIndex = 3
		Me.label3.Text = "Text:"
		' 
		' txtMessage
		' 
		Me.txtMessage.Location = New System.Drawing.Point(330, 478)
		Me.txtMessage.Name = "txtMessage"
		Me.txtMessage.Size = New System.Drawing.Size(306, 20)
		Me.txtMessage.TabIndex = 4
		' 
		' btnWriteMessage
		' 
		Me.btnWriteMessage.Location = New System.Drawing.Point(329, 501)
		Me.btnWriteMessage.Name = "btnWriteMessage"
		Me.btnWriteMessage.Size = New System.Drawing.Size(306, 43)
		Me.btnWriteMessage.TabIndex = 5
		Me.btnWriteMessage.Text = "Write Text to Console Screen at Current Cursor Position Using The Default Colors"
		Me.btnWriteMessage.UseVisualStyleBackColor = True
		AddHandler Me.btnWriteMessage.Click, New System.EventHandler(AddressOf Me.btnWriteMessage_Click)
		' 
		' label4
		' 
		Me.label4.AutoSize = True
		Me.label4.Location = New System.Drawing.Point(240, 431)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(95, 13)
		Me.label4.TabIndex = 6
		Me.label4.Text = "Background Color:"
		' 
		' pnlBackgroundColor
		' 
		Me.pnlBackgroundColor.Location = New System.Drawing.Point(335, 430)
		Me.pnlBackgroundColor.Name = "pnlBackgroundColor"
		Me.pnlBackgroundColor.Size = New System.Drawing.Size(40, 20)
		Me.pnlBackgroundColor.TabIndex = 7
		AddHandler Me.pnlBackgroundColor.DoubleClick, New System.EventHandler(AddressOf Me.pnlBackgroundColor_DoubleClick)
		' 
		' pnlForegroundColor
		' 
		Me.pnlForegroundColor.Location = New System.Drawing.Point(335, 453)
		Me.pnlForegroundColor.Name = "pnlForegroundColor"
		Me.pnlForegroundColor.Size = New System.Drawing.Size(40, 20)
		Me.pnlForegroundColor.TabIndex = 9
		AddHandler Me.pnlForegroundColor.DoubleClick, New System.EventHandler(AddressOf Me.pnlForegroundColor_DoubleClick)
		' 
		' label5
		' 
		Me.label5.AutoSize = True
		Me.label5.Location = New System.Drawing.Point(245, 454)
		Me.label5.Name = "label5"
		Me.label5.Size = New System.Drawing.Size(91, 13)
		Me.label5.TabIndex = 8
		Me.label5.Text = "Foreground Color:"
		' 
		' colorDialog1
		' 
		Me.colorDialog1.AnyColor = True
		Me.colorDialog1.FullOpen = True
		' 
		' consoleControl2
		' 
		Me.consoleControl2.AllowInput = True
		Me.consoleControl2.BackColor = System.Drawing.Color.Black
		Me.consoleControl2.ConsoleBackgroundColor = System.Drawing.Color.Black
		Me.consoleControl2.ConsoleForegroundColor = System.Drawing.Color.LightGray
		Me.consoleControl2.CurrentBackgroundColor = System.Drawing.Color.Black
		Me.consoleControl2.CurrentForegroundColor = System.Drawing.Color.LightGray
		Me.consoleControl2.CursorType = ConsoleControl.CursorTypes.Underline
		Me.consoleControl2.EchoInput = True
		Me.consoleControl2.ForeColor = System.Drawing.Color.LightGray
		Me.consoleControl2.Location = New System.Drawing.Point(678, 6)
		Me.consoleControl2.Name = "consoleControl2"
		Me.consoleControl2.ShowCursor = True
		Me.consoleControl2.Size = New System.Drawing.Size(646, 377)
		Me.consoleControl2.TabIndex = 10
		AddHandler Me.consoleControl2.Enter, New System.EventHandler(AddressOf Me.consoleControl2_Enter)
		AddHandler Me.consoleControl2.Leave, New System.EventHandler(AddressOf Me.consoleControl2_Leave)
		' 
		' consoleControl1
		' 
		Me.consoleControl1.AllowInput = True
		Me.consoleControl1.BackColor = System.Drawing.Color.Black
		Me.consoleControl1.ConsoleBackgroundColor = System.Drawing.Color.Black
		Me.consoleControl1.ConsoleForegroundColor = System.Drawing.Color.LightGray
		Me.consoleControl1.CurrentBackgroundColor = System.Drawing.Color.Black
		Me.consoleControl1.CurrentForegroundColor = System.Drawing.Color.LightGray
		Me.consoleControl1.CursorType = ConsoleControl.CursorTypes.Underline
		Me.consoleControl1.EchoInput = True
		Me.consoleControl1.ForeColor = System.Drawing.Color.LightGray
		Me.consoleControl1.Location = New System.Drawing.Point(6, 6)
		Me.consoleControl1.Name = "consoleControl1"
		Me.consoleControl1.ShowCursor = True
		Me.consoleControl1.Size = New System.Drawing.Size(646, 377)
		Me.consoleControl1.TabIndex = 0
		AddHandler Me.consoleControl1.Enter, New System.EventHandler(AddressOf Me.consoleControl1_Enter)
		AddHandler Me.consoleControl1.Leave, New System.EventHandler(AddressOf Me.consoleControl1_Leave)
		' 
		' Form1
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1334, 587)
		Me.Controls.Add(Me.consoleControl2)
		Me.Controls.Add(Me.pnlForegroundColor)
		Me.Controls.Add(Me.label5)
		Me.Controls.Add(Me.pnlBackgroundColor)
		Me.Controls.Add(Me.label4)
		Me.Controls.Add(Me.btnWriteMessage)
		Me.Controls.Add(Me.txtMessage)
		Me.Controls.Add(Me.label3)
		Me.Controls.Add(Me.groupBox1)
		Me.Controls.Add(Me.btnClearConsole)
		Me.Controls.Add(Me.consoleControl1)
		Me.DoubleBuffered = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.MaximizeBox = False
		Me.Name = "Form1"
		Me.Text = "Console Control Demo"
		AddHandler Me.Load, New System.EventHandler(AddressOf Me.Form1_Load)
		AddHandler Me.Paint, New System.Windows.Forms.PaintEventHandler(AddressOf Me.Form1_Paint)
		Me.groupBox1.ResumeLayout(False)
		Me.groupBox1.PerformLayout()
		Me.groupBox2.ResumeLayout(False)
		Me.groupBox2.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	#End Region

	Private consoleControl1 As ConsoleControl.ConsoleControl
	Private btnClearConsole As System.Windows.Forms.Button
	Private groupBox1 As System.Windows.Forms.GroupBox
	Private btnMoveCursor As System.Windows.Forms.Button
	Private txtCursorColumn As System.Windows.Forms.TextBox
	Private txtCursorRow As System.Windows.Forms.TextBox
	Private label2 As System.Windows.Forms.Label
	Private label1 As System.Windows.Forms.Label
	Private groupBox2 As System.Windows.Forms.GroupBox
	Private rbNone As System.Windows.Forms.RadioButton
	Private rbBlock As System.Windows.Forms.RadioButton
	Private rbUnderline As System.Windows.Forms.RadioButton
	Private label3 As System.Windows.Forms.Label
	Private txtMessage As System.Windows.Forms.TextBox
	Private btnWriteMessage As System.Windows.Forms.Button
	Private label4 As System.Windows.Forms.Label
	Private pnlBackgroundColor As System.Windows.Forms.Panel
	Private pnlForegroundColor As System.Windows.Forms.Panel
	Private label5 As System.Windows.Forms.Label
	Private colorDialog1 As System.Windows.Forms.ColorDialog
	Private consoleControl2 As ConsoleControl.ConsoleControl
End Class

