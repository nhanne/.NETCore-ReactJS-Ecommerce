using Clothings_Store.Data;
using Clothings_Store.Models.Others;
using Clothings_Store.Patterns;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            if (TempData.ContainsKey("Confirmed") && (bool)TempData["Confirmed"]! == false)
            {
                ViewBag.PaymentConfirm = "Giao dịch không thành công, vui lòng thanh toán lại";
            }
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
        public async Task<IActionResult> CheckOut(OrderInfoSession orderInfoModel)
        {
            saveOrderSession(orderInfoModel);
            switch (orderInfoModel.PaymentId)
            {
                case 1:
                    TempData["Confirmed"] = true;
                    await _paymentService.COD();
                    return Json(new { redirectToUrl = Url.Action("PaymentConfirm") });
                case 2:
                    string vnpaymentUrl = await _paymentService.VNPay();
                    return Json(new { redirectToUrl = vnpaymentUrl });
                case 3:
                    var response = await _paymentService.CreatePaymentAsync(orderInfoModel);
                    return Json(new { redirectToUrl = response.PayUrl });
                default:
                    return View();
            }
        }
        public IActionResult PaymentConfirm()
        {
            if (TempData.ContainsKey("Confirmed") && (bool)TempData["Confirmed"]!)
            {
                var listOrder = _session.GetSession("order");
                var listCart = _cartService.GetCart();
                var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listOrder[0]);
                ViewBag.listCart = listCart;
                _session.ClearSession("order");
                _session.ClearSession("cart");
                return View(orderInfo);
            }
            else
            {
                return RedirectToAction("Store", "Home");
            }
        }
        public async Task<IActionResult> VNPayConfirm()
        {
            if (!await _paymentService.VNPayConfirm())
            {
                TempData["Confirmed"] = false;
                return RedirectToAction("Index");
            }
            TempData["Confirmed"] = true;
            return RedirectToAction("PaymentConfirm");
        }
        [HttpGet]
        public IActionResult MomoConfirm()
        { 
            var response = _paymentService.PaymentExecuteAsync(HttpContext.Request.Query);
            if (int.Parse(response.ErrorCode) != 0)
            {
                TempData["Confirmed"] = false;
                return RedirectToAction("Index");
            }
            TempData["Confirmed"] = true;
            return RedirectToAction("PaymentConfirm");
        }

    }
}
