using EMS.Core.Enums;
using EMS.Core.Interfaces;
using EMS.Core.Models;

namespace EMS.ControlInterface.Controllers;

public class PusherController
{
    private readonly INetworkService _networkService;

    public PusherController(INetworkService networkService)
    {
        _networkService = networkService;
    }

    public async Task RemoveProductAsync()
    {
        var command = new NetworkCommand
        {
            Action = QueueActions.Remove,
            Product = null
        };

        await _networkService.SendCommandAsync(command);
    }
}