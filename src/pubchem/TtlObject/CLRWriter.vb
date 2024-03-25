Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.MIME.application.rdf_xml

Module CLRWriter

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

    Friend Function WriteObject(ttlObj As TtlObject, rdf As RDFEntity) As TtlObject
        Dim fields = getTtlSchema(ttlObj.GetType)
        Dim prop As PropertyInfo
        Dim strs As String()
        Dim bind As BindProperty(Of DataFrameColumnAttribute)

        ttlObj.subject = rdf.RDFId

        For Each propertyData In rdf.Properties
            bind = fields.TryGetValue(propertyData.Key)

            If bind Is Nothing OrElse bind.member Is Nothing Then
                Throw New MissingMemberException($"missing rdf ttl member({propertyData.Key}) mapping in clr object({ttlObj.GetType.Name})!")
            End If

            prop = bind.member
            strs = propertyData.Value.Properties.Values _
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
End Module
