using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using EMS.ControlInterface.Commands;
using EMS.ControlInterface.Controllers;
using EMS.ControlInterface.Services;
using EMS.Core.Enums;

namespace EMS.ControlInterface.ViewModels;

public class ControlViewModel : INotifyPropertyChanged
{
    private readonly CameraController _cameraController;
    private readonly PusherController _pusherController;
    private bool _isValid;

    public ControlViewModel(NetworkService networkService)
    {
        _cameraController = new CameraController(networkService);
        _pusherController = new PusherController(networkService);
        AddCameraCommand = new RelayCommand(async _ => await AddCameraAsync());
        RemovePusherCommand = new RelayCommand(async _ => await RemovePusherAsync());

        networkService.OnConnectionError += message => { ShowErrorMessage(message); };
    }

    public bool IsValid
    {
        get => _isValid;
        set
        {
            if (_isValid != value)
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }

    public ICommand AddCameraCommand { get; }
    public ICommand RemovePusherCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    private async Task AddCameraAsync()
    {
        await _cameraController.AddProductAsync(IsValid ? ProductStatus.Valid : ProductStatus.Invalid);
    }

    private async Task RemovePusherAsync()
    {
        await _pusherController.RemoveProductAsync();
    }

    private void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}