using System;
using System.Collections.Generic;

namespace Clothings_Store.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Ghichu { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
