using System;
using System.Collections.Generic;

namespace Clothings_Store.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Avatar { get; set; }

    public string? Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Cmt { get; set; }

    public string? Phone { get; set; }

    public int? JobTitle { get; set; }

    public virtual JobTitle? JobTitleNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
