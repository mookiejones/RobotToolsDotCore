using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using RobotTools.Controls.MRU;

namespace RobotTools.ViewModels
{
    partial class RecentFilesViewModel : Base.ToolViewModel
    {
       
        #region fields
        static ImageSourceConverter ISC = new ImageSourceConverter();
        #endregion fields
        public const string ToolContentId = "RecentFilesTool";

        public RecentFilesViewModel()
          : base("Recent Files")
        {
            ////Workspace.This.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            ContentId = ToolContentId;

            mruList = new MRUListVM();
            //IconSource= ISC.ConvertFromInvariantString(@"pack://application:,,,/RobotTools;component/Controls/MRU/images/NoPin16.png") as ImageSource;

            //new Uri("pack://application:,,,/RobotTools;component/ViewModels/MRU/Images/NoPin16.png", UriKind.RelativeOrAbsolute);
        }



        [ObservableProperty]
        private MRUListVM mruList;
        

        public void AddNewEntryIntoMRU(string filePath)
        {
            if (MruList.FindMRUEntry(filePath) == null)
            {
                MRUEntryVM e = new MRUEntryVM() { IsPinned = false, PathFileName = filePath };

                MruList.AddMRUEntry(e);
                OnPropertyChanged(nameof(MruList));

            }
        }
    }
}
