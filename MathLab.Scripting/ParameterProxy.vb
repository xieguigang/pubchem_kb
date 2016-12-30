Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Emit.CodeDOM_VBC

Public MustInherit Class ParameterProxy

    Public Function GetParameters() As Dictionary(Of String, Double)
        Dim proxy As Type = MyClass.GetType
        Dim properties As PropertyInfo() = proxy _
            .GetProperties(PublicProperty) _
            .Where(Function(p) p.PropertyType.Equals(GetType(Double))) _
            .ToArray
        Dim out As Dictionary(Of String, Double) = properties _
            .ToDictionary(Function(name) name.Name,
                          Function(value) DirectCast(value.GetValue(Me), Double))

        Return out
    End Function

    Public Shared Function Creates(vars$()) As ParameterProxy
        Dim [class] As New StringBuilder()

        For Each var$ In vars
            Call [class].AppendLine($"Public Property {var} As Double")
        Next

        Dim code As String = $"
Public Class {NameOf(ParameterProxy)} : Inherits {GetType(ParameterProxy).FullName}

#Region ""Parameters""
{[class].ToString}
#End Region

End Class"

        Dim args As CompilerParameters = VBC.CreateParameters(references:=App.References)
        Dim assm As Assembly = VBC.CompileCode(code, args)
        Dim type As Type = assm.GetType(NameOf(ParameterProxy))
        Dim obj As Object = Activator.CreateInstance(type)
        Return DirectCast(obj, ParameterProxy)
    End Function
End Class
