using ICSharpCode.AvalonEdit.Document;

namespace RobotTools.UI.Editor
{
    public interface IEditorDocumentLine : IDocumentLine
    {
        string Text { get; }
    }
}
