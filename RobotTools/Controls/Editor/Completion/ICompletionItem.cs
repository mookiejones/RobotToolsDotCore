using RobotTools.Controls.Editor.Bookmarks;

namespace RobotTools.Controls.Editor.Completion
{
    public interface ICompletionItem
    {
        string Text { get; }
        string Description { get; }
        IImage Image { get; }
        double Priority { get; }
        void Complete(CompletionContext context);
    }
}
