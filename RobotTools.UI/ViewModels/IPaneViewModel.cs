using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RobotTools.UI.ViewModels
{
    public interface IPaneViewModel:INotifyPropertyChanged,INotifyPropertyChanging
    {
        string Title { get; set; }

        ImageSource IconSource { get;}

        string ContentId { get; set; }

        bool IsSelected { get; set; }

        bool IsActive { get; set; }
    }

    public interface IToolViewModel : IPaneViewModel
    {
        string Name { get; }

        bool IsVisible { get; set; }
    }
}
