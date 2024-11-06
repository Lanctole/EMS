using EMS.Core.Models;

namespace EMS.QueueDisplay.Services;

public class QueueService
{
    private readonly int _maxQueueSize;
    private readonly LinkedList<Product> _queue;

    public QueueService(int maxQueueSize = 5)
    {
        _maxQueueSize = maxQueueSize;
        _queue = new LinkedList<Product>();
    }

    public bool AddProduct(Product product)
    {
        if (_queue.Count >= _maxQueueSize) return false;
        _queue.AddFirst(product);
        return true;
    }

    public bool RemoveProduct()
    {
        if (_queue.Count == 0) return false;
        _queue.RemoveLast();
        return true;
    }

    public IReadOnlyCollection<Product> GetQueue()
    {
        return _queue;
    }
}