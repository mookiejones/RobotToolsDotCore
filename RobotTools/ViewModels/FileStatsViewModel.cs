using System;
using System.IO;
using System.Windows.Media.Imaging;

using CommunityToolkit.Mvvm.DependencyInjection;

namespace RobotTools.ViewModels
{
    class FileStatsViewModel : Base.ToolViewModel
  {

        private WorkspaceViewModel Workspace => Ioc.Default.GetRequiredService<WorkspaceViewModel>();
        public FileStatsViewModel()
      : base("File Stats")
    {


            Workspace.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
      ContentId = ToolContentId;

      BitmapImage bi = new BitmapImage();
      bi.BeginInit();
      bi.UriSource = new Uri("pack://application:,,,/RobotTools;component/Images/property-blue.png");
      bi.EndInit();
      IconSource = bi;
    }

    public const string ToolContentId = "FileStatsTool";

    void OnActiveDocumentChanged(object sender, EventArgs e)
    {
      if (Workspace.ActiveDocument != null &&
         Workspace.ActiveDocument.FilePath != null &&
          File.Exists(Workspace.ActiveDocument.FilePath))
      {
        var fi = new FileInfo(Workspace.ActiveDocument.FilePath);
        FileSize = fi.Length;
        LastModified = fi.LastWriteTime;
      }
      else
      {
        FileSize = 0;
        LastModified = DateTime.MinValue;
      }
    }

    #region FileSize

    private long _fileSize;
    public long FileSize
    {
      get { return _fileSize; }
      set
      {
        if (_fileSize != value)
        {
          _fileSize = value;
                    OnPropertyChanged("FileSize");
        }
      }
    }

    #endregion

    #region LastModified

    private DateTime _lastModified;
    public DateTime LastModified
    {
      get { return _lastModified; }
      set
      {
        if (_lastModified != value)
        {
          _lastModified = value;
                    OnPropertyChanged("LastModified");
        }
      }
    }

    #endregion




  }
}
