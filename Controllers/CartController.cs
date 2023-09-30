using Clothings_Store.Data;
using Clothings_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Clothings_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly ILogger<CartController> _logger;
        public CartController(StoreContext context, ILogger<CartController> logger)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";
        // Lấy cart từ Session (danh sách CartItem)
        public List<Cart> GetCart()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (!string.IsNullOrEmpty(jsoncart))
            {
                return JsonConvert.DeserializeObject<List<Cart>>(jsoncart);
            }

            return new List<Cart>();
        }
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }
        void SaveCartSession(List<Cart> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int colorId, int sizeId)
        {
            var stock = _db.Stocks.Where(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId).FirstOrDefault();
            if (stock == null) return Json(new { messege = "sản phẩm hiện đang hết hàng" });
            var cart = GetCart();
            var cartitem = cart.Find(p => p.IdCart == stock.Id);
            if (cartitem != null)
            {
                cartitem.quantity++;
            }
            else
            {
                cartitem = new Cart(_db,stock.Id);
                cart.Add(cartitem);
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            return Json(new { messege = "Đã thêm sản phẩm vào giỏ hàng" });
        }
        public IActionResult CheckOut()
        {
            // Xử lý khi đặt hàng
            return View();
        }
    }
}
