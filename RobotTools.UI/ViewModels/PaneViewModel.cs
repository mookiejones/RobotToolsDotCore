using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace RobotTools.UI.ViewModels;


public partial class PaneViewModel : ObservableRecipient, IPaneViewModel
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
