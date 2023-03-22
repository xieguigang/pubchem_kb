Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.rdf_xml
Imports Microsoft.VisualBasic.MIME.application.rdf_xml.Turtle
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("rdf_tools")>
<RTypeExport("pc_gene", GetType(pc_gene))>
Public Module Rscript

    <ExportAPI("to_ttlobject")>
    <RApiReturn(GetType(TtlObject))>
    Public Function ParseTtlEntityData(<RRawVectorArgument>
                                       ttl As Object,
                                       Optional [typeof] As Object = Nothing,
                                       Optional env As Environment = Nothing) As Object

        Dim rdf_ttl As pipeline = pipeline.TryCreatePipeline(Of Triple)(ttl, env)
        Dim type As RType = env.globalEnvironment.GetType([typeof])
        Dim ttl_type As Type

        If type Is Nothing OrElse Not type.raw.IsInheritsFrom(GetType(TtlObject)) Then
            ttl_type = GetType(TtlObject)
        Else
            ttl_type = type.raw
        End If

        If rdf_ttl.isError Then
            Return rdf_ttl.getError
        End If

        Dim rdf As RDFEntity() = rdf_ttl _
            .populates(Of Triple)(env) _
            .PopulateObjects _
            .ToArray
        Dim pubchem_ttl As TtlObject() = New TtlObject(rdf.Length - 1) {}

        For i As Integer = 0 To pubchem_ttl.Length - 1
            pubchem_ttl(i) = TtlObject.CreateObject(rdf(i), ttl_type)
        Next

        Return pubchem_ttl
    End Function
End Module
