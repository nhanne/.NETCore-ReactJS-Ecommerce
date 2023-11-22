using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using Clothings_Store.Services.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Clothings_Store.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly StoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly VnPayConfig _vnPayConfig;
        public PaymentService(
            StoreContext db,
            IHttpContextAccessor httpContextAccessor, 
            IOrderService orderService,
            IOptions<VnPayConfig> vnPayConfig
            )
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _vnPayConfig = vnPayConfig.Value;
            _orderService = orderService;
        }
        public void COD(AppUser userModel, Order orderModel)
        {
            _orderService.PlaceOrder(userModel, orderModel);
        }
        public string VNPay(AppUser userModel, Order orderModel)
        {
            Order order = _orderService.PlaceOrder(userModel, orderModel);
            DateTime expirationTime = DateTime.Now.AddDays(1);
            PayLib pay = new PayLib();
            pay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
            pay.AddRequestData("vnp_ReturnUrl", _vnPayConfig.ReturnUrl);
            pay.AddRequestData("vnp_Command", _vnPayConfig.Command);
            pay.AddRequestData("vnp_CurrCode", _vnPayConfig.CurrCode);
            pay.AddRequestData("vnp_Version", _vnPayConfig.Version);
            pay.AddRequestData("vnp_Locale", _vnPayConfig.Locale);
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_Amount", (order.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_TxnRef", order.Id.ToString());
            pay.AddRequestData("vnp_CreateDate", expirationTime.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress(_httpContextAccessor));
            string paymentUrl = pay.CreateRequestUrl(_vnPayConfig.Url, _vnPayConfig.HashSecret);
            return paymentUrl;
        }
        public bool VNPayConfirm()
        {
            if (_httpContextAccessor.HttpContext!.Request.Query.Count > 0)
            {
                string hashSecret = _vnPayConfig.HashSecret; 
                var vnpayData = _httpContextAccessor.HttpContext.Request.Query;
                PayLib pay = new PayLib(); 
                // Get all response data
                foreach (var key in vnpayData.Keys)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        string value = vnpayData[key]!;
                        pay.AddResponseData(key, value);
                    }
                }

                int orderId = Convert.ToInt32(pay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); 
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); 
                string vnp_SecureHash = _httpContextAccessor.HttpContext.Request.Query["vnp_SecureHash"]!; 
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);
                Order order = _db.Orders.FirstOrDefault(o => o.Id == orderId)!;
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        return true;
                    }
                    else
                    {
                        handlePaymentFailed(order);
                        return false;
                    }
                }
                else
                {
                    handlePaymentFailed(order);
                    return false;
                }
            }
            return true;
        }
        public void handlePaymentFailed(Order order)
        {
            var orderDetail = _db.OrderDetails.Where(o => o.OrderId == order.Id).ToList();
            foreach (var item in orderDetail)
            {
                _db.OrderDetails.Remove(item);
                _db.SaveChanges();
            }
            _db.Orders.Remove(order);
            _db.SaveChanges();
        }
    }
}
