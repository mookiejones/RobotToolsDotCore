﻿using RobotTools.UI.Editor.Completion;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace RobotTools.UI.Editor
{
    public interface ITextEditor : IServiceProvider
    {
        ITextEditor PrimaryView { get; }
        IEditor Document { get; }
        ITextEditorCaret Caret { get; }
        ITextEditorOptions Options { get; }
        int SelectionStart { get; }
        int SelectionLength { get; }
        string SelectedText { get; set; }
        ICompletionListWindow ActiveCompletionWindow { get; }
        IInsightWindow ActiveInsightWindow { get; }
        event EventHandler SelectionChanged;
        event KeyEventHandler KeyPress;
        void Select(int selectionStart, int selectionLength);
        void JumpTo(int line, int column);
        ICompletionListWindow ShowCompletionWindow(ICompletionItemList data);
        IInsightWindow ShowInsightWindow(IEnumerable<IInsightItem> items);
        IEnumerable<ICompletionItem> GetSnippets();
    }
}
