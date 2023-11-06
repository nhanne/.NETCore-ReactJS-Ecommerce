using Clothings_Store.Data;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Models;

public class Cart
{
    private readonly StoreContext _db;
    public int IdCart { set; get; }
    public int quantity { set; get; }
    public string cateName { set; get; }
    public string images { set; get; }
    public string name { set; get; }
    public double unitPrice { set; get; }
    public string size { set; get; }
    public string color { set; get; }

    public double totalPrice
    {
        get { return quantity * unitPrice; }
    }
    public Cart(StoreContext context,int stockId)
    {
        _db = context;
        if(_db != null)
        {
            Stock? stock = _db.Stocks
                .Include(p => p.Product)
                .Include(p => p.Product.Category)
                .Include(p => p.Color)
                .Include(p => p.Size)
                .FirstOrDefault(p => p.Id == stockId);
            IdCart = stockId;
            quantity = 1;
            cateName = stock.Product.Category.Name;
            images = stock.Product.Picture;
            name = stock.Product.Name;
            unitPrice = stock.Product.UnitPrice;
            size = stock.Size.Name;
            color = stock.Color.Name;
        }
    }
}