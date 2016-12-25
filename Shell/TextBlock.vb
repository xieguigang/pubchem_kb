Imports System.Drawing

Friend Structure TextBlock
	Public Property BackgroundColor() As Color
		Get
			Return m_BackgroundColor
		End Get
		Set
			m_BackgroundColor = Value
		End Set
	End Property
	Private m_BackgroundColor As Color
	Public Property ForegroundColor() As Color
		Get
			Return m_ForegroundColor
		End Get
		Set
			m_ForegroundColor = Value
		End Set
	End Property
	Private m_ForegroundColor As Color
	Public Property Character() As Char
		Get
			Return m_Character
		End Get
		Set
			m_Character = Value
		End Set
	End Property
	Private m_Character As Char
End Structure
