Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq

Public Class ConsoleControl
	Inherits UserControl
	Private Const RenderFontName As String = "Courier New"
	Private Const ScreenSize As Integer = 80 * 25

	Private ReadOnly _keysBuffer As List(Of Char)
	Private ReadOnly _commandBuffer As List(Of String)

	Private _commandBufferIndex As Integer

	Private _isCursorOn As Boolean

	Private ReadOnly _renderFont As New Font(RenderFontName, 10, FontStyle.Regular)

	Public Delegate Sub LineEnteredDelegate(sender As Object, line As String)

	''' <summary>
	''' This event is raised whenever text is entered into the console control
	''' </summary>
	Public Event LineEntered As LineEnteredDelegate

	Private _consoleBackgroundColor As Color
	Private _consoleForegroundColor As Color
	Private _cursorX As Integer
	Private _cursorY As Integer
	Private _cursorType As CursorTypes

	Private ReadOnly _textBlockArray As TextBlock()

	''' <summary>
	''' Gets or Sets a value indicating whether the console should show the cursor
	''' </summary>
	Public Property ShowCursor() As Boolean
		Get
			Return m_ShowCursor
		End Get
		Set
			m_ShowCursor = Value
		End Set
	End Property
	Private m_ShowCursor As Boolean

	''' <summary>
	''' Gets or Sets a value indicating whether the console should allow keyboard input
	''' </summary>
	Public Property AllowInput() As Boolean
		Get
			Return m_AllowInput
		End Get
		Set
			m_AllowInput = Value
		End Set
	End Property
	Private m_AllowInput As Boolean

	''' <summary>
	''' Gets or Sets a value indicating whether the console should echo any input it receives.
	''' </summary>
	Public Property EchoInput() As Boolean
		Get
			Return m_EchoInput
		End Get
		Set
			m_EchoInput = Value
		End Set
	End Property
	Private m_EchoInput As Boolean

	Private _readLineTimer As Timer

	Public Property CurrentForegroundColor() As Color
		Get
			Return m_CurrentForegroundColor
		End Get
		Set
			m_CurrentForegroundColor = Value
		End Set
	End Property
	Private m_CurrentForegroundColor As Color
	Public Property CurrentBackgroundColor() As Color
		Get
			Return m_CurrentBackgroundColor
		End Get
		Set
			m_CurrentBackgroundColor = Value
		End Set
	End Property
	Private m_CurrentBackgroundColor As Color

	''' <summary>
	''' Gets or Sets the current cursor type.
	''' </summary>
	Public Property CursorType() As CursorTypes
		Get
			Return _cursorType
		End Get
		Set
			_cursorType = value
			Invalidate()
		End Set
	End Property

	''' <summary>
	''' Gets or Sets the background color of the console.
	''' Default is Black.
	''' </summary>
	Public Property ConsoleBackgroundColor() As Color
		Get
			Return _consoleBackgroundColor
		End Get
		Set
			_consoleBackgroundColor = value
			BackColor = value
			Invalidate()
		End Set
	End Property

	''' <summary>
	''' Gets or Sets the foreground color of the console.
	''' Default is light gray.
	''' </summary>
	Public Property ConsoleForegroundColor() As Color
		Get
			Return _consoleForegroundColor
		End Get
		Set
			_consoleForegroundColor = value
			ForeColor = value
			Invalidate()
		End Set
	End Property

	Public Sub New()
		Width = 646
		Height = 377

		_cursorX = 0
		_cursorY = 0
		_isCursorOn = False
		CursorType = CursorTypes.Underline

		ConsoleBackgroundColor = Color.Black
		ConsoleForegroundColor = Color.LightGray
		CurrentForegroundColor = Color.LightGray
		CurrentBackgroundColor = Color.Black

		ShowCursor = True
		AllowInput = True
		EchoInput = True

		_textBlockArray = New TextBlock(ScreenSize - 1) {}
		' 80 x 25
		For i As Integer = 0 To ScreenSize - 1
			_textBlockArray(i).BackgroundColor = ConsoleBackgroundColor
			_textBlockArray(i).ForegroundColor = ConsoleForegroundColor
			_textBlockArray(i).Character = ControlChars.NullChar
		Next

        Dim cursorFlashTimer As New Timer() With {
             .Enabled = True,
             .Interval = 500
        }
        AddHandler cursorFlashTimer.Tick, AddressOf CursorFlashTimerTick

		_keysBuffer = New List(Of Char)()
		_commandBuffer = New List(Of String)()
		_commandBufferIndex = 0

		AddHandler KeyPress, AddressOf ConsoleControlKeyPress
	End Sub

	Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
		If keyData = Keys.Up Then
			If _commandBufferIndex <= 0 OrElse (Not AllowInput) Then
				Return True
			End If

			Dim len As Integer = _keysBuffer.Count
			For i As Integer = 0 To len - 1
				ConsoleControlKeyPress(Me, New KeyPressEventArgs(ChrW(8)))
			Next
			_keysBuffer.Clear()
			For Each c As Char In _commandBuffer(_commandBufferIndex - 1)
				_keysBuffer.Add(c)
				If EchoInput Then
					Write(c)
				End If
			Next
			_commandBufferIndex -= 1
			Return True
		End If
		If keyData = Keys.Down Then
			If (_commandBufferIndex + 1) >= _commandBuffer.Count OrElse (Not AllowInput) Then
				Return True
			End If

			Dim len As Integer = _keysBuffer.Count
			For i As Integer = 0 To len - 1
				ConsoleControlKeyPress(Me, New KeyPressEventArgs(ChrW(8)))
			Next
			_keysBuffer.Clear()

			For Each c As Char In _commandBuffer(_commandBufferIndex + 1)
				_keysBuffer.Add(c)
				If EchoInput Then
					Write(c)
				End If
			Next

			_commandBufferIndex += 1
			Return True
		End If
		Return MyBase.ProcessCmdKey(msg, keyData)
	End Function

	Private Sub ConsoleControlKeyPress(sender As Object, e As KeyPressEventArgs)
		If Not AllowInput Then
			Return
		End If

        If Asc(e.KeyChar) = 8 Then
            If _keysBuffer.Count = 0 Then
                Return
            End If
            If EchoInput Then
                _textBlockArray(GetIndex()).Character = ControlChars.NullChar
                _cursorX -= 1
                If _cursorX < 0 Then
                    _cursorY -= 1
                    _cursorX = 79
                    If _cursorY < 0 Then
                        _cursorY += 1
                        _cursorX = 0
                    End If
                End If
                _textBlockArray(GetIndex()).Character = ControlChars.NullChar
                Invalidate()
            End If
            _keysBuffer.RemoveAt(_keysBuffer.Count - 1)
            Return
        End If
        _keysBuffer.Add(e.KeyChar)
		If EchoInput Then
			Write(e.KeyChar)
			If e.KeyChar = ControlChars.Cr Then
				Write(ControlChars.Lf)
			End If
		End If

		If e.KeyChar = ControlChars.Cr Then
			If Environment.NewLine.Length = 2 Then
				_keysBuffer.Add(ControlChars.Lf)
			End If
			Dim s As String = _keysBuffer.Aggregate("", Function(current, c) current & c)
			_keysBuffer.Clear()

			_commandBuffer.Add(s.Trim(ControlChars.Cr, ControlChars.Lf))
			_commandBufferIndex = _commandBuffer.Count
			RaiseEvent LineEntered(Me, s)
		End If
		Invalidate()
	End Sub

	Private Sub CursorFlashTimerTick(sender As Object, e As EventArgs)
		If Not ShowCursor Then
			Return
		End If
		_isCursorOn = Not _isCursorOn
		Dim c As Char
		Select Case CursorType
			Case CursorTypes.Block
				c = "█"C
				Exit Select
			Case CursorTypes.Invisible
				c = " "C
				Exit Select
			Case Else
				c = "_"C
				Exit Select
		End Select
		_textBlockArray(GetIndex()).Character = If(_isCursorOn, c, ControlChars.NullChar)
		Invalidate()
	End Sub

	Protected Overrides Sub OnPaint(e As PaintEventArgs)
		Dim x As Integer = 0
		Dim y As Integer = 0
		Dim charWidth As Integer = 8
		Dim charHeight As Integer = 15

        Using bitmap As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(bitmap)
                For i As Integer = 0 To ScreenSize - 1
                    Dim fc As Color = If(_textBlockArray(i).Character = ControlChars.NullChar, _consoleForegroundColor, _textBlockArray(i).ForegroundColor)
                    Dim bc As Color = If(_textBlockArray(i).Character = ControlChars.NullChar, _consoleBackgroundColor, _textBlockArray(i).BackgroundColor)

                    Dim bgBrush As Brush = New SolidBrush(bc)
                    Dim fgBrush As Brush = New SolidBrush(fc)

                    g.FillRectangle(bgBrush, New Rectangle(x + 2, y + 1, charWidth, charHeight))
                    g.DrawString(If(_textBlockArray(i).Character = ControlChars.NullChar, " ", _textBlockArray(i).Character.ToString()), _renderFont, fgBrush, New PointF(x, y))

                    x += charWidth
                    If x > 79 * charWidth Then
                        y += charHeight
                        x = 0
                    End If
                Next

                e.Graphics.DrawImage(bitmap, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel)
            End Using
        End Using

        MyBase.OnPaint(e)
	End Sub

	Protected Overrides Sub OnResize(e As EventArgs)
		MyBase.OnResize(e)
		Width = 646
		Height = 377
	End Sub

	Protected Overrides ReadOnly Property CreateParams() As CreateParams
		Get
			Dim cp As CreateParams = MyBase.CreateParams
			cp.ExStyle = cp.ExStyle Or &H2000000
			Return cp
		End Get
	End Property

	''' <summary>
	''' Sets the position of the cursor
	''' </summary>
	''' <param name="row">The row of the cursor position</param>
	''' <param name="column">The column of the cursor position</param>
	Public Sub SetCursorPosition(row As Integer, column As Integer)
		If ShowCursor Then
			_textBlockArray(GetIndex()).Character = ControlChars.NullChar
		End If
		_cursorX = column
		_cursorY = row

		Invalidate()
	End Sub

	''' <summary>
	''' Sets the position of the cursor
	''' </summary>
	''' <param name="location">The location of the cursor position</param>
	Public Sub SetCursorPosition(location As Location)
		SetCursorPosition(location.Row, location.Column)
	End Sub

	''' <summary>
	''' Gets the position of the cursor
	''' </summary>
	''' <returns>The location of the cursor</returns>
	Public Function GetCursorPosition() As Location
        Return New Location() With {
             .Column = _cursorX,
             .Row = _cursorY
        }
    End Function

	''' <summary>
	''' Writes a newline (carriage return) to the console.
	''' </summary>
	Public Sub Write()
		Write(Environment.NewLine)
	End Sub

	''' <summary>
	''' Writes a character to the console using the
	''' current foreground color and background color.
	''' </summary>
	''' <param name="c">The character to write to the console.</param>
	Public Sub Write(c As Char)
		Write(c, CurrentForegroundColor, CurrentBackgroundColor)
	End Sub

    ''' <summary>
    ''' Writes a character to the console using the
    ''' specified foreground color and background color
    ''' </summary>
    ''' <param name="c">The character to write to the console.</param>
    ''' <param name="fgColor">The foreground color</param>
    ''' <param name="bgColor">The background color</param>
    Public Sub Write(c As Char, fgColor As Color, bgColor As Color)
        Dim ascii% = Asc(c)

        If ascii = 7 Then
            Console.Beep(1000, 500)
            Return
        End If

        If ascii = 13 Then
            SetCursorPosition(GetCursorPosition().Row, 0)
            Return
        End If
        If ascii = 10 Then
            If Environment.NewLine.Length = 1 Then
                SetCursorPosition(GetCursorPosition().Row, 0)
            End If
            _cursorY += 1
            If _cursorY > 24 Then
                ScrollUp()
                _cursorY = 24
            End If
            Return
        End If
        _textBlockArray(GetIndex()).Character = c
        _textBlockArray(GetIndex()).BackgroundColor = bgColor
        _textBlockArray(GetIndex()).ForegroundColor = fgColor
        MoveCursorPosition()

        Invalidate()
    End Sub

    ''' <summary>
    ''' Writes a string to the console using the current
    ''' foreground color and background color
    ''' </summary>
    ''' <param name="text">The string to write to the console</param>
    Public Sub Write(text As String)
		Write(text, CurrentForegroundColor, CurrentBackgroundColor)
	End Sub

	''' <summary>
	''' Writes a string to the console using the specified
	''' foreground color and background color
	''' </summary>
	''' <param name="text">The string to write to the console</param>
	''' <param name="fgColor">The foreground color</param>
	''' <param name="bgColor">The background color</param>
	Public Sub Write(text As String, fgColor As Color, bgColor As Color)
		For Each c As Char In text
			Write(c, fgColor, bgColor)
		Next
		Invalidate()
	End Sub

	Private Sub MoveCursorPosition()
		_cursorX += 1
		If _cursorX > 79 Then
			_cursorX = 0
			_cursorY += 1
		End If
		If _cursorY > 24 Then
			ScrollUp()
			_cursorY = 24
		End If
	End Sub

	Private Function GetIndex(row As Integer, col As Integer) As Integer
		Return 80 * row + col
	End Function

	Private Function GetIndex() As Integer
		Return GetIndex(_cursorY, _cursorX)
	End Function

	''' <summary>
	''' Scrolls the console screen window up the given.
	''' number of lines.
	''' </summary>
	''' <param name="lines">The number of lines to scroll up</param>
	Public Sub ScrollUp(lines As Integer)
		While lines > 0
			For i As Integer = 0 To ScreenSize - 81
				_textBlockArray(i) = _textBlockArray(i + 80)
			Next
			For i As Integer = ScreenSize - 80 To ScreenSize - 1
				_textBlockArray(i).Character = ControlChars.NullChar
				_textBlockArray(i).BackgroundColor = ConsoleBackgroundColor
				_textBlockArray(i).ForegroundColor = ConsoleForegroundColor
			Next
			lines -= 1
		End While
		Invalidate()
	End Sub

	''' <summary>
	''' Scrolls the console screen window up one line.
	''' </summary>
	Public Sub ScrollUp()
		ScrollUp(1)
	End Sub

	''' <summary>
	''' Clears the console screen
	''' </summary>
	Public Sub Clear()
		For i As Integer = 0 To ScreenSize - 1
			_textBlockArray(i).BackgroundColor = ConsoleBackgroundColor
			_textBlockArray(i).ForegroundColor = ConsoleForegroundColor
			_textBlockArray(i).Character = ControlChars.NullChar
		Next

		_cursorX = 0
		_cursorY = 0

		Invalidate()
	End Sub

	''' <summary>
	''' Sets the background color at the specified location
	''' </summary>
	''' <param name="color">The background color to set</param>
	''' <param name="row">The row at which to set the background color</param>
	''' <param name="column">The column at which to set the background color</param>
	Public Sub SetBackgroundColorAt(color As Color, row As Integer, column As Integer)
		_textBlockArray(GetIndex(row, column)).BackgroundColor = color
		Invalidate()
	End Sub

	''' <summary>
	''' Sets the foreground color at the specified location
	''' </summary>
	''' <param name="color">The foreground color to set</param>
	''' <param name="row">The row at which to set the foreground color</param>
	''' <param name="column">The column at which to set the foreground color</param>
	Public Sub SetForegroundColorAt(color As Color, row As Integer, column As Integer)
		_textBlockArray(GetIndex(row, column)).ForegroundColor = color
		Invalidate()
	End Sub

	''' <summary>
	''' Sets the character at the specified location
	''' </summary>
	''' <param name="character">The character to set</param>
	''' <param name="row">The row at which to place the character</param>
	''' <param name="column">The column at which to place the character</param>
	Public Sub SetCharacterAt(character As Char, row As Integer, column As Integer)
		_textBlockArray(GetIndex(row, column)).Character = character
		Invalidate()
	End Sub
End Class
