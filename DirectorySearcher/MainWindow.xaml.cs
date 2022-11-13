using CommunityToolkit.Mvvm.DependencyInjection;

namespace DirectorySearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow  
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<WorkspaceViewModel>();
        }
    }
}
