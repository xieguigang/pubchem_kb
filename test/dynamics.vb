#Region "Dynamics Model"

Const beta  = 8.8 * 10 ^ -6
Const delta = 2.6
Const p     = 3 * 10 ^ -2
Const c     = 2

Dim T = -beta * T * V
Dim I = beta * T * V - delta * I
Dim V = p * I - c * V

#End Region

