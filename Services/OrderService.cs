using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Clothings_Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomSessionService<string> _session;
        public OrderService(
            StoreContext db,
            ILogger<OrderService> logger,
            ICartService cartService,
            IHttpContextAccessor httpContextAccessor,
            ICustomSessionService<string> session)
        {
            _db = db;
            _logger = logger;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _session = session;
        }
        public void PlaceOrder()
        {
            var listSession = _session.GetSession("order");
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listSession[0]);
            try
            {
                if (listSession.Count > 0 && orderInfo != null)
                {
                    Order order = new Order();
                    OrderCustomer(order, orderInfo);
                    OrderInfo(order, orderInfo);
                    OrderDetail(order.Id);
                    _logger.LogInformation("Place Order Success.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Place Order Failed.");
                throw;
            }
        }
        private void OrderCustomer(Order order, OrderInfoSession orderInfoModel)
        {
            var httpContext = _httpContextAccessor.HttpContext!;
            if (httpContext.User.Identity!.IsAuthenticated == true)
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                order.UserId = userId;
            }
            else
            {
                Customer customer = new Customer();
                customer.Email = orderInfoModel.Email;
                customer.Phone = orderInfoModel.Phone;
                customer.FullName = orderInfoModel.FullName;
                customer.Address = orderInfoModel.Address;
                _db.Customers.Add(customer);
                _db.SaveChanges();
                order.CustomerId = customer.Id;
            }
        }
        private void OrderInfo(Order order, OrderInfoSession orderInfo)
        {
            if (orderInfo == null || _db == null) return;
            order.Id = orderInfo.Id;
            order.OrdTime = DateTime.UtcNow;
            order.DeliTime = order.OrdTime.AddDays(3);
            order.Status = "Chờ xác nhận";
            order.PaymentId = orderInfo.PaymentId;
            order.Address = orderInfo.Address;
            order.Note = orderInfo.Note;
            order.TotalQuantity = _cartService.TotalItems();
            // Get Promotion
            DateTime now = DateTime.Now;
            var codeKM = _db.Promotions.SingleOrDefault(m => m.PromotionName == orderInfo.DiscountCode && m.EndDate > now);
            double percent = (codeKM != null) ? (double)codeKM.DiscountPercentage : 100;
            // Strategy Pattern
            IBillingStrategy normalPrice = new NormalStrategy();
            var CustomerBill = new CustomerBill(normalPrice);
            order.TotalPrice = CustomerBill.LastPrice(_cartService.TotalPrice(), percent);
            _logger.LogInformation("Use Strategy Pattern Success.");
            _db.Orders.Add(order);
            _db.SaveChanges();
        }
        private void OrderDetail(string orderId)
        {
            foreach (var item in _cartService.GetCart())
            {
                OrderDetail model = new OrderDetail
                {
                    OrderId = orderId,
                    StockId = item.IdCart,
                    Quantity = item.quantity,
                    UnitPrice = item.unitPrice
                };
                _db.OrderDetails.Add(model);
            }
            _db.SaveChanges();
        }
    }
}
