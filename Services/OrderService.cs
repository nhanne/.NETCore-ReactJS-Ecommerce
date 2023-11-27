using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                Order order = new Order();
                await OrderInfo(order);
                await OrderDetail(order.Id);
                _logger.LogInformation("Place Order Success.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Place Order Failed.");
                throw;
            }
        }
        private async Task OrderInfo(Order order)
        {
            var listSession = _session.GetSession("order");
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listSession[0]);
            if (orderInfo == null || listSession.Count == 0) return;
            await Data(order, orderInfo);
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public async Task Data(Order order, OrderInfoSession Model)
        {
            order.CustomerId = CustomerInfo(Model);
            var amount = Amount(Model);
            var httpContext = _httpContextAccessor.HttpContext!;
            if (httpContext.User.Identity!.IsAuthenticated == true)
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                order.UserId = userId;
            }
            order.Id = Model.Id;    
            order.OrdTime = DateTime.UtcNow;
            order.DeliTime = order.OrdTime.AddDays(3);
            order.Status = "Chờ xác nhận";
            order.PaymentId = Model.PaymentId;
            order.Address = Model.Address;
            order.Note = Model.Note;
            order.TotalQuantity = _cartService.TotalItems();
            order.TotalPrice = await amount;
        }
        private int CustomerInfo(OrderInfoSession Model)
        {
            Customer customer = new Customer();
            customer.Email = Model.Email;
            customer.Phone = Model.Phone;
            customer.FullName = Model.FullName;
            customer.Address = Model.Address;
            _db.Customers.Add(customer);
            _db.SaveChanges();
            return customer.Id;
        }
        public async Task<double> Amount(OrderInfoSession Model)
        {
            DateTime now = DateTime.UtcNow;
            var codeKM = await _db.Promotions.SingleOrDefaultAsync(m => 
                               m.PromotionName == Model.DiscountCode && m.EndDate > now);
            double percent = (codeKM != null) ? (double)codeKM.DiscountPercentage : 100;

            IBillingStrategy normalPrice = new NormalStrategy();
            var CustomerBill = new CustomerBill(normalPrice);
            return CustomerBill.LastPrice(_cartService.TotalPrice(), percent);
        }
       

        private async Task OrderDetail(string orderId)
        {
            var cartItems = _cartService.GetCart().ToList();
            foreach (var item in cartItems)
            {
                OrderDetail model = new OrderDetail
                {
                    OrderId = orderId,
                    StockId = item.IdCart,
                    Quantity = item.quantity,
                    UnitPrice = item.unitPrice
                };
                await _db.OrderDetails.AddAsync(model);
            }
            await _db.SaveChangesAsync();
        }
    }
}
