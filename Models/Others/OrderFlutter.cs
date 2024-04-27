using Clothings_Store.Models.Consts;

namespace Clothings_Store.Models.Others
{
    public partial class OrderFlutter
    {
        public string Id { set; get; } = DateTime.UtcNow.Ticks.ToString();
        public string Name { set; get; } = string.Empty;
        public string PhoneNumber { set; get; } = string.Empty;
        public string Address { set; get; } = string.Empty;
        public string FlutterAccountId { set; get; } = string.Empty;
        public DateTime OrderTime { set; get; } = DateTime.Now;
        public OrderStatusConst OrderStatus { set; get; } = OrderStatusConst.Waiting;
    }
}
