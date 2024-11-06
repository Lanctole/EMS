using EMS.QueueDisplay.Services;
using EMS.Core.Models;
using EMS.Core.Enums;

namespace EMS.Tests;

public class QueueServiceTests
{
    [Fact]
    public void AddProduct_ShouldAddProductToQueue_WhenQueueNotFull()
    {
        var queueService = new QueueService();
        var product = new Product { Status = ProductStatus.Valid, Color = "Green" };

        bool result = queueService.AddProduct(product);

        Assert.True(result);
        Assert.Contains(product, queueService.GetQueue());
    }

    [Fact]
    public void AddProduct_ShouldNotAddProduct_WhenQueueIsFull()
    {
        var queueService = new QueueService(5);
        for (int i = 0; i < 5; i++)
        {
            queueService.AddProduct(new Product { Status = ProductStatus.Valid, Color = "Green" });
        }

        bool result = queueService.AddProduct(new Product { Status = ProductStatus.Invalid, Color = "Yellow" });

        Assert.False(result);
        Assert.Equal(5, queueService.GetQueue().Count);
    }

    [Fact]
    public void RemoveProduct_ShouldRemoveLastProductFromQueue_WhenQueueNotEmpty()
    {
        var queueService = new QueueService();
        var product = new Product { Status = ProductStatus.Valid, Color = "Green" };
        queueService.AddProduct(product);

        bool result = queueService.RemoveProduct();

        Assert.True(result);
        Assert.DoesNotContain(product, queueService.GetQueue());
    }

    [Fact]
    public void RemoveProduct_ShouldReturnFalse_WhenQueueIsEmpty()
    {
        var queueService = new QueueService();

        bool result = queueService.RemoveProduct();

        Assert.False(result);
    }
}