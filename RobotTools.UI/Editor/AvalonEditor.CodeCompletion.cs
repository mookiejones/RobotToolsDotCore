using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using RobotTools.UI.Editor.Completion;
using RobotTools.UI.Editor.Languages;
using RobotTools.UI.Editor.Snippets;

namespace RobotTools.UI.Editor;

public partial class AvalonEditor
{
    #region Members

    private InsightWindow insightWindow;
    #endregion

    #region Properties

    public bool UseCodeCompletion { get; set; } = true;

    public static readonly DependencyProperty CompletionWindowProperty =
DependencyProperty.Register("CompletionWindow", typeof(CompletionWindow), typeof(AvalonEditor));
    private CompletionWindow CompletionWindow
    {
        get => (CompletionWindow)GetValue(CompletionWindowProperty);
        set => SetValue(CompletionWindowProperty, value);
    }
    #endregion

    private void Complete(char newChar)
    {
    }

   

    private string Extension => Path.GetExtension(Filename);
    partial void HandleTextEntered(object sender, TextCompositionEventArgs e)
    {
        if (!IsReadOnly && e.Text.Length == 1)
        {
            var newChar = e.Text[0];
            if (UseCodeCompletion)
            {
                Complete(newChar);
            }
        }
        if (CompletionWindow != null)
        {
            return;
        }

        string wordBeforeCaret = this.GetWordBeforeCaret(this.GetWordParts());

        if (SnippetManager.HasSnippetsFor(wordBeforeCaret,Extension))// this.DocumentType))
        {
            insightWindow = new InsightWindow(base.TextArea)
            {
                Content = "Press tab to enter snippet",
                Background = Brushes.Linen
            };
            this.insightWindow.Show();
            return;
        }
              
            var text = FindWord();
            if (IsModified || IsModified)
            {
                UpdateFolds();
            }
            if (text == null || !(string.IsNullOrEmpty(text) | text.Length < 3))
            {
                ShowCompletionWindow(text);
            }
        

    }

    private void ShowCompletionWindow(string currentword)
    {
        CompletionWindow = new CompletionWindow(TextArea);
        var completionItems = GetCompletionItems();
        foreach (var current in completionItems)
        {
            CompletionWindow.CompletionList.CompletionData.Add(current);
        }
        CompletionWindow.Closed += delegate { CompletionWindow = null; };
        CompletionWindow.CloseWhenCaretAtBeginning = true;
        CompletionWindow.CompletionList.SelectItem(currentword);
        if (CompletionWindow.CompletionList.SelectedItem != null)
        {
            CompletionWindow.Show();
        }
    }

    private IEnumerable<ICompletionData> GetCompletionItems()
    {
        var list = new List<ICompletionData>();
        list.AddRange(HighlightList());
        list.AddRange(ObjectBrowserCompletionList());
        return list.ToArray();
    }

    private IEnumerable<ICompletionData> HighlightList()
    {


        var items = new List<ICompletionData>();
        /*
        foreach (var current in
            from rule in SyntaxHighlighting.MainRuleSet.Rules
            select rule.Regex.ToString()
                into parseString
            let start = parseString.IndexOf(">", StringComparison.Ordinal) + 1
            let end = parseString.LastIndexOf(")", StringComparison.Ordinal)
            select parseString.Substring(start, end - start)
                    into parseString1
            select parseString1.Split(new[]
    {
                '|'
            })
                        into spl
            from item in
            from t in spl
            where !string.IsNullOrEmpty(t)
            select new CodeCompletion(t.Replace("\\b", ""))
                    into item
            where !items.Contains(item) && char.IsLetter(item.Text, 0)
            select item
            select item)
        {
            items.Add(current);
        }
        */
        return items.ToArray();
    }

    private IEnumerable<ICompletionData> ObjectBrowserCompletionList()
    {
        return (
            from v in FileLanguage.Instance.Fields
            where v.Type != "def" && v.Type != "deffct"
            select new CompletionData(v.Name)
            {
                Image = v.Icon
            }).ToArray<ICompletionData>();
    }

    public string FindWord()
    {
        var line = GetLine(TextArea.Caret.Line);
        var text = line;
        var anyOf = new[]
        {
            ' ',
            '=',
            '(',
            ')',
            '[',
            ']',
            '<',
            '>',
            '\r',
            '\n'
        };
        var num = line.IndexOfAny(anyOf, TextArea.Caret.Column - 1);
        if (num > -1)
        {
            text = line.Substring(0, num);
        }
        var num2 = text.LastIndexOfAny(anyOf) + 1;
        if (num2 > -1)
        {
            text = text.Substring(num2).Trim();
        }
        return text;
    }
    private string GetLine(int idx)
    {
        var lineByNumber = Document.GetLineByNumber(idx);
        return Document.GetText(lineByNumber.Offset, lineByNumber.Length);
    }
    private void HandleTextEntering(object sender, TextCompositionEventArgs e)
    {

    }


    public  string GetWordBeforeCaret()
    {
        var offset = TextArea.Caret.Offset;
        var num = Document.FindPrevWordStart(offset);
        if (num < 0)
        {
            return string.Empty;
        }
        return Document.GetText(num, offset - num);
    }

  

    protected internal virtual char[] GetWordParts()
    {
        return new char[0];
    }
}
