using EMS.Core.Enums;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using Moq;

namespace EMS.Tests;

public class NetworkServiceTests
{
    [Fact]
    public async Task SendCommandAsync_ShouldSendCommand_WhenConnected()
    {
        var networkServiceMock = new Mock<INetworkService>();
        var command = new NetworkCommand { Action = QueueActions.Add, Product = new Product { Status = ProductStatus.Valid, Color = "Green" } };

        await networkServiceMock.Object.SendCommandAsync(command);

        networkServiceMock.Verify(ns => ns.SendCommandAsync(It.Is<NetworkCommand>(cmd =>
            cmd.Action == QueueActions.Add && cmd.Product.Color == "Green")), Times.Once);
    }

    [Fact]
    public async Task ReceiveCommandAsync_ShouldReceiveCommand()
    {
        var networkServiceMock = new Mock<INetworkService>();
        var command = new NetworkCommand { Action = QueueActions.Add, Product = new Product { Status = ProductStatus.Valid, Color = "Green" } };
        networkServiceMock.Setup(ns => ns.ReceiveCommandAsync()).ReturnsAsync(command);

        var result = await networkServiceMock.Object.ReceiveCommandAsync();

        Assert.Equal(QueueActions.Add, result.Action);
        Assert.Equal("Green", result.Product.Color);
    }
}