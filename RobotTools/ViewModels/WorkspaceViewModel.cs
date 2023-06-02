using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls.Dialogs;

using Microsoft.Win32;

using RobotTools.Controls.MRU;
using RobotTools.Core.Utilities;
using RobotTools.Services;
using RobotTools.UI.AngleConverter;
using RobotTools.UI.Editor;
using RobotTools.UI.ViewModels.Base;

namespace RobotTools.ViewModels;

partial class WorkspaceViewModel: ObservableValidator
{

    private IDialogCoordinator _dialogCoordinator;

    #region Members
    private IFileService _fileService;
    #endregion

    #region Properties
    #region Files

    
    public ObservableCollection<FileViewModel> Files { get; set; }= new ObservableCollection<FileViewModel>();
    #endregion



    #region Options
    
    public EditorOptions Options
    {
        get => EditorOptions.Instance;
        set
        {
            EditorOptions.Instance = value;
            OnPropertyChanged(nameof(Options));
        }
    }

    #endregion

    [ObservableProperty]
    private bool showOptions;


     

    #region Tools

    ToolViewModel[] _tools = null;

    public IEnumerable<ToolViewModel> Tools
    {
        get
        {
            if (_tools == null)
                _tools = new ToolViewModel[] { FileStats, RecentFiles, AngleConverter };
            return _tools;
        }
    }
    #endregion

    private AngleConverterViewModel _angleConverterViewModel;

    public AngleConverterViewModel AngleConverter => _angleConverterViewModel ??= new AngleConverterViewModel();



    #region FileStats

    FileStatsViewModel _fileStats = null;
    public FileStatsViewModel FileStats=>_fileStats ??= new FileStatsViewModel();

    #endregion


    #region ActiveDocument

    private FileViewModel _activeDocument = null;
    public FileViewModel ActiveDocument
    {
        get => _activeDocument;
        set
        {
            if (_activeDocument != value)
            {
                _activeDocument = value;
                OnPropertyChanged(nameof(ActiveDocument));
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
    public AvalonDockLayoutViewModel ADLayout=>mAVLayout??= new AvalonDockLayoutViewModel();

    public static string LayoutFileName=>"Layout.config";
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
                                             Path.DirectorySeparatorChar + ApplicationHelper.Company;

            try
            {
                if (Directory.Exists(dirPath) == false)
                    Directory.CreateDirectory(dirPath);
            }
            catch
            {
            }

            return dirPath;
        }
    }

    public static string Company => "EdiDemo";
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

        // Get Position of file
        var fileToRemove = Files.FirstOrDefault(o=>o.FileName== fileToClose.FileName);
        var index = Files.IndexOf(fileToRemove);
        Files.RemoveAt(index);
        //Files.RemoveAt(index);
//            Files.Remove(fileToRemove);

        switch(index){
            case -1:
            case 1:
            ActiveDocument=Files.FirstOrDefault();
            break;
            default:
            ActiveDocument=Files[index];
            break;
        }
        OnPropertyChanged(nameof(Files));



    }

    internal void Save(FileViewModel fileToSave, bool saveAsFlag = false)
    {
        _activeDocument=fileToSave;
        bool canSave=true;
        if (fileToSave.FilePath == null || saveAsFlag)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName=fileToSave.FileName;
            if (dlg.ShowDialog().GetValueOrDefault())
                fileToSave.FilePath = dlg.SafeFileName;
                else
                canSave=false;
        }
if(canSave)
        File.WriteAllText(fileToSave.FilePath, fileToSave.Document.Text);
        ActiveDocument.IsDirty = false;
    }
    #endregion close save file handling methods


    #endregion

    #region Commands

  
    #region ToggleEditorOptionCommand
   

    private bool CanToggleEditorOption(object parameter)
    {
        if (ActiveDocument != null)
            return true;

        return false;
    }
    [RelayCommand(CanExecute =nameof(CanToggleEditorOption))]
    private void ToggleEditorOption(object parameter)
    {
        FileViewModel f = ActiveDocument;

        if (parameter == null)
            return;

        if ((parameter is ToggleEditorOption) == false)
            return;

        var t = (ToggleEditorOption)parameter;

        if (f != null)
        {
            switch (t)
            {
                case ViewModels.ToggleEditorOption.WordWrap:
                    f.WordWrap = !f.WordWrap;
                    break;

                case ViewModels.ToggleEditorOption.ShowLineNumber:
                    f.ShowLineNumbers = !f.ShowLineNumbers;
                    break;

                case ViewModels.ToggleEditorOption.ShowSpaces:
                    f.TextOptions.ShowSpaces = !f.TextOptions.ShowSpaces;
                    break;

                case ViewModels.ToggleEditorOption.ShowTabs:
                    f.TextOptions.ShowTabs = !f.TextOptions.ShowTabs;
                    break;

                case ViewModels.ToggleEditorOption.ShowEndOfLine:
                    f.TextOptions.ShowEndOfLine = !f.TextOptions.ShowEndOfLine;
                    break;

                default:
                    break;
            }
        }
    }
    #endregion ToggleEditorOptionCommand



    #endregion



    #region Recent File List Pin Unpin Commands
    /// <summary>
    /// This property manages the data visible in the Recent Files ViewModel.
    /// </summary>
    private RecentFilesViewModel _recentFiles = null;
    public RecentFilesViewModel RecentFiles
    {
        get
        {
            if (_recentFiles == null)
                _recentFiles = new RecentFilesViewModel();

            return _recentFiles;
        }
    }

    private void PinCommand_Executed(object o, ExecutedRoutedEventArgs e)
    {
        var cmdParam = o as MRUEntryVM;

        if (cmdParam == null)
            return;

        if (e != null)
            e.Handled = true;

        RecentFiles.MruList.PinUnpinEntry(!cmdParam.IsPinned, cmdParam);
    }

    private void AddMRUEntry_Executed(object o, ExecutedRoutedEventArgs e)
    {
        var cmdParam = o as MRUEntryVM;

        if (cmdParam == null)
            return;

        if (e != null)
            e.Handled = true;

        RecentFiles.MruList.AddMRUEntry(cmdParam);
    }

    private void RemoveMRUEntry_Executed(object o, ExecutedRoutedEventArgs e)
    {
        var cmdParam = o as MRUEntryVM;

        if (cmdParam == null)
            return;

        if (e != null)
            e.Handled = true;

        RecentFiles.MruList.RemovePinEntry(cmdParam);
    }
    #endregion Recent File List Pin Unpin Commands

    /// <summary>
    /// Bind a window to some commands to be executed by the viewmodel.
    /// </summary>
    /// <param name="win"></param>
    public void InitCommandBinding(Window win)
    {
        win.CommandBindings.Add(new CommandBinding(ApplicationCommands.New,
        (s, e) =>
        {
            OnNew();
        }));

        win.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open,
        (s, e) =>Open()
    ));

        win.CommandBindings.Add(new CommandBinding(AppCommand.LoadFile,
        (s, e) =>
        {
            if (e == null)
                return;

            string filename = e.Parameter as string;

            if (filename == null)
                return;

            Open(filename);
        }));

        win.CommandBindings.Add(new CommandBinding(AppCommand.PinUnpin,
        (s, e) =>
        {
            PinCommand_Executed(e.Parameter, e);
        }));

        win.CommandBindings.Add(new CommandBinding(AppCommand.RemoveMruEntry,
        (s, e) =>
        {
            RemoveMRUEntry_Executed(e.Parameter, e);
        }));

        win.CommandBindings.Add(new CommandBinding(AppCommand.AddMruEntry,
        (s, e) =>
        {
            AddMRUEntry_Executed(e.Parameter, e);
        }));

        win.CommandBindings.Add(new CommandBinding(AppCommand.BrowseURL,
        (s, e) =>
        {
            Process.Start(new ProcessStartInfo("http://Edi.codeplex.com"));
        }));


    }


    private void OpenFiles()
    {
        foreach (var file in _fileService.Files)
        {
            var fileModel = _fileService.Open(file);
            Files.Add(fileModel);
            RecentFiles.AddNewEntryIntoMRU(file);
            ActiveDocument = fileModel;
        }
    }



    public WorkspaceViewModel(IFileService fileService, IDialogCoordinator dialogCoordinator)
    {
        _fileService = fileService;
        _dialogCoordinator = dialogCoordinator;

        OpenFiles();
    }

    /// <summary>
    /// Empty Constructor for Design View
    /// </summary>
    public WorkspaceViewModel()
    {

    }
}
