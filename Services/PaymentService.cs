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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly VnPayConfig _vnPayConfig;
        public PaymentService(
            IHttpContextAccessor httpContextAccessor, 
            IOptions<VnPayConfig> vnPayConfig)
        {
            _httpContextAccessor = httpContextAccessor;
            _vnPayConfig = vnPayConfig.Value;
        }
        public string VNPay(Order orderModel)
        {
            DateTime expirationTime = DateTime.Now.AddDays(1);
            PayLib pay = new PayLib();
            pay.AddRequestData("vnp_Version", _vnPayConfig.Version);
            pay.AddRequestData("vnp_Command", _vnPayConfig.Command);
            pay.AddRequestData("vnp_TmnCode", _vnPayConfig.TmnCode);
            pay.AddRequestData("vnp_Amount", (orderModel.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", expirationTime.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress(_httpContextAccessor));
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", _vnPayConfig.ReturnUrl);
            pay.AddRequestData("vnp_TxnRef", orderModel.ToString()!);
            //pay.AddRequestData("vnp_payment", order.Payment.ToString()); //mã hóa đơn
            string paymentUrl = pay.CreateRequestUrl(_vnPayConfig.Url, _vnPayConfig.HashSecret);
            return paymentUrl;
        }
    }
}
