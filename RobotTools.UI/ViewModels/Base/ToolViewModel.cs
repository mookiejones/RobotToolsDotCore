using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RobotTools.UI.ViewModels.Base;

public partial class ToolViewModel : PaneViewModel, IToolViewModel
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
