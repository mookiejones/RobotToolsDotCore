using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using ICSharpCode.AvalonEdit.Document;

using Microsoft.Win32;

using RobotTools.UI.Extension;

namespace RobotTools.ViewModels
{
    partial class WorkspaceViewModel : IFilesDropped
    {
        #region NewCommand
        RelayCommand _newCommand = null;
        public ICommand NewCommand
        {
            get
            {
                if (_newCommand == null)
                {
                    _newCommand = new RelayCommand(OnNew, CanNew);
                }

                return _newCommand;
            }
        }

        private bool CanNew()
        {
            return true;
        }

        private void OnNew()
        {
            Files.Add(new FileViewModel() { Document = new TextDocument() });
            ActiveDocument = Files.Last();
        }

        #endregion

        #region OpenCommand
        RelayCommand _openCommand = null;
        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand(OnOpen, CanOpen);
                }

                return _openCommand;
            }
        }

        private bool CanOpen()
        {
            return true;
        }

        private void OnOpen()
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog().GetValueOrDefault())
            {
                var fileViewModel = Open(dlg.FileName);
                ActiveDocument = fileViewModel;
            }
        }

        /// <summary>
        /// Open File
        /// </summary>
        /// <param name="filepath">Path to file</param>
        /// <returns><see cref="FileViewModel"/></returns>
        public FileViewModel Open(string filepath)
        {
            var fileViewModel = Files.FirstOrDefault(fm => fm.FilePath == filepath);
            if (fileViewModel != null)
                return fileViewModel;

            fileViewModel = new FileViewModel(filepath);
            Files.Add(fileViewModel);
            RecentFiles.AddNewEntryIntoMRU(filepath);
            ActiveDocument = fileViewModel;
            return fileViewModel;
        }

        #endregion

        #region NewFileCommand
        private RelayCommand _newFileCommand;
        public RelayCommand NewFileCommand => _newFileCommand ?? (_newFileCommand = new RelayCommand(ExecuteNewFile));

        private void ExecuteNewFile()
        {
            throw new NotImplementedException();
        }

        public void OnFileDrop(string[] filepaths)
        {
            throw new NotImplementedException();
        }

        public void OnFilesDropped(string[] files)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
