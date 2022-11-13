using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace XmlStripper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private const string FIELD_REGEX = "private [^;]+;\\s*";
        private const string PROP_REGEX = @"\s*{\s*get[^}]*}\s*set[^}]*}[^}]*}";
        private void ConvertText(object sender, TextChangedEventArgs e)
        {
            var text = Source.Text;

            text=Regex.Replace(text, FIELD_REGEX, string.Empty);


            text = Regex.Replace(text, PROP_REGEX, " {get;set;}");
            Result.Text = text;
        }
    }
}
