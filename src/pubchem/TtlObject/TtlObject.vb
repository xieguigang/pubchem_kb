Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.MIME.application.rdf_xml

''' <summary>
''' a general ttl object with ncbi terms
''' </summary>
Public Class TtlObject : Implements INamedValue

    ''' <summary>
    ''' the rdf id
    ''' </summary>
    ''' <returns></returns>
    Public Property id As String Implements INamedValue.Key

    <Field("skos:prefLabel")> Public Property prefLabel As String()
    <Field("skos:relatedMatch")> Public Property relatedMatch As String()
    <Field("rdf:type")> Public Property type As String()
    <Field("skos:altLabel")> Public Property altLabel As String()
    <Field("skos:closeMatch")> Public Property closeMatch As String()

    Public Overrides Function ToString() As String
        Return id
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function CreateObject(rdf As RDFEntity, type As Type) As TtlObject
        Return WriteObject(ttlObj:=Activator.CreateInstance(type), rdf)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function CreateObject(Of T As {New, TtlObject})(rdf As RDFEntity) As T
        Return WriteObject(ttlObj:=New T With {.id = rdf.RDFId}, rdf)
    End Function
End Class

