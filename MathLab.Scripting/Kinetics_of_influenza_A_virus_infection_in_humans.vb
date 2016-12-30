Imports System.ComponentModel
Imports Microsoft.VisualBasic.Data.Bootstrapping
Imports Microsoft.VisualBasic.Mathematical.LinearAlgebra
Imports y = Microsoft.VisualBasic.Mathematical.Calculus.var

''' <summary>
''' ##### Kinetics of influenza A virus infection in humans
'''
''' > **DOI** 10.3390/v7102875
''' </summary>
''' <remarks>假设为实验观测数据</remarks>
Public Class Kinetics_of_influenza_A_virus_infection_in_humans_Model : Inherits MonteCarlo.Model

    Dim T As y
    Dim I As y
    Dim V As y

    <Description("")>
    Dim p As Double = 3 * 10 ^ -2
    <Description("")>
    Dim c As Double = 2
    <Description("")>
    Dim beta As Double = 8.8 * 10 ^ -6
    <Description("")>
    Dim delta As Double = 2.6

    Protected Overrides Sub func(dx As Double, ByRef dy As Vector)
        dy(T) = -beta * T * V
        dy(I) = beta * T * V - delta * I
        dy(V) = p * I - c * V
    End Sub

    Public Overrides Function eigenvector() As Dictionary(Of String, Eigenvector)
        Return Nothing
    End Function

    Public Overrides Function params() As MonteCarlo.ValueRange()
        Return {
            New MonteCarlo.ValueRange(NameOf(p), 0.000000000000001, 1000),
            New MonteCarlo.ValueRange(NameOf(c), 0, 1000),
            New MonteCarlo.ValueRange(NameOf(beta), 0.00000000000001, 1000),
            New MonteCarlo.ValueRange(NameOf(delta), 0, 1000)
        }
    End Function

    Public Overrides Function yinit() As MonteCarlo.ValueRange()
        Return {
            New MonteCarlo.ValueRange(NameOf(V), 0.0000000000001, 1000.0),
            New MonteCarlo.ValueRange(NameOf(T), 0, 1.0E+20),
            New MonteCarlo.ValueRange(NameOf(I), 0, 1000000)
        }
    End Function
End Class