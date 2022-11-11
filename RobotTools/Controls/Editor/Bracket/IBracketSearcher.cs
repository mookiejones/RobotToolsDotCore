using ICSharpCode.AvalonEdit.Document;

namespace RobotTools.Controls.Editor.Bracket
{
    public interface IBracketSearcher
    {
        BracketSearchResult SearchBracket(TextDocument document, int offset);
    }
}
