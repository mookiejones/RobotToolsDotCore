using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using RobotTools.UI.Editor.Bracket;
using System;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using RobotTools.UI.Editor.Completion;
using ICSharpCode.AvalonEdit.Search;
using RobotTools.UI.Editor.IconBar;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit.CodeCompletion;
using RobotTools.UI.Editor.Snippets;
using System.IO;
using System.Windows.Media;
using RobotTools.UI.Editor.Languages;

namespace RobotTools.UI.Editor;

public partial class AvalonEditor:TextEditor,INotifyPropertyChanged
{


    #region INotifyPropertyChanged Implementation
    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    #endregion

    #region Const
    private const string CATEGORY = "Editor Properties";
    #endregion

    #region Members
    private ToolTip _toolTip = new ToolTip();
    private readonly IconBarManager _iconBarManager;
    private readonly IconBarMargin _iconBarMargin;

    public IList<ICompletionDataProvider> CompletionDataProviders { get; set; }

    #endregion

    #region Properties

    public FileLanguage FileLanguage { get; set; } 

    #region Filename

    private string _fileName;

    [Category("Editor Properties")]
    [Description("Name of file in editor")]
    public string Filename
    {
        get => _fileName;
        set
        {
            _fileName = value;
            RaisePropertyChanged(nameof(Filename));
        }
    }

    /// <summary>
    /// The bindable text property dependency property
    /// </summary>
    public static readonly DependencyProperty FilenameProperty =
        DependencyProperty.Register("Filename", typeof(string), typeof(AvalonEditor), new PropertyMetadata(HandleFilenamePropertyChanged)
        );

    private static void HandleFilenamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var target = d as AvalonEditor;
        target.Filename = (string)e.NewValue;
        target.FileLanguage = new KukaLanguage(target.Filename);
        target.UpdateFolds();

    }
    #endregion

    #region Text
    /// <summary>
    /// A bindable Text property
    /// </summary>
    public new string Text
    {
        get => base.Text;
        set
        {
            base.Text = value;
            // SetValue(TextProperty, value);
            RaisePropertyChanged(nameof(Text));
            HandleTextChanged();
        }
    }


    /// <summary>
    /// The bindable text property dependency property
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(AvalonEditor), new PropertyMetadata(HandleTextPropertyChanged));

    private static void HandleTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var target = d as AvalonEditor;
        target.Text = (string)e.NewValue;

    }

    protected override void OnTextChanged(EventArgs e)
    {

        base.OnTextChanged(e);

        RaisePropertyChanged(nameof(Text));
    }
    #endregion

    public new bool ShowLineNumbers
    {
        get => Options.ShowLineNumbers;
        set {
            Options.ShowLineNumbers = value;
            base.ShowLineNumbers = value;
        }
    }

    private static EditorOptions _textOptions;

    [Category(CATEGORY)]
    [Description("Text Editor Options")]
    public new EditorOptions Options
    {
        get => EditorOptions.Instance;
        set => Options = value;
    }

    [



    Category(CATEGORY)]
    [Description("Current Line")]
    public int Line => TextArea.Caret.Column;


    [Category(CATEGORY)]
    [Description("Current Column of Caret")]
    public int Column => TextArea.Caret.Column;

    [Category(CATEGORY)]
    [Description("Current offset of Caret in editor.")]
    public int Offset => TextArea.Caret.Offset;


    #endregion


    partial void RegisterEvents();
    public AvalonEditor()
    {
        ChangeCommandBindings();
        CompletionDataProviders = new List<ICompletionDataProvider>()
               {
                  new SnippetCompletionDataProvider()
             };
        _iconBarMargin = new IconBarMargin(_iconBarManager = new IconBarManager());
        InitializeEditor();

        MouseHoverStopped += (s, e) => _toolTip.IsOpen = false;

        SetHighlighting();

        RegisterEvents();

        base.Options = EditorOptions.Instance;

        InvokeModifiedChanged(false);
    }

    public event EventHandler IsModifiedChanged;
    public void InvokeModifiedChanged(bool isNowModified)
    {
        IsModified = isNowModified;
        if (IsModifiedChanged != null)
            IsModifiedChanged(this, new EventArgs());

    }

    private void ChangeCommandBindings()
    {

        var bindings = TextArea.DefaultInputHandler.Editing.CommandBindings
           .Where(o => o.Command == AvalonEditCommands.DeleteLine);

        foreach (var binding in bindings)
        {
            var keyGesture = new KeyGesture(Key.L, ModifierKeys.Control);
            var command = new RoutedCommand("DeleteLine", typeof(AvalonEditor), new InputGestureCollection { keyGesture });
            binding.Command = command;
        }
        ICollection<CommandBinding> commandBindings = TextArea.DefaultInputHandler.Editing.CommandBindings;



    }


    private void InitializeEditor()
    {
        TextArea.LeftMargins.Insert(0, _iconBarMargin);
        SearchPanel.Install(TextArea);
        TextArea.TextEntered += TextEntered;

        TextArea.Caret.PositionChanged += CaretPositionChanged;

    }
    protected override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
        switch (managerType.Name)
        {
            case "TextChanged":
                FindBookmarkMembers();
                //IsModified = true;
                UpdateFolds();
                break;
        }
        return base.ReceiveWeakEvent(managerType, sender, e);
    }
    /// <summary>
    /// Evaluates each line in selection and Comments/Uncomments "Each Line"
    /// </summary>
    private void ToggleComment()
    {
        //No point in commenting if I dont know the Language
        if (FileLanguage == null) return;

        // Get Comment to insert
        var start = Document.GetLineByOffset(SelectionStart);
        var end = Document.GetLineByOffset(SelectionStart + SelectionLength);

        using (Document.RunUpdate())
        {
            for (var line = start; line.LineNumber < end.LineNumber + 1; line = line.NextLine)
            {
                var currentline = GetLine(line.LineNumber);

                // Had to put in comment offset for Fanuc 
                if (FileLanguage.IsLineCommented(currentline))
                    Document.Insert(FileLanguage.CommentOffset(currentline) + line.Offset,
                                    FileLanguage.CommentChar);
                else
                {
                    var replacestring = FileLanguage.CommentReplaceString(currentline);
                    Document.Replace(line.Offset, currentline.Length, replacestring);
                }
            }
        }
    }

    private void TextEntered(object sender, TextCompositionEventArgs e)
    {
        if (!base.IsReadOnly && e.Text.Length == 1)
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
        var ext = Path.GetExtension(Filename);
        if (SnippetManager.HasSnippetsFor(wordBeforeCaret, ext))
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
        HandleTextEntered(sender,e);

    }
    private void ExecuteTextChanged(object sender, EventArgs e)
    {
        HandleTextChanged();
        OnTextChanged(sender, e);
    }

    private void HandleTextChanged()
    {
        var wordBeforeCaret = this.GetWordBeforeCaret(GetWordParts());

        var ext = Path.GetExtension(Filename);
        if (SnippetManager.HasSnippetsFor(wordBeforeCaret, ext))
        {
            insightWindow = new InsightWindow(TextArea)
            {
                Content = "Press tab to enter snippet",
                Background = Brushes.Linen
            };
            insightWindow.Show();
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


    private void CaretPositionChanged(object sender, EventArgs e)
    {
        var caret = sender as Caret;
        UpdateLineTransformers();
        if (caret != null)
        {
            RaisePropertyChanged(nameof(Line));
            RaisePropertyChanged(nameof(Column));
            RaisePropertyChanged(nameof(Offset));
            //FileSave = ((!string.IsNullOrEmpty(Filename))
            //    ? File.GetLastWriteTime(Filename).ToString(CultureInfo.InvariantCulture)
            //    : string.Empty);
        }
        HighlightBrackets(sender, e);
    }


    private void UpdateLineTransformers()
    {
        TextArea.TextView.BackgroundRenderers.Clear();


        TextArea.TextView.BackgroundRenderers.Add(new BackgroundRenderer(Document.GetLineByOffset(CaretOffset)));

        if (_bracketRenderer == null)
        {
            _bracketRenderer = new BracketHighlightRenderer(TextArea.TextView);
        }
        else
        {
            TextArea.TextView.BackgroundRenderers.Add(_bracketRenderer);
        }
    }
}
