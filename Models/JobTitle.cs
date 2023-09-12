using System;
using System.Collections.Generic;

namespace Clothings_Store.Models;

public partial class JobTitle
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
