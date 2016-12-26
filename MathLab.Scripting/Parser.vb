Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Mathematical.Calculus
Imports Microsoft.VisualBasic.Scripting.TokenIcer
Imports Microsoft.VisualBasic.Text

''' <summary>
''' 动力学系统脚本之中并没有多行的结构
''' </summary>
Public Module Parser

    ''' <summary>
    ''' Mathematics dynamics system script parser
    ''' </summary>
    ''' <param name="path$"></param>
    ''' <returns></returns>
    <Extension> Public Function LoadScript(path$) As Dynamics
        Dim lines$() = path _
            .IterateAllLines _
            .Where(Function(s) Not s.IsBlank) _
            .Select(AddressOf Trim) _
            .ToArray

        Dim vars As New List(Of var)
        Dim y0 As New List(Of NamedValue(Of Double))
        Dim params As New List(Of NamedValue(Of Double))
        Dim a#, b#, n%
        Dim c As New List(Of String)
        Dim comment As New Value(Of String)

        ' Static keywords$() = {"Dim", "Const", "Let", "Set"}

        For Each line As String In lines

            ' Is code comment line
            If Not (comment = line.GetCodeComment) Is Nothing Then
                c += comment.value

            Else ' Is the definition of the dynamics system
                Dim assign As (Prefix As String, var$, Value As String) = line.Assign

                comment = c.JoinBy(ASCII.LF)
                c *= 0

                Select Case assign.Prefix.ToLower
                    Case "dim"
                        vars += New var With {.Name = assign.var, .}
                End Select
            End If
        Next
    End Function

    <Extension>
    Private Function Assign(code$) As (prefix$, var$, value$)
        Dim x As NamedValue(Of String) = code.GetTagValue("=", trim:=True)
        Dim t$() = Regex.Split(x.Name.Trim, "\s+")
        Return (t(0), t(1), x.Value)
    End Function
End Module
