using ICSharpCode.AvalonEdit.Document;

namespace RobotTools.UI.Editor.Bracket
{
    public interface IBracketSearcher
    {
        BracketSearchResult SearchBracket(TextDocument document, int offset);
    }
}
