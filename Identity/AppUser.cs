using Clothings_Store.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clothings_Store.Identity
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
        [PersonalData]
        public string? Address { get; set; }
        [PersonalData]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
       
    }
}
