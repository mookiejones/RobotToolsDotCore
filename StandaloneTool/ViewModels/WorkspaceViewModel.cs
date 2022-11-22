using System;
using System.Collections.Generic;
using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;

using StandaloneTool.UI;

namespace StandaloneTool.ViewModels
{
    public class WorkspaceViewModel : ObservableObject,IFilesDropped
    {


        #region Title
        private string _title="TTTTGGGG";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion


        public void OnFilesDropped(string[] files)
        {
            throw new NotImplementedException();
        }
    }
}
