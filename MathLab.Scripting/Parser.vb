Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.Scripting.TokenIcer
Imports Microsoft.VisualBasic.Serialization.JSON
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

        Dim vars As New List(Of NamedValue(Of String))
        Dim y0 As New List(Of NamedValue(Of String))
        Dim params As New List(Of NamedValue(Of String))
        Dim a#, b#, n%
        Dim c As New List(Of String)
        Dim comment As New Value(Of String)

        ' Static keywords$() = {"Dim", "Const", "Let", "Set"}

        For Each line As String In lines

            ' Is code comment line
            If Not (comment = line.GetCodeComment) Is Nothing Then

                Call c.Add(comment)

            Else ' Is the definition of the dynamics system
                Dim assign As (Prefix As String, var$, Value As String) = line.Assign
                Dim x As New NamedValue(Of String) With {
                    .Name = assign.var,
                    .Value = assign.Value,
                    .Description = c.JoinBy(ASCII.LF)
                }

                Call c.Clear()

                Select Case assign.Prefix.ToLower
                    Case "dim"
                        vars += x
                    Case "const"
                        params += x
                    Case "let"
                        y0 += x
                    Case "set"
                        Select Case x.Name.ToLower
                            Case "a"
                                a = x.Value.ParseNumeric
                            Case "b"
                                b = x.Value.ParseNumeric
                            Case "n"
                                n = x.Value.ParseNumeric
                            Case Else
                                Call $"Ignore of this invalid token {x.GetJson}".Warning
                        End Select
                    Case Else
                        Throw New SyntaxErrorException(line)
                End Select
            End If
        Next

        Return New Dynamics With {  ' Returns the generated dynamics system model
            .a = a,
            .b = b,
            .n = n,
            .params = params,
            .y0 = y0.ToArray(
                Function(x) New NamedValue(Of Double)(
                    x.Name,
                    x.Value.ParseNumeric,
                    x.Description)),
            .vars = LinqAPI.Exec(Of var) <= From x As NamedValue(Of String)
                                            In vars
                                            Select New var With {
                                                .Comments = x.Description,
                                                .Expression = x.Value,
                                                .Name = x.Name
                                            }
        }
    End Function

    <Extension>
    Private Function Assign(code$) As (prefix$, var$, value$)
        Dim x As NamedValue(Of String) = code.GetTagValue("=", trim:=True)
        Dim t$() = Regex.Split(x.Name.Trim, "\s+")
        Return (t(0), t(1), x.Value)
    End Function
End Module
