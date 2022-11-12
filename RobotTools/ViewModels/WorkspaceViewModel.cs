using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ICSharpCode.AvalonEdit.Document;

using Microsoft.Win32;

using RobotTools.Controls.MRU;
using RobotTools.ViewModels.Base;

namespace RobotTools.ViewModels
{
    class WorkspaceViewModel: ObservableValidator
    {

        #region Properties
        #region Files
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
        #endregion


        #region Tools

        ToolViewModel[] _tools = null;

        public IEnumerable<ToolViewModel> Tools
        {
            get
            {
                if (_tools == null)
                    _tools = new ToolViewModel[] { FileStats, RecentFiles };
                return _tools;
            }
        }
        #endregion

        #region FileStats

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
        #endregion

        #endregion

        #region Commands
        #region OpenCommand
        RelayCommand _openCommand = null;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(OnOpen,CanOpen);
                }

                return _openCommand;
            }
        }

        private bool CanOpen( )
        {
            return true;
        }

        private void OnOpen( )
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
            RecentFiles.AddNewEntryIntoMRU(filepath);
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
                    _newCommand = new RelayCommand( OnNew,CanNew);
                }

                return _newCommand;
            }
        }

        private bool CanNew( )
        {
            return true;
        }

        private void OnNew( )
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
        public AvalonDockLayoutViewModel ADLayout
        {
            get
            {
                if (mAVLayout == null)
                    mAVLayout = new AvalonDockLayoutViewModel();

                return mAVLayout;
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
        RelayCommand<object> _toggleEditorOptionCommand = null;
        public ICommand ToggleEditorOptionCommand
        {
            get
            {
                if (_toggleEditorOptionCommand == null)
                {
                    _toggleEditorOptionCommand = new RelayCommand<object>(OnToggleEditorOption,CanToggleEditorOption);
                }

                return _toggleEditorOptionCommand;
            }
        }

        private bool CanToggleEditorOption(object parameter)
        {
            if (ActiveDocument != null)
                return true;

            return false;
        }

        private void OnToggleEditorOption(object parameter)
        {
            FileViewModel f = ActiveDocument;

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
            MRUEntryVM cmdParam = o as MRUEntryVM;

            if (cmdParam == null)
                return;

            if (e != null)
                e.Handled = true;

            RecentFiles.MruList.PinUnpinEntry(!cmdParam.IsPinned, cmdParam);
        }

        private void AddMRUEntry_Executed(object o, ExecutedRoutedEventArgs e)
        {
            MRUEntryVM cmdParam = o as MRUEntryVM;

            if (cmdParam == null)
                return;

            if (e != null)
                e.Handled = true;

            RecentFiles.MruList.AddMRUEntry(cmdParam);
        }

        private void RemoveMRUEntry_Executed(object o, ExecutedRoutedEventArgs e)
        {
            MRUEntryVM cmdParam = o as MRUEntryVM;

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
            (s, e) =>
            {
                OnOpen();
            }));

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


    }
}
