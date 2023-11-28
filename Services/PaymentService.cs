using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Others;
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
        private readonly MoMoConfig _momoConfig;
        public PaymentService(
                            IHttpContextAccessor httpContextAccessor,
                            IOrderService orderService,
                            ICustomSessionService<string> session,
                            IOptions<VnPayConfig> vnPayConfig,
                            IOptions<MoMoConfig> momoConfig
                            )
        {
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
            _session = session;
            _vnPayConfig = vnPayConfig.Value;
            _momoConfig = momoConfig.Value;
        }
        public async Task COD()
        {
            await _orderService.PlaceOrder();
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
        public async Task<bool> VNPayConfirm()
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
                        await _orderService.PlaceOrder();
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
        public async Task<MomoCreatePaymentResponse> CreatePaymentAsync(OrderInfoSession model)
        {
            model.Id = DateTime.UtcNow.Ticks.ToString();
            model.Note = "Khách hàng: " + model.FullName + ". Nội dung: " + model.Note;
            var rawData =
                $"partnerCode={_momoConfig.PartnerCode}&accessKey={_momoConfig.AccessKey}&requestId={model.Id}&amount={model.Amount}&orderId={model.Id}&orderInfo={model.Note}&returnUrl={_momoConfig.ReturnUrl}&notifyUrl={_momoConfig.NotifyUrl}&extraData=";

            var signature = ComputeHmacSha256(rawData, _momoConfig.SecretKey);

            var client = new RestClient(_momoConfig.MomoApiUrl);
            var request = new RestRequest() { Method = RestSharp.Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = _momoConfig.AccessKey,
                partnerCode = _momoConfig.PartnerCode,
                requestType = _momoConfig.RequestType,
                notifyUrl = _momoConfig.NotifyUrl,
                returnUrl = _momoConfig.ReturnUrl,
                orderId = model.Id,
                amount = model.Amount.ToString(),
                orderInfo = model.Note,
                requestId = model.Id,
                extraData = "",
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponse>(response.Content!)!;
        }

        public MomoExecuteResponse PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            var errorCode = collection.First(s => s.Key == "errorCode").Value;
            return new MomoExecuteResponse()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo,
                ErrorCode = errorCode
            };
        }
        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }
}
