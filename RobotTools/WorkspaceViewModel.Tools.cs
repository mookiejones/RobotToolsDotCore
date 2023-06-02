
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using RobotTools.UI.DirectorySearcher;

namespace RobotTools.ViewModels;

partial class WorkspaceViewModel
{

    private RelayCommand _showDirectorySearcherCommand;
    public RelayCommand ShowDirectorySearcherCommand => _showDirectorySearcherCommand ?? (_showDirectorySearcherCommand = new RelayCommand(ShowDirectorySearcher));



    private void ShowDirectorySearcher()
    {

        var dialog = new CustomDialog() { Title = "Directory Searcher" };

        var window = new MetroWindow { Title = "Directory Searcher" };

        var control = new DirectorySearcherControl(instance =>
       {
           _dialogCoordinator.HideMetroDialogAsync(this, dialog);
       });


        window.Content = control;

        window.Owner = App.Current.MainWindow;
        window.ShowDialog();


      
       
    }
}
