Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.rdf_xml
Imports Microsoft.VisualBasic.MIME.application.rdf_xml.Turtle
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("rdf_tools")>
Public Module Rscript

    <ExportAPI("to_disease")>
    <RApiReturn(GetType(Disease))>
    Public Function ParseDiseaseData(<RRawVectorArgument> ttl As Object, Optional env As Environment = Nothing) As Object
        Dim rdf_ttl As pipeline = pipeline.TryCreatePipeline(Of Triple)(ttl, env)

        If rdf_ttl.isError Then
            Return rdf_ttl.getError
        End If

        Dim rdf As RDFEntity() = rdf_ttl _
            .populates(Of Triple)(env) _
            .PopulateObjects _
            .ToArray
        Dim disease As Disease() = New Disease(rdf.Length - 1) {}

        For i As Integer = 0 To disease.Length - 1
            disease(i) = pubchem.Disease.CreateObject(rdf(i))
        Next

        Return disease
    End Function
End Module
