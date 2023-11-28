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
                _logger.LogInformation("Hoàn thành task OrderInfo");
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
            _logger.LogInformation("Chạy xong hàm Data rồi tới task orderInffo");
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public async Task Data(Order order, OrderInfoSession Model)
        {
            var amount = Amount(Model);
            var customerId = CustomerInfo(Model);
            var httpContext = _httpContextAccessor.HttpContext!;
            if (httpContext.User.Identity!.IsAuthenticated == true)
            {
                string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                order.UserId = userId;
            }
            order.Id = Model.Id;    
            TimeZoneInfo haNoiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime haNoiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, haNoiTimeZone);
            order.OrdTime = haNoiNow;
            order.DeliTime = order.OrdTime.AddDays(3);
            order.Status = "Chờ xác nhận";
            order.PaymentId = Model.PaymentId;
            order.Address = Model.Address;
            order.Note = Model.Note;
            order.TotalQuantity = _cartService.TotalItems();
            order.TotalPrice = await amount; 
            order.CustomerId = await customerId;
            _logger.LogInformation("Chạy xong Task amount và customerId");
        }
        private async Task<int> CustomerInfo(OrderInfoSession Model)
        {
            Customer customer = new Customer
            {
                Email = Model.Email,
                Phone = Model.Phone,
                FullName = Model.FullName,
                Address = Model.Address
            };
            await _db.Customers.AddAsync(customer);
            _logger.LogInformation("Thêm thành công khách hàng");
            _db.SaveChanges();
            return customer.Id;
        }
        private async Task<double> Amount(OrderInfoSession Model)
        {
            var codeKM = await _db.Promotions.SingleOrDefaultAsync(m => 
                               m.PromotionName == Model.DiscountCode && m.EndDate > DateTime.UtcNow);
            double percent = (codeKM != null) ? (double)codeKM.DiscountPercentage : 100;

            IBillingStrategy normalPrice = new NormalStrategy();
            var CustomerBill = new CustomerBill(normalPrice);
            return CustomerBill.LastPrice(_cartService.TotalPrice(), percent);
        }
       

        private async Task OrderDetail(string orderId)
        {
            Task task = new Task(() =>
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
                    _db.OrderDetails.AddAsync(model);
                    _logger.LogInformation("Thêm chi tiết đơn hàng " + item.IdCart);
                }
            });
            Task t2 = new Task(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    _logger.LogInformation("Đang chạy task 2 lần " + i);
                }
            });
            task.Start();
            t2.Start();
            await task;
            await t2;
            _db.SaveChanges();
            _logger.LogInformation("Hoàn thành task orderDetail");
        }
    }
}
