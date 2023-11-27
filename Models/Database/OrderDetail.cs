using System;
using System.Collections.Generic;

namespace Clothings_Store.Models.Database;

public partial class OrderDetail
{
    public string OrderId { get; set; }

    public int StockId { get; set; }

    public int? Quantity { get; set; }

    public double? UnitPrice { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Stock Stock { get; set; } = null!;
}
