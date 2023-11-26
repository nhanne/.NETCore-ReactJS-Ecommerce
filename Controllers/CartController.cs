using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Clothings_Store.Services;
using Clothings_Store.Services.Others;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace Clothings_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        private readonly ICustomSessionService<string> _session;
        public CartController(
            StoreContext context,
            ICartService cartService,
            IPaymentService paymentService,
            ICustomSessionService<string> session
            )
        {
            _db = context;
            _cartService = cartService;
            _paymentService = paymentService;
            _session = session;
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
        void saveOrderSession(OrderInfoSession orderInfoModel)
        {
            _session.ClearSession("order");
            var listSession = _session.GetSession("order");
            string orderInfo = JsonConvert.SerializeObject(orderInfoModel);
            listSession.Add(orderInfo);
            _session.SaveSession(listSession);
        }
        [HttpPost]
        public IActionResult CheckOut(OrderInfoSession orderInfoModel)
        {
            saveOrderSession(orderInfoModel);
            switch (orderInfoModel.PaymentId)
            {
                case 1:
                    TempData["Confirmed"] = true;
                    _paymentService.COD();
                    return Json(new { redirectToUrl = Url.Action("PaymentConfirm") });
                case 2:
                    string vnpaymentUrl = _paymentService.VNPay();
                    return Json(new { redirectToUrl = vnpaymentUrl });
                case 3:

                    return Json(new { redirectToUrl = Url.Action("PaymentConfirm") });
                default:
                    return View();
            }
        }
        public IActionResult PaymentConfirm()
        {
            var listOrder = _session.GetSession("order");
            var listCart = _cartService.GetCart();
            if (TempData.ContainsKey("Confirmed") && (bool)TempData["Confirmed"])
            {
                // Xử lý khi xác nhận đã được thực hiện
            }
            else
            {
                // Nếu không có thông tin xác nhận, chuyển hướng người dùng
                return RedirectToAction("Store", "Home");
            }
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listOrder[0]);
            ViewBag.listCart = listCart;
            _session.ClearSession("order");
            _session.ClearSession("cart");
            return View(orderInfo);
        }
        public IActionResult VNPayConfirm()
        {
            if (!_paymentService.VNPayConfirm())
            {

            }
            TempData["Confirmed"] = true;
            return RedirectToAction("PaymentConfirm");
        }

    }
}
