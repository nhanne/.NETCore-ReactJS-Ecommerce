using System;
using System.Collections.Generic;

namespace Clothings_Store.Models.Database;

public partial class OrderStatus
{
    public string Status { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
