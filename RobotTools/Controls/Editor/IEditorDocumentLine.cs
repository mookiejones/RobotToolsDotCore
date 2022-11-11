using ICSharpCode.AvalonEdit.Document;

namespace RobotTools.Controls.Editor
{
    public interface IEditorDocumentLine : IDocumentLine
    {
        string Text { get; }
    }
}
