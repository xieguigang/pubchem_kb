
Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.rdf_xml.Turtle
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("rdf_tools")>
Public Module Rscript

    ''' <summary>
    ''' parse the RDF Turtle document
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("parseTtl")>
    <RApiReturn(GetType(Triple))>
    Public Function ParseTtl(<RRawVectorArgument> file As Object,
                             Optional lazy As Boolean = True,
                             Optional env As Environment = Nothing) As Object

        Dim stream = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

        If stream Like GetType(Message) Then
            Return stream.TryCast(Of Message)
        End If

        Dim reader As New TurtleFile(stream)

        If lazy Then
            Return pipeline.CreateFromPopulator(reader.ReadObjects)
        Else
            Return reader.ReadObjects.ToArray
        End If
    End Function
End Module
