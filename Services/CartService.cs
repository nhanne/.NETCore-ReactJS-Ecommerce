using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Newtonsoft.Json;

namespace Clothings_Store.Services
{
    public class CartService : ICartService
    {
        public const string CARTKEY = "cart";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove(CARTKEY);
        }

        public List<Cart> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (!string.IsNullOrEmpty(jsoncart))
            {
                return JsonConvert.DeserializeObject<List<Cart>>(jsoncart);
            }
            return new List<Cart>();
        }

        public void SaveCartSession(List<Cart> listCart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(listCart);
            session.SetString(CARTKEY, jsoncart);
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
    }
}
