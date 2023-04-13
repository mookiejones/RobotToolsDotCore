using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Utils;

using RobotTools.UI.Editor;

namespace RobotTools.ViewModels
{
    partial class FileViewModel : PaneViewModel
  {

        private WorkspaceViewModel Workspace => Ioc.Default.GetRequiredService<WorkspaceViewModel>();

    #region fields
    static ImageSourceConverter ISC = new ImageSourceConverter();
    #endregion fields

    #region constructor
    public FileViewModel(string filePath)
    {
      FilePath = filePath;
      Title = FileName;

      //Set the icon only for open documents (just a test)
      IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/Images/document.png") as ImageSource;
    }

    public FileViewModel()
    {
      IsDirty = true;
      Title = FileName;
    }
    #endregion constructor

    #region FilePath
    private string _filePath = null;
    public string FilePath
    {
      get { return _filePath; }
      set
      {
        if (_filePath != value)
        {
          _filePath = value;
          OnPropertyChanged(nameof(FilePath));
          OnPropertyChanged(nameof(FileName));
          OnPropertyChanged(nameof(Title));

          if (File.Exists(_filePath))
          {
            _document = new TextDocument();
            HighlightDef = HighlightingManager.Instance.GetDefinition("XML");
            _isDirty = false;
            IsReadOnly = false;
            ShowLineNumbers = false;
            WordWrap = false;

            // Check file attributes and set to read-only if file attributes indicate that
            if ((File.GetAttributes(_filePath) & FileAttributes.ReadOnly) != 0)
            {
              IsReadOnly = true;
              IsReadOnlyReason = "This file cannot be edit because another process is currently writting to it.\n" +
                                      "Change the file access permissions or save the file in a different location if you want to edit it.";
            }

            _document=GetTextDocument(_filePath);


            ContentId = _filePath;
          }
        }
      }
    }
    private TextDocument GetTextDocument(string filePath)
    {
 using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
              using (StreamReader reader = FileReader.OpenStream(fs, Encoding.UTF8))
              {
                return new TextDocument(reader.ReadToEnd());
              }
            }
    }
    #endregion

    #region FileName
    public string FileName
    {
      get
      {
        if (FilePath == null)
          return "Noname" + (IsDirty ? "*" : "");

        return Path.GetFileName(FilePath) + (IsDirty ? "*" : "");
      }
    }
    #endregion FileName

    #region TextContent

    private TextDocument _document = null;
    public TextDocument Document
    {
      get { return _document; }
      set
      {
        if (_document != value)
        {
          _document = value;
                    OnPropertyChanged(nameof(Document));
          IsDirty = true;
        }
      }
    }

    #endregion

    #region HighlightingDefinition

    private IHighlightingDefinition _highlightdef = null;
    public IHighlightingDefinition HighlightDef
    {
      get { return _highlightdef; }
      set
      {
        if (_highlightdef != value)
        {
          _highlightdef = value;
          OnPropertyChanged(nameof(HighlightDef));
          IsDirty = true;
        }
      }
    }

    #endregion

    #region Title
    /// <summary>
    /// Title is the string that is usually displayed - with or without dirty mark '*' - in the docking environment
    /// </summary>
    public new string Title
    {
      get
      {
        return string.IsNullOrEmpty(FilePath)?FileName:(Path.GetFileName(FilePath)  + (IsDirty == true ? "*" : string.Empty));
      }

      set
      {
        base.Title = value;
        OnPropertyChanged(nameof(Title));
      }
    }
    #endregion

    #region IsDirty

    private bool _isDirty = false;
    public bool IsDirty
    {
      get { return _isDirty; }
      set
      {
        if (_isDirty != value)
        {
          _isDirty = value;
          OnPropertyChanged(nameof(IsDirty));
          OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(FileName));
        }
      }
    }

    #endregion

    #region IsReadOnly
    private bool mIsReadOnly = false;
    public bool IsReadOnly
    {
      get
      {
        return mIsReadOnly;
      }

      protected set
      {
        if (mIsReadOnly != value)
        {
          mIsReadOnly = value;
                    OnPropertyChanged(nameof(IsReadOnly));
        }
      }
    }

    private string mIsReadOnlyReason = string.Empty;
    public string IsReadOnlyReason
    {
      get
      {
        return mIsReadOnlyReason;
      }

      protected set
      {
        if (mIsReadOnlyReason != value)
        {
          mIsReadOnlyReason = value;
                    OnPropertyChanged(nameof(IsReadOnlyReason));
        }
      }
    }
    #endregion IsReadOnly

    #region WordWrap
    // Toggle state WordWrap
    private bool mWordWrap = false;
    public bool WordWrap
    {
      get
      {
        return mWordWrap;
      }

      set
      {
        if (mWordWrap != value)
        {
          mWordWrap = value;
                    OnPropertyChanged(nameof(WordWrap));
        }
      }
    }
    #endregion WordWrap

    #region ShowLineNumbers
    // Toggle state ShowLineNumbers
    private bool mShowLineNumbers = false;
    public bool ShowLineNumbers
    {
      get
      {
        return mShowLineNumbers;
      }

      set
      {
        if (mShowLineNumbers != value)
        {
          mShowLineNumbers = value;
                    OnPropertyChanged(nameof(ShowLineNumbers));
        }
      }
    }
    #endregion ShowLineNumbers

    #region EditorOptions
    private EditorOptions mTextOptions
      = new EditorOptions()
      {
        ConvertTabsToSpaces= false,
        IndentationSize = 2
      };

    public EditorOptions TextOptions
    {
      get
      {
        return EditorOptions.Instance;
      }

      set
      {
        if (EditorOptions.Instance != value)
        {
          EditorOptions.Instance = value;
                    OnPropertyChanged(nameof(TextOptions));
        }
      }
    }
        #endregion EditorOptions

        [RelayCommand(CanExecute =nameof(CanSave))]
    private void Save(FileViewModel parameter)=> Workspace.Save(parameter, false);
        

    private static bool CanSave(FileViewModel parameter)=>parameter != null && parameter.IsDirty;
        

    

     

        private static bool CanSaveAs(FileViewModel parameter) => parameter != null && parameter.IsDirty;

        [RelayCommand(CanExecute = nameof(CanSaveAs))]
        private void SaveAs(FileViewModel parameter) => Workspace.Save(parameter, true);


        [RelayCommand]
        private void Close(FileViewModel viewModel) => Workspace.Close(viewModel);

    }
}
