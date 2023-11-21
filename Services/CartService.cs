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
        public CartService(IHttpContextAccessor httpContextAccessor, ILogger<CartService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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
            catch(Exception ex)
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
    }
}
