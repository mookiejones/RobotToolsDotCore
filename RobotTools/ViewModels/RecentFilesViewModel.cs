using System.Windows.Media;

using RobotTools.Controls.MRU;

namespace RobotTools.ViewModels
{
    class RecentFilesViewModel : Base.ToolViewModel
    {
        private MRUListVM mMruList;
        #region fields
        static ImageSourceConverter ISC = new ImageSourceConverter();
        #endregion fields
        public const string ToolContentId = "RecentFilesTool";

        public RecentFilesViewModel()
          : base("Recent Files")
        {
            ////Workspace.This.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            ContentId = ToolContentId;

            mMruList = new MRUListVM();
            //IconSource= ISC.ConvertFromInvariantString(@"pack://application:,,,/RobotTools;component/Controls/MRU/images/NoPin16.png") as ImageSource;

            //new Uri("pack://application:,,,/RobotTools;component/ViewModels/MRU/Images/NoPin16.png", UriKind.RelativeOrAbsolute);
        }

       

        public MRUListVM MruList
        {
            get
            {
                return mMruList;
            }

            private set
            {
                if (mMruList != value)
                {
                    mMruList = value;
                    NotifyPropertyChanged(() => MruList);
                }
            }
        }

        public void AddNewEntryIntoMRU(string filePath)
        {
            if (MruList.FindMRUEntry(filePath) == null)
            {
                MRUEntryVM e = new MRUEntryVM() { IsPinned = false, PathFileName = filePath };

                MruList.AddMRUEntry(e);

                NotifyPropertyChanged(() => MruList);
            }
        }
    }
}
