using Clothings_Store.Data;
namespace Clothings_Store.Models;

public class Cart
{
    private readonly StoreContext _db;
    public int IdCart { set; get; }
    public double unitPrice { set; get; }
    public int quantity { set; get; }
    public string images { set; get; }
    public string size { set; get; }
    public string color { set; get; }
    public virtual Stock Stock { set; get; }

    public double totalPrice
    {
        get { return quantity * unitPrice; }
    }

    public Cart(StoreContext context,int stockId)
    {
        _db = context;
        if(_db != null)
        {
            Stock stock = _db.Stocks.Find(stockId);
            IdCart = stockId;
            quantity = 1;
            images = _db.Products.Find(stock.ProductId).Picture;
            unitPrice = _db.Products.Find(stock.ProductId).UnitPrice;
            size = _db.Sizes.Find(stock.SizeId).Name;
            color = _db.Colors.Find(stock.ColorId).Name;
        }
    }
}