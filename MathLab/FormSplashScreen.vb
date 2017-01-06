Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device
Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D
Imports System.ComponentModel

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

        '   Canvas.RefreshInterval = 20
        Canvas.Plot = New Func(Of Double, Double, (Z#, Color As Double))(AddressOf Model).GetPlotFunction("-3,3", "-3,3", xn:=200, yn:=200, showLegend:=False)
        Canvas.Camera.ViewDistance = -6
        Canvas.Camera.angleZ = 30
        Canvas.Camera.angleX = 30
        Canvas.Camera.angleY = -30
        Canvas.AutoRotation = True
        Canvas.Camera.offset = New Point(200, 200)
    End Sub

    Private Sub FormSplashScreen_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Canvas.AutoRotation = False
        Canvas.Pause()
    End Sub
End Class