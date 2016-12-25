<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormInteractive
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
        Me.ConsoleControl1 = New MathLab.Console.ConsoleControl()
        Me.SuspendLayout()
        '
        'ConsoleControl1
        '
        Me.ConsoleControl1.AllowInput = True
        Me.ConsoleControl1.AutoScroll = True
        Me.ConsoleControl1.BackColor = System.Drawing.Color.White
        Me.ConsoleControl1.ConsoleBackgroundColor = System.Drawing.Color.White
        Me.ConsoleControl1.ConsoleForegroundColor = System.Drawing.Color.Black
        Me.ConsoleControl1.CurrentBackgroundColor = System.Drawing.Color.White
        Me.ConsoleControl1.CurrentForegroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ConsoleControl1.CursorType = MathLab.Console.CursorTypes.Block
        Me.ConsoleControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ConsoleControl1.EchoInput = True
        Me.ConsoleControl1.ForeColor = System.Drawing.Color.Black
        Me.ConsoleControl1.Location = New System.Drawing.Point(0, 0)
        Me.ConsoleControl1.Name = "ConsoleControl1"
        Me.ConsoleControl1.ShowCursor = True
        Me.ConsoleControl1.Size = New System.Drawing.Size(646, 377)
        Me.ConsoleControl1.TabIndex = 0
        '
        'FormInteractive
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(963, 606)
        Me.Controls.Add(Me.ConsoleControl1)
        Me.Name = "FormInteractive"
        Me.Text = "Interactive Window"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ConsoleControl1 As Console.ConsoleControl
End Class
