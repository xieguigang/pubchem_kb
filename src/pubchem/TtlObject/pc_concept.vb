
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps

Public Class pc_concept : Inherits TtlObject

    <Field("pav:importedFrom")> Public Property importedFrom As String
    <Field("skos:inScheme")> Public Property inScheme As String
    <Field("skos:broader")> Public Property broader As String

End Class
