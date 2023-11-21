using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly ILogger<CartController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        public CartController(
            StoreContext context,
            ILogger<CartController> logger,
            IOrderService orderService,
            ICartService cartService)
        {
            _db = context;
            _logger = logger;
            _orderService = orderService;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            ViewBag.TotalPrice = _cartService.TotalPrice();
            ViewBag.TotalItems = _cartService.TotalItems();
            return View(_cartService.GetCart());
        }

        [HttpPost]
        public JsonResult AddToCart(int productId, int colorId, int sizeId)
        {
            try
            {
                var stock = _db.Stocks.Where(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId).FirstOrDefault();
                if (stock == null) return Json(false);

                var listCart = _cartService.GetCart();
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
                _cartService.SaveCartSession(listCart);
                _logger.LogInformation("Add product to cart success.");
                return Json(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when add product to cart.");
                throw;
            }
        }
        public IActionResult RemoveFromCart(int IdCart)
        {
            var listCart = _cartService.GetCart();
            Cart item = listCart.SingleOrDefault(c => c.IdCart == IdCart);
            if (item != null)
            {
                listCart.RemoveAll(c => c.IdCart == IdCart);
                _cartService.SaveCartSession(listCart);
            }
            if (listCart.Count == 0)
            {
                return RedirectToAction("Store", "Home");
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateCart(int IdCart, IFormCollection form)
        {
            List<Cart> listCart = _cartService.GetCart();
            Cart item = listCart.SingleOrDefault(c => c.IdCart == IdCart);
            if (item != null)
            {
                item.quantity = int.Parse(form["quantity"].ToString());
            }
            _cartService.SaveCartSession(listCart);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            List<Cart> listCart = _cartService.GetCart();
            if (listCart.Count == 0)
            {
                return RedirectToAction("Store", "Home");
            }
            return View(listCart);
        }
        [HttpPost]
        public IActionResult CheckOut(AppUser userModel, Order orderModel, string code)
        {
            try
            {
                Order order = new Order();
                _orderService.OrderCustomer(order, userModel);
                _orderService.OrderInfo(order, orderModel, code, _cartService.TotalPrice(), _cartService.TotalItems());
                _db.Orders.Add(order);
                _db.SaveChanges();
                _orderService.OrderDetail(order.Id, _cartService.GetCart());
                _cartService.ClearCart();
                return Json(new { redirectToUrl = Url.Action("Index", new { Id = order.Id }) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check out.");
                throw;
            }
        }
    }
}
