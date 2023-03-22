Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.MIME.application.rdf_xml.Turtle

Module Utils

    <Extension>
    Public Function FilterCid(data As ttl_property) As ttl_property
        ' descriptor:CID2075_Canonical_SMILES
        Dim cid As String = data.subject.Match("CID\d+").Replace("CID", "")
        Dim dataStr As String = data.value.GetStackValue("""", """")

        data.subject = cid
        data.value = dataStr

        Return data
    End Function
End Module
