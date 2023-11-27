using Clothings_Store.Data;
using Clothings_Store.Models.Database;

namespace Clothings_Store.Models.Others
{
    public class OrderInfoSession
    {
        public string Id { get; set; } = DateTime.UtcNow.Ticks.ToString();
        public string Address { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public DateTime DeliveryTime { get; set; } = DateTime.UtcNow.AddDays(3);
        public double Amount { get; set; } = 0;
        public string DiscountCode { get; set; } = "";
        public int PaymentId { get; set; }
        public string Note { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
