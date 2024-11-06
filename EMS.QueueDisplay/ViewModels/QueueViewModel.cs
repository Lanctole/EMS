using System.Collections.ObjectModel;
using System.ComponentModel;
using EMS.Core.Models;
using EMS.QueueDisplay.Controllers;

namespace EMS.QueueDisplay.ViewModels;

public class QueueViewModel : INotifyPropertyChanged
{
    private readonly QueueController _queueController;

    public QueueViewModel(QueueController queueController)
    {
        _queueController = queueController;
        Products = new ObservableCollection<Product>();
        _queueController.OnQueueUpdated += UpdateQueue;
    }

    public ObservableCollection<Product> Products { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    private void UpdateQueue(IReadOnlyCollection<Product> queue)
    {
        Products.Clear();
        foreach (var product in queue) Products.Add(product);
    }
}