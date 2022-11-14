using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using RobotTools.Core.Kop;

namespace RobotTools.UI.DirectorySearcher
{
    class DirectorySearcherViewModel:ObservableValidator
    {
        private  KopFiles _kopFiles = new KopFiles();
        public KopFiles Files
        {
            get => _kopFiles;
            set => SetProperty(ref _kopFiles, value);
        }

        private RelayCommand<string> _copyFileCommand;
        public RelayCommand<string> CopyFileCommand => _copyFileCommand ?? (_copyFileCommand = new RelayCommand<string>(ExecuteCopyFiles, CanCopyFiles));

        private bool CanCopyFiles(string arg)
        {
            if (string.IsNullOrEmpty(arg))
                return false;

            return Files.Count > 0;

        }

        private void ExecuteCopyFiles(string obj)
        {
            Files.CopyToDirectory(obj);

        }

        private string _resultData;
        public string ResultData { get => _resultData; set => SetProperty(ref _resultData, value); }
        private void GetFiles(string path)
        {
            var files = new KopFiles(path);
            Files = files;


            var sb = new StringBuilder();
            foreach (var file in files)
                sb.AppendLine(file.ToString());
            ResultData = sb.ToString();

            CopyFileCommand.NotifyCanExecuteChanged();
        }
        public void OnFilesDropped(string[] files)
        {
            Files.Clear();
            foreach (var file in files)
                GetFiles(file);


        }
    }
}
