Imports System.Drawing
Imports System.Text.RegularExpressions
Imports MathLab.TextEditor

Public Class SyntaxHighlight

    Dim _editor As FastColoredTextBox

#Region "Highlight styles"

    Dim BlueStyle As TextStyle = New TextStyle(Brushes.Blue, Nothing, FontStyle.Regular)
    Dim BoldStyle As TextStyle = New TextStyle(Nothing, Nothing, FontStyle.Bold Or FontStyle.Underline)
    Dim GrayStyle As TextStyle = New TextStyle(Brushes.Gray, Nothing, FontStyle.Regular)
    Dim MagentaStyle As TextStyle = New TextStyle(Brushes.Magenta, Nothing, FontStyle.Regular)
    Dim GreenStyle As TextStyle = New TextStyle(Brushes.Green, Nothing, FontStyle.Italic)
    Dim BrownStyle As TextStyle = New TextStyle(Brushes.Brown, Nothing, FontStyle.Italic)
    Dim MaroonStyle As TextStyle = New TextStyle(Brushes.Maroon, Nothing, FontStyle.Regular)
    Dim SameWordsStyle As MarkerStyle = New MarkerStyle(New SolidBrush(Color.FromArgb(40, Color.Gray)))

#End Region

    Sub New(editor As FastColoredTextBox)
        _editor = editor
    End Sub

    Public Sub SyntaxHighlightHandler(sender As System.Object, e As TextChangedEventArgs)
        _editor.LeftBracket = "("
        _editor.RightBracket = ")"
        _editor.LeftBracket2 = "\x0"
        _editor.RightBracket2 = "\x0"
        'clear style of changed range
        e.ChangedRange.ClearStyle(BlueStyle, BoldStyle, GrayStyle, MagentaStyle, GreenStyle, BrownStyle)
        'string highlighting
        e.ChangedRange.SetStyle(BrownStyle, """.*?""|'.+?'")
        'comment highlighting
        e.ChangedRange.SetStyle(GreenStyle, "//.*$", RegexOptions.Multiline)
        e.ChangedRange.SetStyle(GreenStyle, "(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline)
        e.ChangedRange.SetStyle(GreenStyle, "(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline Or RegexOptions.RightToLeft)
        'number highlighting
        e.ChangedRange.SetStyle(MagentaStyle, "\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b")
        'attribute highlighting
        e.ChangedRange.SetStyle(GrayStyle, "^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline)
        'class name highlighting
        e.ChangedRange.SetStyle(BoldStyle, "\b(class|struct|enum|interface)\s+(?<range>\w+?)\b")
        'keyword highlighting
        e.ChangedRange.SetStyle(BlueStyle, "\b(abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|virtual|void|volatile|while|add|alias|ascending|descending|dynamic|from|get|global|group|into|join|let|orderby|partial|remove|select|set|value|var|where|yield)\b|#region\b|#endregion\b")

        'clear folding markers
        e.ChangedRange.ClearFoldingMarkers()
        'set folding markers
        e.ChangedRange.SetFoldingMarkers("{", "}") 'allow to collapse brackets block
        e.ChangedRange.SetFoldingMarkers("#region\b", "#endregion\b") 'allow to collapse #region blocks
        e.ChangedRange.SetFoldingMarkers("/\*", "\*/") 'allow to collapse comment block
    End Sub
End Class
