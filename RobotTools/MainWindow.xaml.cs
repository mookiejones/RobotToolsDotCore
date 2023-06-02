
using CommunityToolkit.Mvvm.DependencyInjection;
using RobotTools.UI.Editor;
using RobotTools.ViewModels;

namespace RobotTools;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow  
{
     public MainWindow()
    {
        InitializeComponent();

        var dataContext = Ioc.Default.GetRequiredService<WorkspaceViewModel>();
        DataContext = dataContext;

       dataContext.InitCommandBinding(this);
    }

    private void WindowLoaded(object sender, RoutedEventArgs e) => EditorCommands.Register(this);
}
