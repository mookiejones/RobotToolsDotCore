using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using ICSharpCode.AvalonEdit.Document;

using Microsoft.Win32;

using RobotTools.UI.Extension;

namespace RobotTools.ViewModels;

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

    private bool CanNew() => true;

    private void OnNew()
    {
        Files.Add(new FileViewModel() { Document = new TextDocument() });
        ActiveDocument = Files.Last();
    }

    #endregion




    #region OpenCommand
    

    

    [RelayCommand()]
    private void Open()
    {
        var dlg = new OpenFileDialog();
        if (dlg.ShowDialog().GetValueOrDefault())
        {
           Open(dlg.FileName);
            
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
    [RelayCommand]
    private void NewFile()
    {
        var fileViewModel = new FileViewModel();
        Files.Add(fileViewModel);
        ActiveDocument = fileViewModel;
    }
    #region NewFileCommand
 
    public void OnFileDrop(string[] filepaths) => throw new NotImplementedException();

    public void OnFilesDropped(string[] files) => throw new NotImplementedException();
    #endregion


}
