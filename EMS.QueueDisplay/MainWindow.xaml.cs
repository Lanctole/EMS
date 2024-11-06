using System.Windows;
using EMS.QueueDisplay.Controllers;
using EMS.QueueDisplay.Services;
using EMS.QueueDisplay.ViewModels;

namespace EMS.QueueDisplay;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly NetworkService _networkService;
    private readonly QueueController _queueController;
    private readonly QueueService _queueService;
    private readonly QueueViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        _networkService = new NetworkService();
        _queueService = new QueueService();
        _queueController = new QueueController(_queueService);
        _networkService.OnCommandReceived += _queueController.HandleCommand;

        _viewModel = new QueueViewModel(_queueController);
        DataContext = _viewModel;

        StartListening();
    }

    private async void StartListening()
    {
        await _networkService.StartListeningAsync();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _networkService.Dispose();
    }
}