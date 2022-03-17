using System.Windows;
namespace LeaderPivot.XAML.WPF.Host;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
