using ICSharpCode.AvalonEdit.Document;
using RobotTools.Controls.Editor.Completion;
using System;
using System.Collections.Generic;

namespace RobotTools.Controls.Editor
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
