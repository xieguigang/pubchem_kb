Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.MIME.application.rdf_xml

''' <summary>
''' ttl object with ncbi terms
''' </summary>
Public Class TtlObject : Implements INamedValue

    Public Property subject As String Implements INamedValue.Key

    <Field("skos:prefLabel")> Public Property prefLabel As String()
    <Field("skos:relatedMatch")> Public Property relatedMatch As String()
    <Field("rdf:type")> Public Property type As String()
    <Field("skos:altLabel")> Public Property altLabel As String()
    <Field("skos:closeMatch")> Public Property closeMatch As String()

    Public Shared Function CreateObject(rdf As RDFEntity) As TtlObject
        Static fields As Dictionary(Of BindProperty(Of DataFrameColumnAttribute)) = DataFrameColumnAttribute _
            .LoadMapping(Of TtlObject)(mapsAll:=False) _
            .ToDictionary(Function(f) f.Value.field.Name,
                          Function(f)
                              Return f.Value
                          End Function)

        Dim disease As New TtlObject With {.subject = rdf.RDFId}

        For Each propertyData In rdf.Properties
            Dim prop As PropertyInfo = fields(propertyData.Key).member
            Dim strs As String() = propertyData.Value.Properties.Values _
                .Select(Function(si) si.value) _
                .ToArray

            If prop.PropertyType Is GetType(String) Then
                If strs.Length > 1 Then
                    Throw New InvalidCastException
                Else
                    prop.SetValue(disease, strs(0))
                End If
            Else
                Call prop.SetValue(disease, strs)
            End If
        Next

        Return disease
    End Function
End Class
