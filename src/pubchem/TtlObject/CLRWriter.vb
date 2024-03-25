Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.MIME.application.rdf_xml

Module CLRWriter

    ''' <summary>
    ''' Parse the clr schema for write data to .net object
    ''' </summary>
    ''' <param name="type"></param>
    ''' <returns></returns>
    Private Function getTtlSchema(type As Type) As Dictionary(Of BindProperty(Of DataFrameColumnAttribute))
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

    ''' <summary>
    ''' write ttl text data to a specific clr object
    ''' </summary>
    ''' <param name="ttlObj">the target clr object to write ttl text data</param>
    ''' <param name="rdf">the ttl text data source</param>
    ''' <returns></returns>
    Friend Function WriteObject(ttlObj As TtlObject, rdf As RDFEntity) As TtlObject
        Dim fields = getTtlSchema(ttlObj.GetType)
        Dim prop As PropertyInfo
        Dim strs As String()
        Dim bind As BindProperty(Of DataFrameColumnAttribute)
        Dim data_scalar As Boolean

        ttlObj.id = rdf.RDFId

        For Each propertyData As KeyValuePair(Of String, RDFEntity) In rdf.Properties
            bind = fields.TryGetValue(propertyData.Key)
            data_scalar = propertyData.Value.Properties.Count <= 1

            If bind Is Nothing OrElse bind.member Is Nothing Then
                Throw New MissingMemberException($"missing rdf ttl member({propertyData.Key}{If(data_scalar, ", probably scalar string", "")}) mapping in clr object({ttlObj.GetType.Name})!")
            End If

            prop = bind.member
            strs = propertyData.Value.Properties.Values _
                .Select(Function(si) si.value) _
                .ToArray

            If prop.PropertyType Is GetType(String) Then
                If strs.Length > 1 Then
                    Throw New InvalidCastException($"an string array from '{propertyData.Key}' could not be set to the '{ttlObj.GetType.Name}::{bind.member.Name}' scalar string value!")
                Else
                    Call prop.SetValue(ttlObj, strs(0))
                End If
            Else
                Call prop.SetValue(ttlObj, strs)
            End If
        Next

        Return ttlObj
    End Function
End Module
