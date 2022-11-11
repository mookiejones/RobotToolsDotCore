using System.Collections.Generic;

namespace RobotTools.UI.Editor.Completion
{
    public interface ICompletionItemList
    {
        IEnumerable<ICompletionItem> Items { get; }
        ICompletionItem SuggestedItem { get; }
        int PreselectionLength { get; }
        bool ContainsAllAvailableItems { get; }
        CompletionItemListKeyResult ProcessInput(char key);
        void Complete(CompletionContext context, ICompletionItem item);
    }
}
