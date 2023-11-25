using Clothings_Store.Data;
using Clothings_Store.Models;
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
        public const string INFOKEY = "order";
        private readonly StoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;
        private readonly VnPayConfig _vnPayConfig;
        private readonly MoMoConfig _momoConfig;
        public PaymentService(
            StoreContext db,
            IHttpContextAccessor httpContextAccessor,
            IOrderService orderService,
            IOptions<VnPayConfig> vnPayConfig,
            IOptions<MoMoConfig> momoConfig
            )
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _vnPayConfig = vnPayConfig.Value;
            _momoConfig = momoConfig.Value;
        }
        public void COD(OrderInfoSession orderInfoModel)
        {
            _orderService.PlaceOrder(orderInfoModel);
        }
        private List<string> GetOrderSession()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            string jsonInfo = session.GetString(INFOKEY) ?? string.Empty;
            if (!string.IsNullOrEmpty(jsonInfo))
            {
                return JsonConvert.DeserializeObject<List<string>>(jsonInfo)!;
            }
            return new List<string>();
        }
        private void ClearSession()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                httpContext.Session.Remove(INFOKEY);
            }
        }
        public void SaveSession(List<string> listSession)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                var session = httpContext.Session;
                string jsonInfo = JsonConvert.SerializeObject(listSession);
                session.SetString(INFOKEY, jsonInfo);
            }
        }
        public string VNPay(OrderInfoSession orderInfoModel)
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
            pay.AddRequestData("vnp_Amount", (orderInfoModel.Amount * 100).ToString());
            pay.AddRequestData("vnp_TxnRef", orderInfoModel.Id.ToString());
            pay.AddRequestData("vnp_CreateDate", expirationTime.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress(_httpContextAccessor));
            ClearSession();
            var listSession = GetOrderSession();
            string orderInfo = JsonConvert.SerializeObject(orderInfoModel);
            listSession.Add(orderInfo);
            SaveSession(listSession);
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

                List<string> listSession = GetOrderSession();
                string s = listSession[0];
                var order = JsonConvert.DeserializeObject<OrderInfoSession>(listSession[0]);
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode");
                string vnp_SecureHash = _httpContextAccessor.HttpContext.Request.Query["vnp_SecureHash"]!;
                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret);
                if (checkSignature && listSession.Count > 0)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        _orderService.PlaceOrder(order);
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

        public string Momo(OrderInfoSession orderInfoModel)
        {
            return "momo";
        }
        public bool MomoConfirm()
        {
            return true;
        }
    }
}
