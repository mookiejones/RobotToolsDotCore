using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RobotTools.UI.AngleConverter;

/// <summary>
/// Interaction logic for ValueBox.xaml
/// </summary>
public partial class ValueBox : UserControl
{



    public string Header
    {
        get { return (string)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(ValueBox), new PropertyMetadata(string.Empty,HeaderChangedPropertyCallback));

    private static void HeaderChangedPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is ValueBox control))return;


    }

    public ValueBox()
    {
        InitializeComponent();
    }
}
