using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Newtonsoft.Json;

namespace Clothings_Store.Services
{
    public class CartService : ICartService
    {
        public const string CARTKEY = "cart";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartService> _logger;
        private readonly StoreContext _db;
        public CartService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<CartService> logger,
            StoreContext db)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _db = db;
        }
        public void AddToCart(Stock stock)
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
        }
        public void ClearCart()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.Session != null)
                {
                    httpContext.Session.Remove(CARTKEY);
                }
                _logger.LogInformation("Xóa giỏ hàng thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Thất bại khi xóa giỏ hàng.");
                throw;
            }

        }
        public List<Cart> GetCart()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var session = httpContext?.Session;
            string jsoncart = session?.GetString(CARTKEY) ?? string.Empty;
            if (!string.IsNullOrEmpty(jsoncart))
            {
                return JsonConvert.DeserializeObject<List<Cart>>(jsoncart)!;
            }
            return new List<Cart>();
        }

        public int RemoveFromCart(int IdCart)
        {
            var listCart = GetCart();
            var item = listCart.SingleOrDefault(c => c.IdCart == IdCart);
            if (item != null)
            {
                listCart.RemoveAll(c => c.IdCart == IdCart);
                SaveCartSession(listCart);
            }
            if (listCart.Count == 0)
            {
                return 0;
            }
            return 1;
        }

        public void SaveCartSession(List<Cart> listCart)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                var session = httpContext.Session;
                string jsoncart = JsonConvert.SerializeObject(listCart);
                session.SetString(CARTKEY, jsoncart);
            }
        }
        public int TotalItems()
        {
            int iTotalItems = 0;
            var listCart = GetCart(); ;
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
    }
}
