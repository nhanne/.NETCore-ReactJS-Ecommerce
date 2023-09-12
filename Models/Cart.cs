using Clothings_Store.Data;
using System;
using System.Collections.Generic;
namespace Clothings_Store.Models;

public partial class Cart
{
    private readonly StoreContext _db;
    public int IdProduct { set; get; }
    public int IdColor { set; get; }
    public int IdSize { set; get; }
    public int IdStock { set; get; }

    public string ProductName { set; get; }
    public string Picture { set; get; }
    public float unitPrice { set; get; }
    public int Quantity { set; get; }
    public string Brand { set; get; }
    public string Color { set; get; }
    public string Size { set; get; }
    public float totalPrice
    {
        get { return Quantity * unitPrice; }
    }
    public Cart(int MaSP, int MaMau, int MaSize)
    {
        IdProduct = MaSP;
        IdColor = MaMau;
        IdSize = MaSize;
        Stock model = _db.Stocks.Single(p => p.ProductId == IdProduct && p.ColorId == IdColor && p.SizeId == IdSize);
        ProductName = model.Product.Name;
        Picture = model.Product.Picture;
        unitPrice = float.Parse(model.Product.UnitPrice.ToString());
        Quantity = 1;
        Brand = model.Product.Category.Name;
        Color = model.Color.Name;
        Size = model.Size.Name;
        IdStock = model.Id;
    }
}