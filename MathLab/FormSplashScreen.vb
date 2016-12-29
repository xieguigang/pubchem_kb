Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D

Public Class FormSplashScreen

    Dim WithEvents Canvas As Canvas

    Private Function Model(x#, y#) As (Z#, Color#)
        Return (3 * Math.Sin(x) * Math.Cos(y), Color:=x + y ^ 2)
    End Function

    Private Sub FormSplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Canvas = New Canvas With {
            .Dock = DockStyle.Fill
        }
        Call Controls.Add(Canvas)

        Canvas.Plot = New Func(Of Double, Double, (Z#, Color As Double))(AddressOf Model).GetPlotFunction("-3,3", "-3,3", showLegend:=False)
        Canvas.Camera.ViewDistance = -6
        Canvas.Camera.angleZ = 30
        Canvas.Camera.angleX = 30
        Canvas.Camera.angleY = -30
        Canvas.AutoRotation = True
    End Sub
End Class