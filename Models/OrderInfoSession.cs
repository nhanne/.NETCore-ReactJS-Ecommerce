namespace Clothings_Store.Models
{
    public class OrderInfoSession
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public DateTime Time { get; set; }
        public DateTime DeliveryTime { get; set; }
        public double Amount { get; set; }
        public string DiscountCode { get; set; }
        public int PaymentId { get; set; }
        public string Note { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
