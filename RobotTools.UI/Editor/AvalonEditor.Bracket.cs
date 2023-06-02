using RobotTools.UI.Editor.Bracket;
using System;


namespace RobotTools.UI.Editor;

public partial class AvalonEditor
{
    private BracketHighlightRenderer _bracketRenderer;


    private readonly AvalonEditorBracketSearcher _bracketSearcher = new AvalonEditorBracketSearcher();

    /// <summary>
    ///     HighlightBrackets
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    // ReSharper disable UnusedParameter.Local
    private void HighlightBrackets(object sender, EventArgs e)
    {
        var highlight = _bracketSearcher.SearchBracket(Document, TextArea.Caret.Offset);
        _bracketRenderer.SetHighlight(highlight);
    }
}
