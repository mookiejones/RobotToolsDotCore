using System;
using System.ComponentModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace RobotTools.UI.Editor.Completion;

public class CompletionData : ICompletionData
{
    private string _description = string.Empty;

    //public CodeCompletion(Variable variable)
    //{
    //    Text = variable.Name;
    //    Image = variable.Icon;
    //    Description = variable.Description;
    //}


    [Localizable(false)]
    public CompletionData(string text)
    {
        Text = text;
    }

    public ImageSource Image { get; set; }
    public string Text { get; private set; }

    // Use this property if you want to show a fancy UIElement in the list.
    public object Content
    {
        get { return Text; }
    }


    [Localizable(false)]
    public object Description
    {
        get
        {
            return string.IsNullOrEmpty(_description)
                ? null
                : $"Description for {Text} \r\n {_description}";
        }
        set => _description = (string)value;
    }

    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {

        //string currentWord = main.ActiveEditor.TextBox.FindWord();
        //int offs = completionSegment.Offset - currentWord.Length;
        //// Create New AnchorSegment 
        //textArea.Document.Replace(offs, currentWord.Length, Text);
    }

    public double Priority
    {
        get { return 0; }
    }
}
