using System;
using System.Collections.Generic;

namespace Clothings_Store.Models;

public partial class Order
{
    public string Id { get; set; }

    public int? CustomerId { get; set; }
    public string? UserId { get; set; }

    public string? Status { get; set; }

    public string? Address { get; set; }

    public int PaymentId { get; set; }

    public DateTime OrdTime { get; set; }

    public DateTime DeliTime { get; set; }

    public double TotalPrice { get; set; }

    public int TotalQuantity { get; set; }

    public string? Note { get; set; }
    public string? PromoCode { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual AppUser? User { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual Payment? Payment { get; set; } 
    public virtual OrderStatus? StatusNavigation { get; set; } 


}
