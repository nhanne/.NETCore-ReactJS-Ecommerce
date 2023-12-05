using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Encodings.Web;

namespace Clothings_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly StoreContext _db;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;
        private readonly ICustomSessionService<string> _session;
        private readonly IEmailSender _emailSender;
        public CartController(
            StoreContext context,
            ICartService cartService,
            IPaymentService paymentService,
            ICustomSessionService<string> session,
            IEmailSender emailSender
            )
        {
            _db = context;
            _cartService = cartService;
            _paymentService = paymentService;
            _session = session;
            _emailSender = emailSender;
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
                    string vnpaymentUrl = _paymentService.VNPay();
                    return Json(new { redirectToUrl = vnpaymentUrl });
                case 3:
                    var response = await _paymentService.CreatePaymentAsync(orderInfoModel);
                    return Json(new { redirectToUrl = response.PayUrl });
                default:
                    return View();
            }
        }
        public async Task<IActionResult> PaymentConfirm()
        {
            if (TempData.ContainsKey("Confirmed") && (bool)TempData["Confirmed"]!)
            {
                var listCart = _cartService.GetCart();
                _session.ClearSession("cart");
                ViewBag.listCart = listCart;

                var listOrder = _session.GetSession("order");
                _session.ClearSession("order");
                var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listOrder[0]);

                var emailBody = new StringBuilder();
                emailBody.AppendLine($"Kính gửi {orderInfo.FullName}, <br><br>");
                emailBody.AppendLine($"Thông tin chi tiết đơn hàng:<br>");
                foreach (var item in listCart)
                {
                    emailBody.AppendLine($"- {item.name}, size: {item.size}, color: {item.color} x{item.quantity}</p><br/>");
                }
                emailBody.AppendLine($"Cảm ơn bạn đã ghé thăm và đặt hàng tại Clothings Store. Chúng tôi sẽ sớm xác nhận lại với bạn khi đã tiếp nhận đơn hàng.<br><br>");
                emailBody.AppendLine("Trân trọng ! <br> Tran Thanh Nhan");

                await _emailSender.SendEmailAsync(
                    orderInfo.Email,
                    "[Clothings Store] Thông báo đơn hàng",
                    emailBody.ToString()
                );

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
