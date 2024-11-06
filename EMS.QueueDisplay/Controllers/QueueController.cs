using EMS.Core.Enums;
using EMS.Core.Models;
using EMS.QueueDisplay.Services;

namespace EMS.QueueDisplay.Controllers;

public class QueueController
{
    private readonly QueueService _queueService;

    public QueueController(QueueService queueService)
    {
        _queueService = queueService;
    }

    public event Action<IReadOnlyCollection<Product>> OnQueueUpdated;

    public void HandleCommand(NetworkCommand command)
    {
        var changed = false;

        switch (command.Action)
        {
            case QueueActions.Add:
                if (command.Product != null) changed = _queueService.AddProduct(command.Product);
                break;
            case QueueActions.Remove:
                changed = _queueService.RemoveProduct();
                break;
            case QueueActions.Ignore:
                break;
        }

        if (changed) OnQueueUpdated?.Invoke(_queueService.GetQueue());
    }
}