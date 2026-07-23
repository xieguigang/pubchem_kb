' ============================================================================
' MetabolicReaction.vb - 代谢反应数据模型定义
'
' 这个文件定义了从文献中提取的代谢反应信息的数据结构。
' 每一条代谢反应包含9个字段，严格按照用户需求设计。
'
' 字段说明：
'   1. substrates        - 代谢反应底物英文名称列表
'   2. products          - 代谢反应产物英文名称列表
'   3. reaction_name     - 代谢反应名称
'   4. reaction_description - 代谢反应描述
'   5. enzyme            - 代谢酶信息（基因id、名称、ec_number、蛋白质功能结构域名称列表）
'   6. pathway           - 代谢通路名称
'   7. source_organisms  - 来源物种学名列表
'   8. source_doi        - 来源文献DOI
'   9. source_title      - 来源文献标题
' ============================================================================
Imports System.Text.Json.Serialization
Imports System.Text.Json

Namespace MetabolicAgent.Models

    ''' <summary>
    ''' 代谢酶信息数据模型
    ''' 包含基因ID、酶名称、EC编号、蛋白质功能结构域名称列表
    ''' </summary>
    Public Class EnzymeInfo

        ''' <summary>基因标识符，例如 "HGNC:4845" 或 NCBI Gene ID</summary>
        <JsonPropertyName("gene_id")>
        Public Property gene_id As String

        ''' <summary>酶名称，例如 "hexokinase"</summary>
        <JsonPropertyName("name")>
        Public Property name As String

        ''' <summary>EC编号，例如 "2.7.1.1"</summary>
        <JsonPropertyName("ec_number")>
        Public Property ec_number As String

        ''' <summary>蛋白质功能结构域名称列表，例如 {"Hexokinase_N", "Hexokinase_C"}</summary>
        <JsonPropertyName("protein_domains")>
        Public Property protein_domains As List(Of String)

        Public Sub New()
            protein_domains = New List(Of String)()
        End Sub

    End Class

    ''' <summary>
    ''' 代谢反应数据模型
    ''' 包含9个字段，从文献原文中严谨提取
    ''' </summary>
    Public Class MetabolicReaction

        ''' <summary>字段1: 代谢反应底物英文名称列表</summary>
        <JsonPropertyName("substrates")>
        Public Property substrates As List(Of String)

        ''' <summary>字段2: 代谢反应产物英文名称列表</summary>
        <JsonPropertyName("products")>
        Public Property products As List(Of String)

        ''' <summary>字段3: 代谢反应名称</summary>
        <JsonPropertyName("reaction_name")>
        Public Property reaction_name As String

        ''' <summary>字段4: 代谢反应描述</summary>
        <JsonPropertyName("reaction_description")>
        Public Property reaction_description As String

        ''' <summary>字段5: 代谢酶信息</summary>
        <JsonPropertyName("enzyme")>
        Public Property enzyme As EnzymeInfo

        ''' <summary>字段6: 代谢通路名称</summary>
        <JsonPropertyName("pathway")>
        Public Property pathway As String

        ''' <summary>字段7: 来源物种学名列表</summary>
        <JsonPropertyName("source_organisms")>
        Public Property source_organisms As List(Of String)

        ''' <summary>字段8: 来源文献DOI</summary>
        <JsonPropertyName("source_doi")>
        Public Property source_doi As String

        ''' <summary>字段9: 来源文献标题</summary>
        <JsonPropertyName("source_title")>
        Public Property source_title As String

        Public Sub New()
            substrates = New List(Of String)()
            products = New List(Of String)()
            enzyme = New EnzymeInfo()
            source_organisms = New List(Of String)()
        End Sub

    End Class

    ''' <summary>
    ''' 文献信息数据模型
    ''' 用于在Phase 1搜索阶段保存文献的基本信息
    ''' </summary>
    Public Class PaperInfo

        ''' <summary>PubMed唯一标识符</summary>
        <JsonPropertyName("pmid")>
        Public Property pmid As String

        ''' <summary>文献标题</summary>
        <JsonPropertyName("title")>
        Public Property title As String

        ''' <summary>文献DOI</summary>
        <JsonPropertyName("doi")>
        Public Property doi As String

        ''' <summary>发表年份</summary>
        <JsonPropertyName("year")>
        Public Property year As String

        ''' <summary>文献摘要</summary>
        <JsonPropertyName("abstract")>
        Public Property abstract As String

        ''' <summary>期刊名称</summary>
        <JsonPropertyName("journal")>
        Public Property journal As String

        ''' <summary>作者列表</summary>
        <JsonPropertyName("authors")>
        Public Property authors As String

        ''' <summary>MeSH主题词</summary>
        <JsonPropertyName("mesh_terms")>
        Public Property mesh_terms As String

    End Class

    ''' <summary>
    ''' 文献全文信息数据模型
    ''' 用于在Phase 2获取全文阶段保存文献的完整信息
    ''' </summary>
    Public Class PaperFullText

        <JsonPropertyName("pmid")>
        Public Property pmid As String

        <JsonPropertyName("title")>
        Public Property title As String

        <JsonPropertyName("doi")>
        Public Property doi As String

        <JsonPropertyName("abstract")>
        Public Property abstract As String

        <JsonPropertyName("full_text")>
        Public Property full_text As String

        <JsonPropertyName("mesh_terms")>
        Public Property mesh_terms As String

    End Class

    ''' <summary>
    ''' JSON序列化/反序列化辅助工具
    ''' 提供统一的JSON处理方法，支持中文和缩进格式
    ''' </summary>
    Public Module JsonHelper

        Private ReadOnly s_options As New JsonSerializerOptions With {
            .WriteIndented = True,
            .Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }

        ''' <summary>将对象序列化为JSON字符串（带缩进，支持中文）</summary>
        Public Function ToJson(Of T)(obj As T) As String
            Return JsonSerializer.Serialize(obj, s_options)
        End Function

        ''' <summary>将JSON字符串反序列化为对象</summary>
        Public Function FromJson(Of T)(json As String) As T
            Return JsonSerializer.Deserialize(Of T)(json, s_options)
        End Function

        ''' <summary>尝试将JSON字符串反序列化为对象，失败返回Nothing</summary>
        Public Function TryFromJson(Of T)(json As String, ByRef result As T) As Boolean
            Try
                result = JsonSerializer.Deserialize(Of T)(json, s_options)
                Return result IsNot Nothing
            Catch
                result = Nothing
                Return False
            End Try
        End Function

    End Module

End Namespace
