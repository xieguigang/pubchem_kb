' Kinetics of influenza A virus infection in humans
' DOI: 10.3390/v7102875

#Region "Dynamics Model"

Const beta  = 8.8 * 10 ^ -6
Const delta = 2.6
Const p     = 3 * 10 ^ -2
Const c     = 2

' Declare dynamics system
Dim T = -beta * T * V
Dim I = beta * T * V - delta * I
Dim V = p * I - c * V

' Set y0 values
LET V = 1.4 * 10 ^ -2
LET T = 4 * 10 ^ 8
LET I = 0

#End Region

' ODEs system configs
SET a = 0
SET b = 10
SET n = 10000