using System;
using System.Collections.Generic;

namespace Clothings_Store.Models.Database;

public partial class Size
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Ghichu { get; set; } = null!;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
