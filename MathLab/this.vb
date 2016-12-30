Imports Microsoft.VisualBasic.Logging

Module this

    Public Property Closing As Boolean = False
    Public Property Logger As FormOutputLogs

    Public Sub Logging(text$, Optional level As MSG_TYPES = MSG_TYPES.INF)
        SyncLock Logger
            Call Logger.Log(text, level)
        End SyncLock
    End Sub
End Module
