using ICSharpCode.AvalonEdit.Document;
using RobotTools.UI.Editor.Completion;
using System;
using System.Collections.Generic;

namespace RobotTools.UI.Editor
{
    public interface IInsightWindow : ICompletionWindow
    {
        IList<IInsightItem> Items { get; }
        IInsightItem SelectedItem { get; set; }
        event EventHandler<TextChangeEventArgs> DocumentChanged;
        event EventHandler SelectedItemChanged;
        event EventHandler CaretPositionChanged;
    }
}
