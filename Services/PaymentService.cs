using Clothings_Store.Data;
using Clothings_Store.Models.Others;
using Clothings_Store.Patterns;
using Clothings_Store.Services.Others;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Clothings_Store.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly ICustomSessionService<string> _session;
        private readonly VnPayConfig _vnPayConfig;
        public PaymentService(
                            IHttpContextAccessor httpContextAccessor,
                            IOrderService orderService,
                            ICustomSessionService<string> session,
                            IOptions<VnPayConfig> vnPayConfig
                            )
        {
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _session = session;
            _vnPayConfig = vnPayConfig.Value;
        }
        public void COD()
        {
            _orderService.PlaceOrder();
        }
        public string VNPay()
        {
            var listSession = _session.GetSession("order");
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listSession[0]);
            try
            {
                if (orderInfo != null)
                {
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
                    pay.AddRequestData("vnp_Amount", (orderInfo.Amount * 100).ToString());
                    pay.AddRequestData("vnp_TxnRef", orderInfo.Id.ToString());
                    pay.AddRequestData("vnp_CreateDate", expirationTime.ToString("yyyyMMddHHmmss"));
                    pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress(_httpContextAccessor));
                    string paymentUrl = pay.CreateRequestUrl(_vnPayConfig.Url, _vnPayConfig.HashSecret);
                    return paymentUrl;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return "/";

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
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                string vnp_SecureHash = _httpContextAccessor.HttpContext.Request.Query["vnp_SecureHash"]!;
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        _orderService.PlaceOrder();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public string Momo()
        {

            return "momo.html";
        }
        public bool MomoConfirm()
        {
            return true;
        }
    }
}
