Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Mathematical
Imports Microsoft.VisualBasic.Mathematical.Calculus
Imports Microsoft.VisualBasic.Mathematical.Types
Imports y = Microsoft.VisualBasic.Mathematical.Calculus.var

Public Module Run

    <Extension> Public Function RunTest(model As Dynamics) As ODEsOut
        Dim expression As New Expression
        Dim cal As New List(Of (x$, expr As SimpleExpression))

        For Each x As NamedValue(Of String) In model.params
            cal += (x.Name, expression.Compile(x.Value))
        Next

        For Each x In cal.OrderBy(Function(exp) exp.expr.ReferenceDepth)
            Call expression.SetVariable(x.x, x.expr.Evaluate)
        Next

        Call cal.Clear()

        For Each var As var In model.vars
            cal += (var.Name, expression.Compile(var.Expression))
        Next

        Dim vars = LinqAPI.Exec(Of y) <=
 _
            From x As NamedValue(Of Double)
            In model.y0
            Select New y With {
                .Name = x.Name,
                .value = x.Value
            }

        ' 获取整个动力学系统的定义
        Dim dynamics = cal.ToDictionary(
            Function(x) x.x,
            Function(x) x.expr)

        Return New GenericODEs(vars) With {
            .df = Sub(dx, ByRef dy)

                      For Each var As y In vars ' 首先将所有的变量值更新到计算引擎之中
                          Call expression.Variables.Set(var.Name, var.value)
                      Next

                      For Each var As y In vars  ' 然后分别计算常微分方程
                          dy(var) = dynamics(var.Name).Evaluate
                      Next

                      ' tick += 1

                      ' 在这里执行生物扰动实验
                      ' Call experimentTrigger.Tick()
                  End Sub
        }.Solve(model.n, model.a, model.b)
    End Function

    ''' <summary>
    ''' Compile the dynamics script into an assembly dll module.
    ''' </summary>
    ''' <param name="model"></param>
    ''' <param name="dll$">The model ``*.dll`` output file path.</param>
    ''' <returns></returns>
    <Extension> Public Function Compile(model As Dynamics, dll$) As Boolean

    End Function
End Module
