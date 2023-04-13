using CommunityToolkit.Mvvm.ComponentModel;

namespace RobotTools.ViewModels.Base
{
    partial class ToolViewModel : PaneViewModel
    {
        public ToolViewModel(string name)
        {
            Name = name;
            Title = name;
        }

        public string Name
        {
            get;
            private set;
        }

        [ObservableProperty]
        private bool isVisible = true;



    }
}
