using Clothings_Store.Models.Database;

namespace Clothings_Store.Models.Others
{
    public partial class OrderDetailFlutter
    {
        public string OrderFlutterId { get; set; } 
        public int StockId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public virtual OrderFlutter? OrderFlutter { get; set; }
        public virtual Stock? Stock { get; set; } 
    }
}
