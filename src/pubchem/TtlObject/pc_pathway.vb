
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps

Public Class pc_pathway : Inherits TtlObject

    <Field("owl:sameAs")> Public Property sameAs As String
    <Field("dcterms:source")> Public Property source As String
    <Field("up:organism")> Public Property organism As String
    <Field("dcterms:title")> Public Property title As String()
    <Field("obo:RO_0000057")> Public Property RO_0000057 As String()
    <Field("bp:pathwayComponent")> Public Property pathwayComponent As String()
    <Field("cito:isDiscussedBy")> Public Property isDiscussedBy As String()
    <Field("skos:related")> Public Property related As String()

End Class
