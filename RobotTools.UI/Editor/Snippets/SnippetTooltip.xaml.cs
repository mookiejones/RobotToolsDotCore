using System.Windows.Controls;

namespace RobotTools.UI.Editor.Snippets
{
    /// <summary>
    /// Interaction logic for SnippetTooltip.xaml
    /// </summary>
    public partial class SnippetToolTip : UserControl
    {

        public string Author { get; set; }
        public string Description { get; set; }
        public string Shortcuts { get; set; }
        public string Title { get; set; }

        public SnippetToolTip(SnippetInfo snippetInfo)
        {
            InitializeComponent();
            DataContext = this;
        }
        public SnippetToolTip()
        {
            InitializeComponent();
        }
    }
}
