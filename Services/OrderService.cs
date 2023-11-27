using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;
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
        public async Task PlaceOrder()
        {
            var listSession = _session.GetSession("order");
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listSession[0]);
            try
            {
                if (listSession.Count > 0 && orderInfo != null)
                {
                    Order order = new Order();
                    OrderInfo(order, orderInfo);
                    await Task.WhenAll(
                    OrderCustomer(order, orderInfo),
                    OrderDetail(order.Id));
                    _logger.LogInformation("Place Order Success.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Place Order Failed.");
                throw;
            }
        }
        private void OrderInfo(Order order, OrderInfoSession orderInfo)
        {
            try
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
                _db.Orders.Add(order);
                _db.SaveChanges();
                _logger.LogInformation("Add Order Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add Order Failed.");
                throw;
            }
        }
        private async Task OrderCustomer(Order order, OrderInfoSession orderInfoModel)
        {
            try
            {
                Customer customer = new Customer();
                customer.Email = orderInfoModel.Email;
                customer.Phone = orderInfoModel.Phone;
                customer.FullName = orderInfoModel.FullName;
                customer.Address = orderInfoModel.Address;
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                order.CustomerId = customer.Id;
                var httpContext = _httpContextAccessor.HttpContext!;
                if (httpContext.User.Identity!.IsAuthenticated == true)
                {
                    string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                    order.UserId = userId;
                }
                _logger.LogInformation("Add User Info Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add User Info Failed.");
                throw;
            }

        }
       
        private async Task OrderDetail(string orderId)
        {
            try
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
                await _db.SaveChangesAsync();
                _logger.LogInformation("Add OrderDetail Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Add OrderDetail Failed.");
                throw;
            }

        }
    }
}
