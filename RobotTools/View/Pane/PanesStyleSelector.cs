using System.Windows;
using System.Windows.Controls;
using RobotTools.UI.ViewModels.Base;
using RobotTools.ViewModels; 
namespace RobotTools.View.Pane;

class PanesStyleSelector : StyleSelector
{
public Style ToolStyle
{
  get;
  set;
}

public Style FileStyle
{
  get;
  set;
}

public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
{
        switch (item)
        {
            case ToolViewModel _:return ToolStyle;
                case FileViewModel _:return FileStyle;
            default: return base.SelectStyle(item, container);
        }
  
}
}
