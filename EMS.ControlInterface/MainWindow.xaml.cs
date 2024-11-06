using System.Windows;
using EMS.ControlInterface.Services;
using EMS.ControlInterface.ViewModels;

namespace EMS.ControlInterface;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly NetworkService _networkService;
    private readonly ControlViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _networkService = new NetworkService();
        _viewModel = new ControlViewModel(_networkService);
        DataContext = _viewModel;
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _networkService.Dispose();
    }
}