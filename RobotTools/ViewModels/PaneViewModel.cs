using System;
using System.Linq.Expressions;
using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;

namespace RobotTools.ViewModels
{
    partial class PaneViewModel : ObservableRecipient
  {
    public PaneViewModel()
    { }

        [ObservableProperty]
    protected string title;
  

    public ImageSource IconSource
    {
      get;
      protected set;
    }
        [ObservableProperty]
        private string contentId = null;

        [ObservableProperty]
        private bool isSelected = false;

        [ObservableProperty]
        private bool isActive;

         
    }
}
