Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Serialization.JSON

Public Class var
    Implements INamedValue

    Public Property Name As String Implements INamedValue.Key
    Public Property Expression As String
    Public Property Comments As String

    Public Overrides Function ToString() As String
        Return Me.GetJson
    End Function
End Class
