Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Mathematical.Calculus
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class Dynamics

    Public Property vars As var()
    Public Property params As NamedValue(Of Double)()
    Public Property y0 As NamedValue(Of Double)()

    Public Property a As Double
    Public Property b As Double
    Public Property n As Integer

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class
