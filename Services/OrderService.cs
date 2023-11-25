using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;
using System.Security.Claims;

namespace Clothings_Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(
            StoreContext db,
            ILogger<OrderService> logger,
            ICartService cartService,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _logger = logger;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }
        public void PlaceOrder(OrderInfoSession orderInfoModel)
        {
            try
            {
                Order order = new Order();
                OrderCustomer(order, orderInfoModel);
                OrderInfo(order, orderInfoModel);
                OrderDetail(order.Id);
                _cartService.ClearCart();
                _logger.LogInformation("Place Order Success.");
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
        private void OrderInfo(Order order, OrderInfoSession orderInfoModel)
        {
            if (orderInfoModel == null || _db == null) return;
            order.Id = orderInfoModel.Id;
            order.OrdTime = DateTime.Now;
            order.DeliTime = order.OrdTime.AddDays(3);
            order.Status = "Chờ xác nhận";
            order.PaymentId = orderInfoModel.PaymentId;
            order.Address = orderInfoModel.Address;
            order.Note = orderInfoModel.Note;
            order.TotalQuantity = _cartService.TotalItems();
            // Get Promotion
            DateTime now = DateTime.Now;
            var codeKM = _db.Promotions.SingleOrDefault(m => m.PromotionName == orderInfoModel.DiscountCode && m.EndDate > now);
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
