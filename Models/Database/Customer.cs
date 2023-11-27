using System;
using System.Collections.Generic;

namespace Clothings_Store.Models.Database;

public partial class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
