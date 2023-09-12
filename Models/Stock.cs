using System;
using System.Collections.Generic;

namespace Clothings_Store.Models;

public partial class Stock
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? ColorId { get; set; }

    public int? SizeId { get; set; }

    public int? Stock1 { get; set; }

    public DateTime? StockInDate { get; set; }

    public virtual Color? Color { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product? Product { get; set; }

    public virtual Size? Size { get; set; }
}
