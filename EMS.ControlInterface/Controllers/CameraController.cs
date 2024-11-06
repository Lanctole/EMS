using EMS.Core.Enums;
using EMS.Core.Interfaces;
using EMS.Core.Models;

namespace EMS.ControlInterface.Controllers;

public class CameraController
{
    private readonly INetworkService _networkService;

    public CameraController(INetworkService networkService)
    {
        _networkService = networkService;
    }

    public async Task AddProductAsync(ProductStatus status)
    {
        var product = new Product
        {
            Status = status,
            Color = status == ProductStatus.Valid ? "Green" : "Yellow"
        };

        var command = new NetworkCommand
        {
            Action = QueueActions.Add,
            Product = product
        };

        await _networkService.SendCommandAsync(command);
    }
}