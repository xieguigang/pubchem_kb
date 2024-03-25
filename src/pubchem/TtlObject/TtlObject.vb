Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.MIME.application.rdf_xml

''' <summary>
''' a general ttl object with ncbi terms
''' </summary>
Public Class TtlObject : Implements INamedValue

    Public Property subject As String Implements INamedValue.Key

    <Field("skos:prefLabel")> Public Property prefLabel As String()
    <Field("skos:relatedMatch")> Public Property relatedMatch As String()
    <Field("rdf:type")> Public Property type As String()
    <Field("skos:altLabel")> Public Property altLabel As String()
    <Field("skos:closeMatch")> Public Property closeMatch As String()

    Public Overrides Function ToString() As String
        Return subject
    End Function

    Private Shared Function getTtlSchema(type As Type) As Dictionary(Of BindProperty(Of DataFrameColumnAttribute))
        Static fields As New Dictionary(Of Type, Dictionary(Of BindProperty(Of DataFrameColumnAttribute)))

        If Not fields.ContainsKey(type) Then
            fields(type) = DataFrameColumnAttribute _
               .LoadMapping(type, mapsAll:=False) _
               .ToDictionary(Function(f) f.Value.field.Name,
                             Function(f)
                                 Return f.Value
                             End Function)
        End If

        Return fields(type)
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function CreateObject(rdf As RDFEntity, type As Type) As TtlObject
        Return WriteObject(ttlObj:=Activator.CreateInstance(type), rdf)
    End Function

    Private Shared Function WriteObject(ttlObj As TtlObject, rdf As RDFEntity) As TtlObject
        Dim fields = getTtlSchema(ttlObj.GetType)

        ttlObj.subject = rdf.RDFId

        For Each propertyData In rdf.Properties
            Dim prop As PropertyInfo = fields(propertyData.Key).member
            Dim strs As String() = propertyData.Value.Properties.Values _
                .Select(Function(si) si.value) _
                .ToArray

            If prop.PropertyType Is GetType(String) Then
                If strs.Length > 1 Then
                    Throw New InvalidCastException
                Else
                    prop.SetValue(ttlObj, strs(0))
                End If
            Else
                Call prop.SetValue(ttlObj, strs)
            End If
        Next

        Return ttlObj
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Shared Function CreateObject(Of T As {New, TtlObject})(rdf As RDFEntity) As T
        Return WriteObject(ttlObj:=New T With {.subject = rdf.RDFId}, rdf)
    End Function
End Class

