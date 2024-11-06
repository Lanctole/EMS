using EMS.Core.Enums;

namespace EMS.Core.Models;

public class Product
{
    public ProductStatus Status { get; set; }
    public string Color { get; set; }
}