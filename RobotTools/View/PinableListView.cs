using System.Windows;
using System.Windows.Controls;

namespace RobotTools.View
{
    public class PinableListView : ListView
    {
        // Getting CustomControl style from Themes/Generic.xaml does not work ???
        static PinableListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PinableListView),
                      new FrameworkPropertyMetadata(typeof(PinableListView)));
        }

        protected override DependencyObject GetContainerForItemOverride() => new PinableListViewItem();

        protected override bool IsItemItsOwnContainerOverride(object item) => item is PinableListViewItem;

        public override void OnApplyTemplate() => base.OnApplyTemplate();
    }
}
