Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ApplicationServices.Terminal.ProgressBar
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.MIME.application.rdf_xml
Imports Microsoft.VisualBasic.MIME.application.rdf_xml.Turtle
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' 
''' </summary>
<Package("rdf_tools")>
<RTypeExport("pc_gene", GetType(pc_gene))>
<RTypeExport("pc_concept", GetType(pc_concept))>
<RTypeExport("pc_gene_disease", GetType(pc_gene_disease))>
<RTypeExport("pc_source", GetType(pc_source))>
<RTypeExport("pc_pathway", GetType(pc_pathway))>
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

        Dim rdf As IEnumerable(Of RDFEntity) = rdf_ttl _
            .populates(Of Triple)(env) _
            .PopulateObjects
        Dim pubchem_ttl As New List(Of TtlObject)

        For Each obj As RDFEntity In Tqdm.Wrap(rdf.ToArray)
            Call pubchem_ttl.Add(TtlObject.CreateObject(obj, ttl_type))
        Next

        Return pubchem_ttl.ToArray
    End Function

    <ExportAPI("get_cid_tupleData")>
    <RApiReturn(GetType(ttl_property))>
    Public Function getCompoundsData(<RRawVectorArgument>
                                     file As Object,
                                     Optional lazy As Boolean = False,
                                     Optional env As Environment = Nothing) As Object

        Return env.extractData(file, lazy, ttl_filter:=AddressOf Utils.FilterCid)
    End Function

    <Extension>
    Private Function extractData(env As Environment,
                                 file As Object,
                                 lazy As Boolean,
                                 ttl_filter As Func(Of ttl_property, ttl_property)) As Object

        Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If

        Dim stream = ttl_property.LoadTuples(buf.TryCast(Of Stream)).Select(ttl_filter)

        If lazy Then
            Return pipeline.CreateFromPopulator(stream)
        Else
            Return stream.ToArray
        End If
    End Function

    <ExportAPI("get_hash_tupleData")>
    <RApiReturn(GetType(ttl_property))>
    Public Function getCompoundsData2(<RRawVectorArgument>
                                      file As Object,
                                      Optional lazy As Boolean = False,
                                      Optional env As Environment = Nothing) As Object

        Return env.extractData(file, lazy, ttl_filter:=AddressOf Utils.FilterHashData)
    End Function
End Module
