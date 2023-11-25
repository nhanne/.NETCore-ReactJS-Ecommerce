using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Clothings_Store.Services;
using Clothings_Store.Services.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NuGet.Protocol;

namespace Clothings_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        public CartController(
            StoreContext context,
            ICartService cartService,
            IPaymentService paymentService
            )
        {
            _db = context;
            _cartService = cartService;
            _paymentService = paymentService;
        }
        public IActionResult Index()
        {
            List<Cart> listCart = _cartService.GetCart();
            if (listCart.Count == 0)
            {
                return RedirectToAction("Store", "Home");
            }
            ViewBag.TotalPrice = _cartService.TotalPrice();
            ViewBag.TotalItems = _cartService.TotalItems();
            return View(_cartService.GetCart());
        }
        [HttpPost]
        public async Task<JsonResult> AddToCart(int productId, int colorId, int sizeId)
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
        [HttpPost]
        public IActionResult CheckOut(OrderInfoSession orderInfoModel)
        {
            orderInfoModel.Id = DateTime.UtcNow.Ticks.ToString();
            switch (orderInfoModel.PaymentId)
            {
                case 1:
                    _paymentService.COD(orderInfoModel);
                    return Json(new { redirectToUrl = Url.Action("PaymentConfirm") });
                case 2:
                    string vnpaymentUrl = _paymentService.VNPay(orderInfoModel);
                    return Json(new { redirectToUrl = vnpaymentUrl });
                case 3:

                    return Json(new { redirectToUrl = Url.Action("PaymentConfirm") });
                default:
                    return View();
            }
        }
        public IActionResult PaymentConfirm()
        {

            return View();
        }
        public IActionResult VNPayConfirm()
        {
            if (!_paymentService.VNPayConfirm())
            {

            }
            return RedirectToAction("PaymentConfirm");
        }
      
    }
}
