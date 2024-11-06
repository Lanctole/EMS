using EMS.Core.Models;

namespace EMS.Core.Interfaces;

public interface INetworkService
{
    Task SendCommandAsync(NetworkCommand command);
    Task<NetworkCommand> ReceiveCommandAsync();
}