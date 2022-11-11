using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using ICSharpCode.AvalonEdit.Document;

using Microsoft.Win32;

using RobotTools.Command;
using RobotTools.ViewModels.Base;

namespace RobotTools.ViewModels
{
    class Workspace : Base.ViewModelBase
  {
    protected Workspace()
    {

    }

    static Workspace _this = new Workspace();

    public static Workspace This
    {
      get { return _this; }
    }

    ObservableCollection<FileViewModel> _files = new ObservableCollection<FileViewModel>();
    ReadOnlyObservableCollection<FileViewModel> _readonyFiles = null;
    public ReadOnlyObservableCollection<FileViewModel> Files
    {
      get
      {
        if (_readonyFiles == null)
          _readonyFiles = new ReadOnlyObservableCollection<FileViewModel>(_files);

        return _readonyFiles;
      }
    }

    ToolViewModel[] _tools = null;

    public IEnumerable<ToolViewModel> Tools
    {
      get
      {
        if (_tools == null)
          _tools = new ToolViewModel[] { FileStats };
        return _tools;
      }
    }

    FileStatsViewModel _fileStats = null;
    public FileStatsViewModel FileStats
    {
      get
      {
        if (_fileStats == null)
          _fileStats = new FileStatsViewModel();

        return _fileStats;
      }
    }

    #region OpenCommand
    RelayCommand _openCommand = null;
    public ICommand OpenCommand
    {
      get
      {
        if (_openCommand == null)
        {
          _openCommand = new RelayCommand((p) => OnOpen(p), (p) => CanOpen(p));
        }

        return _openCommand;
      }
    }

    private bool CanOpen(object parameter)
    {
      return true;
    }

    private void OnOpen(object parameter)
    {
      var dlg = new OpenFileDialog();
      if (dlg.ShowDialog().GetValueOrDefault())
      {
        var fileViewModel = Open(dlg.FileName);
        ActiveDocument = fileViewModel;
      }
    }

    public FileViewModel Open(string filepath)
    {
      var fileViewModel = _files.FirstOrDefault(fm => fm.FilePath == filepath);
      if (fileViewModel != null)
        return fileViewModel;

      fileViewModel = new FileViewModel(filepath);
      _files.Add(fileViewModel);

      return fileViewModel;
    }

    #endregion

    #region NewCommand
    RelayCommand _newCommand = null;
    public ICommand NewCommand
    {
      get
      {
        if (_newCommand == null)
        {
          _newCommand = new RelayCommand((p) => OnNew(p), (p) => CanNew(p));
        }

        return _newCommand;
      }
    }

    private bool CanNew(object parameter)
    {
      return true;
    }

    private void OnNew(object parameter)
    {
      _files.Add(new FileViewModel() { Document = new TextDocument() });
      ActiveDocument = _files.Last();
    }

    #endregion

    #region ActiveDocument

    private FileViewModel _activeDocument = null;
    public FileViewModel ActiveDocument
    {
      get { return _activeDocument; }
      set
      {
        if (_activeDocument != value)
        {
          _activeDocument = value;
          RaisePropertyChanged("ActiveDocument");
          if (ActiveDocumentChanged != null)
            ActiveDocumentChanged(this, EventArgs.Empty);
        }
      }
    }

    public event EventHandler ActiveDocumentChanged;

    #endregion

    #region ADLayout
    private AvalonDockLayoutViewModel mAVLayout = null;

    /// <summary>
    /// Expose command to load/save AvalonDock layout on application startup and shut-down.
    /// </summary>
    public AvalonDockLayoutViewModel ADLayout
    {
      get
      {
        if (this.mAVLayout == null)
          this.mAVLayout = new AvalonDockLayoutViewModel();

        return this.mAVLayout;
      }
    }

    public static string LayoutFileName
    {
      get
      {
        return "Layout.config";
      }
    }
    #endregion ADLayout

    #region Application Properties
    /// <summary>
    /// Get a path to the directory where the application
    /// can persist/load user data on session exit and re-start.
    /// </summary>
    public static string DirAppData
    {
      get
      {
        string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                         System.IO.Path.DirectorySeparatorChar + Workspace.Company;

        try
        {
          if (System.IO.Directory.Exists(dirPath) == false)
            System.IO.Directory.CreateDirectory(dirPath);
        }
        catch
        {
        }

        return dirPath;
      }
    }

    public static string Company
    {
      get
      {
        return "EdiDemo";
      }
    }
    #endregion Application Properties

    #region close save file handling methods
    internal void Close(FileViewModel fileToClose)
    {
      if (fileToClose.IsDirty)
      {
        var res = MessageBox.Show(string.Format("Save changes for file '{0}'?", fileToClose.FileName), "AvalonDock Test App", MessageBoxButton.YesNoCancel);
        if (res == MessageBoxResult.Cancel)
          return;
        if (res == MessageBoxResult.Yes)
        {
          Save(fileToClose);
        }
      }

      _files.Remove(fileToClose);
    }

    internal void Save(FileViewModel fileToSave, bool saveAsFlag = false)
    {
      if (fileToSave.FilePath == null || saveAsFlag)
      {
        var dlg = new SaveFileDialog();
        if (dlg.ShowDialog().GetValueOrDefault())
          fileToSave.FilePath = dlg.SafeFileName;
      }

      File.WriteAllText(fileToSave.FilePath, fileToSave.Document.Text);
      ActiveDocument.IsDirty = false;
    }
    #endregion close save file handling methods

    #region ToggleEditorOptionCommand
    RelayCommand _toggleEditorOptionCommand = null;
    public ICommand ToggleEditorOptionCommand
    {
      get
      {
        if (this._toggleEditorOptionCommand == null)
        {
          this._toggleEditorOptionCommand = new RelayCommand((p) => OnToggleEditorOption(p),
                                                             (p) => CanToggleEditorOption(p));
        }

        return this._toggleEditorOptionCommand;
      }
    }

    private bool CanToggleEditorOption(object parameter)
    {
      if (this.ActiveDocument != null)
        return true;

      return false;
    }

    private void OnToggleEditorOption(object parameter)
    {
      FileViewModel f = this.ActiveDocument;

      if (parameter == null)
        return;

      if ((parameter is ToggleEditorOption) == false)
        return;

      ToggleEditorOption t = (ToggleEditorOption)parameter;

      if (f != null)
      {
        switch (t)
        {
          case ToggleEditorOption.WordWrap:
              f.WordWrap = !f.WordWrap;
            break;

          case ToggleEditorOption.ShowLineNumber:
              f.ShowLineNumbers = !f.ShowLineNumbers;
            break;

          case ToggleEditorOption.ShowSpaces:
            f.TextOptions.ShowSpaces = !f.TextOptions.ShowSpaces;
            break;

          case ToggleEditorOption.ShowTabs:
            f.TextOptions.ShowTabs = !f.TextOptions.ShowTabs;
            break;

          case ToggleEditorOption.ShowEndOfLine:
            f.TextOptions.ShowEndOfLine = !f.TextOptions.ShowEndOfLine;
            break;

          default:
            break;
        }
      }
    }
    #endregion ToggleEditorOptionCommand
  }
}
