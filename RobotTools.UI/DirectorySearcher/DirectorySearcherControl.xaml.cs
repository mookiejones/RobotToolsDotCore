using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using RobotTools.Core.Kop;
using RobotTools.UI.DirectorySearcher.Views;

namespace RobotTools.UI.DirectorySearcher
{
    /// <summary>
    /// Interaction logic for DirectorySearcherControl.xaml
    /// </summary>
    public partial class DirectorySearcherControl : UserControl,INotifyPropertyChanged,IFilesDropped
    {
        public DirectorySearcherControl(object p)
        {
            InitializeComponent();
            DataContext = this;
        }

        public DirectorySearcherControl(Action<DirectorySearcherControl> closeHandler)
        {
            InitializeComponent();
            DataContext = this;
            CloseCommand = new RelayCommand<object>(o => closeHandler(this));

        }

        public ICommand CloseCommand { get; }
        private KopFiles _kopFiles = new KopFiles();
        public KopFiles Files
        {
            get => _kopFiles;
            set
            {
                _kopFiles = value;
                RaisePropertyChanged(nameof(Files));
            }
        }


        private void RaisePropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
         

        public event PropertyChangedEventHandler PropertyChanged;

        public string ResultData
        {
            get => _resultData;
            set
            {
                _resultData = value;
                RaisePropertyChanged(nameof(ResultData));
            }

        }

        public ObservableCollection<TreeNode> Nodes { get; set; } = new ObservableCollection<TreeNode>();
        private void GetFiles(string path)
        {
            var files = new KopFiles(path);
            Files = files;


            var nodes = Files.GroupBy(o => o.Name, o => o)
                .Select(o => new TreeNode { Name = o.Key, KopFiles = o.ToList() })
                .OrderBy(o=>o.Name);

            Nodes.Clear();
            foreach (var node in nodes)
                Nodes.Add(node);


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

        public void OnFilesDropped(IEnumerable<string> files)
        {
            Files.Clear();
            foreach (var file in files)
                GetFiles(file);

        }
    }
}
