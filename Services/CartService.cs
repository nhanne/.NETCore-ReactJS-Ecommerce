using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;
using Clothings_Store.Patterns;
using Newtonsoft.Json;

namespace Clothings_Store.Services
{
    public class CartService : ICartService
    {
        private readonly ILogger<CartService> _logger;
        private readonly ICustomSessionService<Cart> _session;
        private readonly StoreContext _db;
        private const string CARTKEY = "cart";
        public CartService(
                         ILogger<CartService> logger,
                         ICustomSessionService<Cart> session,
                         StoreContext db)
        {
            _db = db;
            _logger = logger;
            _session = session;
        }
        public void AddToCart(Stock stock)
        {
            try
            {
                var listCart = GetCart();
                var cartitem = listCart.Find(p => p.IdCart == stock.Id);
                if (cartitem != null)
                {
                    cartitem.quantity++;
                }
                else
                {
                    cartitem = new Cart(_db, stock.Id);
                    listCart.Add(cartitem);
                }
                SaveCartSession(listCart);
                _logger.LogInformation("Add to cart success.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add to cart failed.");
                throw;
            }

        }
        public int RemoveFromCart(int IdCart)
        {
            try
            {
                var listCart = GetCart();
                var item = listCart.SingleOrDefault(c => c.IdCart == IdCart);
                if (item != null)
                {
                    listCart.RemoveAll(c => c.IdCart == IdCart);
                    SaveCartSession(listCart);
                }
                _logger.LogInformation("Remove from cart success.");
                if (listCart.Count == 0)
                {
                    return 0;
                }
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Remove from cart failed.");
                throw;
            }
            return 1;
        }
        public int TotalItems()
        {
            int iTotalItems = 0;
            var listCart = GetCart();
            if (listCart != null)
            {
                iTotalItems = listCart.Sum(n => n.quantity);
            }
            return iTotalItems;
        }
        public double TotalPrice()
        {
            double iTotalPrice = 0;
            var listCart = GetCart();
            if (listCart != null)
            {
                iTotalPrice = listCart.Sum(n => n.totalPrice);
            }
            return iTotalPrice;
        }

        public void UpdateCart(int IdCart, int Quantity)
        {
            List<Cart> listCart = GetCart();
            var item = listCart.SingleOrDefault(c => c.IdCart == IdCart);
            if (item != null)
            {
                item.quantity = Quantity;
            }
            SaveCartSession(listCart);
        }

        public List<Cart> GetCart()
        {
            return _session.GetSession(CARTKEY);
        }

        public void ClearCart()
        {
            _session.ClearSession(CARTKEY);
        }

        public void SaveCartSession(List<Cart> listCart)
        {
            _session.SaveSession(listCart);
        }
    }
}
