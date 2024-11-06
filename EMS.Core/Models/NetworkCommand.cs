using EMS.Core.Enums;

namespace EMS.Core.Models;

public class NetworkCommand
{
    public QueueActions Action { get; set; }
    public Product Product { get; set; }
}