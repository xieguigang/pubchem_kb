Public Class Location
	Public Property Row() As Integer
		Get
			Return m_Row
		End Get
		Set
			m_Row = Value
		End Set
	End Property
	Private m_Row As Integer
	Public Property Column() As Integer
		Get
			Return m_Column
		End Get
		Set
			m_Column = Value
		End Set
	End Property
	Private m_Column As Integer
End Class
