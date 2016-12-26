Imports Microsoft.VisualBasic.Data.ChartPlots.Plot3D.Device

Public Class FormSplashScreen

    Dim WithEvents Canvas As Canvas

    Private Sub FormSplashScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Canvas.Plot = Sub(g, rect)

                      End Sub
    End Sub
End Class