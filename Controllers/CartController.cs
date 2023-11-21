using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        public CartController(
            StoreContext context,
            IOrderService orderService,
            ICartService cartService)
        {
            _db = context;
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
            var stock = _db.Stocks.Where(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId).FirstOrDefault();
            if (stock == null)
            {
                return Json(false);
            }
            _cartService.AddToCart(stock);
            return Json(true);
        }
        public IActionResult RemoveFromCart(int IdCart)
        {
            if (_cartService.RemoveFromCart(IdCart) == 0)
            {
                return RedirectToAction("Store", "Home");
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateCart(int IdCart, IFormCollection form)
        {
            int Quantity = int.Parse(form["quantity"].ToString());
            _cartService.UpdateCart(IdCart, Quantity);
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
            Order order = new Order();
            _orderService.OrderCustomer(order, userModel);
            _orderService.OrderInfo(order, orderModel, code, _cartService.TotalPrice(), _cartService.TotalItems());
            _db.Orders.Add(order);
            _db.SaveChanges();
            _orderService.OrderDetail(order.Id, _cartService.GetCart());
            _cartService.ClearCart();
            return Json(new { redirectToUrl = Url.Action("Index", new { Id = order.Id }) });
        }
    }
}
