Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Linq

Public Partial Class MainForm
	Inherits Form
	Private _currentDirectory As String

	<DllImport("user32.dll", CharSet := CharSet.Auto, CallingConvention := CallingConvention.Winapi)> _
	Friend Shared Function GetFocus() As IntPtr
	End Function

	Public Sub New()
		InitializeComponent()

		Dim codeBase As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
		Dim uri__1 As New UriBuilder(codeBase)
		_currentDirectory = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uri__1.Path))
	End Sub

	Private Sub Form1_Load(sender As Object, e As EventArgs)
		consoleControl1.SetCursorPosition(0, 0)
		consoleControl1.Write("Hello World!")
		consoleControl1.SetCursorPosition(2, 15)

		consoleControl1.Write("Look at this message!", Color.Yellow, Color.Blue)

		txtCursorColumn.Text = consoleControl1.GetCursorPosition().Column.ToString()
		txtCursorRow.Text = consoleControl1.GetCursorPosition().Row.ToString()

		txtMessage.Text = "Hello World!"

		pnlBackgroundColor.BackColor = consoleControl1.CurrentBackgroundColor
		pnlForegroundColor.BackColor = consoleControl1.CurrentForegroundColor

		consoleControl1.AllowInput = False

		consoleControl2.SetCursorPosition(0, 0)
		consoleControl2.Write("Welcome to IcemanShell!")
		consoleControl2.SetCursorPosition(1, 0)
		consoleControl2.Write(String.Format("The current date and time is {0} {1}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()))
		consoleControl2.Write()
		consoleControl2.Write("Use the command 'help' if you need help!")
		consoleControl2.SetCursorPosition(consoleControl2.GetCursorPosition().Row + 2, 0)
		ShowPrompt()

		AddHandler consoleControl2.LineEntered, AddressOf ConsoleControl2LineEntered

		ActiveControl = consoleControl2
	End Sub

	Private Sub ShowPrompt()
		consoleControl2.Write()
		consoleControl2.Write("# ")
	End Sub

	Private Sub ConsoleControl2LineEntered(sender As Object, line As String)
		line = line.Trim()
		Dim ndx As Integer = line.IndexOf(" "C)
		Dim cmd As String = If(ndx < 0, line, line.Substring(0, ndx))
		Dim parameters As String = If(ndx < 0, "", line.Remove(0, ndx + 1))

		Select Case cmd.ToLower()
			Case ""
				ShowPrompt()
				Exit Select
			Case "help"
				ShowHelp()
				ShowPrompt()
				Exit Select
			Case "exit"
				Application.[Exit]()
				Exit Select
			Case "ls"
				DoLsCommand(parameters)
				Exit Select
			Case "echo"
				DoEchoCommand(parameters)
				ShowPrompt()
				Exit Select
			Case "date"
				DoDateCommand()
				ShowPrompt()
				Exit Select
			Case "pwd"
				DoPwdCommand()
				ShowPrompt()
				Exit Select
			Case "cd"
				DoCdCommand(parameters)
				ShowPrompt()
				Exit Select
			Case Else
				consoleControl2.Write("> Unknown Command!")
				consoleControl2.Write()
				ShowPrompt()
				Exit Select
		End Select
	End Sub

	Private Sub btnClearConsole_Click(sender As Object, e As EventArgs)
		consoleControl1.Clear()
		txtCursorColumn.Text = consoleControl1.GetCursorPosition().Column.ToString()
		txtCursorRow.Text = consoleControl1.GetCursorPosition().Row.ToString()
	End Sub

	Private Sub btnMoveCursor_Click(sender As Object, e As EventArgs)
		Dim row As Integer = Integer.Parse(txtCursorRow.Text)
		Dim column As Integer = Integer.Parse(txtCursorColumn.Text)

		If row < 0 OrElse row > 24 Then
			MessageBox.Show("Cursor Row Must be between 0 and 24!")
			Return
		End If

		If column < 0 OrElse column > 79 Then
			MessageBox.Show("Cursor Column Must be between 0 and 79!")
			Return
		End If

		consoleControl1.SetCursorPosition(row, column)
	End Sub

	Private Sub rbUnderline_CheckedChanged(sender As Object, e As EventArgs)
		ChangeCursorType()
	End Sub

	Private Sub rbBlock_CheckedChanged(sender As Object, e As EventArgs)
		ChangeCursorType()
	End Sub

	Private Sub rbNone_CheckedChanged(sender As Object, e As EventArgs)
		ChangeCursorType()
	End Sub

	Private Sub ChangeCursorType()
		If rbUnderline.Checked Then
			consoleControl1.CursorType = ConsoleControl.CursorTypes.Underline
		End If
		If rbBlock.Checked Then
			consoleControl1.CursorType = ConsoleControl.CursorTypes.Block
		End If
		If rbNone.Checked Then
			consoleControl1.CursorType = ConsoleControl.CursorTypes.Invisible
		End If
	End Sub

	Private Sub btnWriteMessage_Click(sender As Object, e As EventArgs)
		consoleControl1.Write(txtMessage.Text)
	End Sub

	Private Sub pnlBackgroundColor_DoubleClick(sender As Object, e As EventArgs)
		colorDialog1.Color = consoleControl1.CurrentBackgroundColor
		Dim dr As DialogResult = colorDialog1.ShowDialog()

		If dr = DialogResult.OK Then
			pnlBackgroundColor.BackColor = colorDialog1.Color
			consoleControl1.CurrentBackgroundColor = colorDialog1.Color
		End If
	End Sub

	Private Sub pnlForegroundColor_DoubleClick(sender As Object, e As EventArgs)
		colorDialog1.Color = consoleControl1.CurrentForegroundColor
		Dim dr As DialogResult = colorDialog1.ShowDialog()

		If dr = DialogResult.OK Then
			pnlForegroundColor.BackColor = colorDialog1.Color
			consoleControl1.CurrentForegroundColor = colorDialog1.Color
		End If
	End Sub

	Private Function FindFocusedControl() As Control
		Dim focusedControl As Control = Nothing
		Dim focusedHandle As IntPtr = GetFocus()
		If focusedHandle <> IntPtr.Zero Then
			focusedControl = FromHandle(focusedHandle)
		End If
		Return focusedControl
	End Function

	Private Sub Form1_Paint(sender As Object, e As PaintEventArgs)
		Const  offsetX As Integer = 2

		Dim b As New Bitmap(ClientSize.Width, ClientSize.Height)
		Dim g As Graphics = Graphics.FromImage(b)

		Dim c1 As Control = FindFocusedControl()

		If c1 IsNot Nothing AndAlso c1.Name.StartsWith("consoleControl") Then
			For i As Integer = 1 To 5
				Dim p As New Pen(Color.FromArgb(Math.Min(25 * (i * 2), 255), 50, 80, 255))
				g.DrawRectangle(p, c1.Location.X - i, c1.Location.Y - i, c1.Size.Width + i + i, c1.Size.Height + i + i)
			Next
		End If

		g.DrawLine(New Pen(Color.FromArgb(200, 200, 200)), 660 + offsetX, 3, 660 + offsetX, ClientSize.Height - 5)
		g.DrawLine(New Pen(Color.FromArgb(180, 180, 180)), 661 + offsetX, 3, 661 + offsetX, ClientSize.Height - 5)
		g.DrawLine(New Pen(Color.FromArgb(128, 128, 128)), 662 + offsetX, 3, 662 + offsetX, ClientSize.Height - 5)
		g.DrawLine(New Pen(Color.FromArgb(108, 108, 108)), 663 + offsetX, 3, 663 + offsetX, ClientSize.Height - 5)

		e.Graphics.DrawImage(b, New Point(0, 0))
		g.Dispose()
		b.Dispose()
	End Sub

	Private Sub consoleControl1_Enter(sender As Object, e As EventArgs)
		Refresh()
	End Sub

	Private Sub consoleControl1_Leave(sender As Object, e As EventArgs)
		Refresh()
	End Sub

	Private Sub consoleControl2_Enter(sender As Object, e As EventArgs)
		Refresh()
	End Sub

	Private Sub consoleControl2_Leave(sender As Object, e As EventArgs)
		Refresh()
	End Sub

	Private Sub ShowHelp()
		consoleControl2.Write("IcemanShell is just a 'toy' shell to show off Icemanind's Console Control.")
		consoleControl2.Write()
		consoleControl2.Write("Here is a list of commands:")
		consoleControl2.Write()
		consoleControl2.Write("(Note that the commands are NOT case sensitive)")
		consoleControl2.Write()
		consoleControl2.Write()
		consoleControl2.Write("----------------------------------------------------------------------")
		consoleControl2.Write()
		consoleControl2.Write("HELP                Shows this help message")
		consoleControl2.Write()
		consoleControl2.Write("EXIT                Quits the application")
		consoleControl2.Write()
		consoleControl2.Write("LS <path>           Shows the contents of the specified directory.")
		consoleControl2.Write()
		consoleControl2.Write("ECHO [string]       Echo's the [string] to the console.")
		consoleControl2.Write()
		consoleControl2.Write("DATE                Shows the current date.")
		consoleControl2.Write()
		consoleControl2.Write("PWD                 Shows the current working directory.")
		consoleControl2.Write()
		consoleControl2.Write("CD <path>           Change the current working directory to <path>.")
		consoleControl2.Write()
	End Sub

	Private Sub DoLsCommand(parameters As String)
		Dim path As String
		Dim bgColor As Color = consoleControl2.CurrentBackgroundColor
		Dim fgColor As Color = consoleControl2.CurrentForegroundColor

		If String.IsNullOrEmpty(parameters) Then
			path = _currentDirectory
		Else
			path = parameters.Trim(""""C)
		End If

		Dim files As String() = System.IO.Directory.GetFiles(path)
		Dim directories As String() = System.IO.Directory.GetDirectories(path)

		consoleControl2.CurrentForegroundColor = Color.CadetBlue
		For Each directory As String In directories.OrderBy(Function(z) z)
			Dim dirName As String = "[" & System.IO.Path.GetFileName(directory) & "]"
			consoleControl2.Write(String.Format("{0,-40}", If(dirName.Length > 40, Environment.NewLine & dirName & Environment.NewLine, dirName)))
		Next

		For Each file As String In files.OrderBy(Function(z) z)
			Dim fileName As String = If(System.IO.Path.GetFileName(file), "")
			Dim extension As String = If(System.IO.Path.GetExtension(file), "")
			Select Case extension.ToLower()
				Case ".exe"
					consoleControl2.CurrentForegroundColor = Color.FromArgb(100, 255, 100)
					Exit Select
				Case ".dll"
					consoleControl2.CurrentForegroundColor = Color.FromArgb(10, 165, 10)
					Exit Select
				Case Else
					consoleControl2.CurrentForegroundColor = Color.LightGray
					Exit Select
			End Select
			consoleControl2.Write(String.Format("{0,-40}", If(fileName.Length > 40, Environment.NewLine & fileName & Environment.NewLine, fileName)))
		Next

		consoleControl2.CurrentBackgroundColor = bgColor
		consoleControl2.CurrentForegroundColor = fgColor

		ShowPrompt()
	End Sub

	Private Sub DoEchoCommand(parameters As String)
		consoleControl2.Write(parameters)
		consoleControl2.Write()
	End Sub

	Private Sub DoDateCommand()
		consoleControl2.Write(String.Format("{0}", DateTime.Now.ToString("ddd MMM d HH:mm:ss K yyyy")))
		consoleControl2.Write()
	End Sub

	Private Sub DoPwdCommand()
		consoleControl2.Write(_currentDirectory)
		consoleControl2.Write()
	End Sub

	Private Sub DoCdCommand(parameters As String)
		parameters = parameters.Trim(""""C)

		If parameters.Length >= 2 AndAlso parameters(1) = ":"C Then
			If System.IO.Directory.Exists(parameters) Then
				_currentDirectory = parameters
			Else
				consoleControl2.Write("Invalid Directory ==> " & parameters)
				consoleControl2.Write()
				Return
			End If
		ElseIf parameters.Length > 0 AndAlso parameters(0) = "\"C Then
			If System.IO.Directory.Exists(System.IO.Path.GetPathRoot(_currentDirectory) & parameters.Remove(0, 1)) Then
				_currentDirectory = System.IO.Path.GetPathRoot(_currentDirectory) & parameters.Remove(0, 1)
			Else
				consoleControl2.Write("Invalid Directory ==> " & System.IO.Path.GetPathRoot(_currentDirectory) & parameters.Remove(0, 1))
				consoleControl2.Write()
				Return
			End If
		ElseIf parameters.Length > 0 Then
			If System.IO.Directory.Exists(_currentDirectory & "\" & parameters) Then
				_currentDirectory = _currentDirectory & "\" & parameters
			Else
				consoleControl2.Write("Invalid Directory ==> " & _currentDirectory & parameters)
				consoleControl2.Write()
				Return
			End If
		End If

		_currentDirectory = _currentDirectory.Replace("\\", "\")
		consoleControl2.Write(String.Format("Current Directory is now: {0}", _currentDirectory))
		consoleControl2.Write()
	End Sub
End Class
