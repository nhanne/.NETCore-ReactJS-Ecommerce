using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;
using Clothings_Store.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Clothings_Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomSessionService<string> _session;

        public OrderService(
            StoreContext db,
            IUnitOfWork unitOfWork,
            ICartService cartService,
            IHttpContextAccessor httpContextAccessor,
            ICustomSessionService<string> session)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _session = session;
        }
        public async Task PlaceOrder()
        {
                Order order = new Order();
                await OrderInfo(order);
                await OrderDetail(order.Id);
        }
        private async Task OrderInfo(Order order)
        {
            var listSession = _session.GetSession("order");
            var orderInfo = JsonConvert.DeserializeObject<OrderInfoSession>(listSession[0]);
            if (orderInfo == null || listSession.Count == 0) return;
            await Data(order, orderInfo);
            _unitOfWork.OrderRepository.Create(order);
            _unitOfWork.SaveChanges();
        }
        public async Task Data(Order order, OrderInfoSession Model)
        {
            Task<double> amount = Amount(Model);
            Task<int> customerId = CustomerInfo(Model);
            Task orderInfo = new Task(() =>
            {
                var httpContext = _httpContextAccessor.HttpContext!;
                if (httpContext.User.Identity!.IsAuthenticated == true)
                {
                    string userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                    order.UserId = userId;
                }
                TimeZoneInfo haNoiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime haNoiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, haNoiTimeZone);
                order.Id = Model.Id;
                order.OrdTime = haNoiNow;
                order.DeliTime = order.OrdTime.AddDays(3);
                order.Status = "Chờ xác nhận";
                order.PaymentId = Model.PaymentId;
                order.Address = Model.Address;
                order.Note = Model.Note;
                order.TotalQuantity = _cartService.TotalItems();
            });
            orderInfo.Start();
            order.TotalPrice = await amount;
            order.CustomerId = await customerId;
            await orderInfo;
        }
        private async Task<int> CustomerInfo(OrderInfoSession Model)
        {
            Task<int> getIdCustomer = new Task<int>(() =>
            {
                Customer customer = new Customer
                {
                    Email = Model.Email,
                    Phone = Model.Phone,
                    FullName = Model.FullName,
                    Address = Model.Address
                };
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return customer.Id;
            });
            getIdCustomer.Start();
            var Id = await getIdCustomer;
            return Id;
        }
        private async Task<double> Amount(OrderInfoSession Model)
        {
            Task<double> getLastPrice = new Task<double>(() =>
            {
                var codeKM = _db.Promotions.SingleOrDefault(m =>
                              m.PromotionName == Model.DiscountCode && m.EndDate > DateTime.UtcNow);
                double percent = (codeKM != null) ? (double)codeKM.DiscountPercentage : 100;

                IBillingStrategy normalPrice = new NormalStrategy(); 
                var CustomerBill = new CustomerBill(normalPrice);
                return CustomerBill.LastPrice(_cartService.TotalPrice(), percent);
            });
            getLastPrice.Start();
            var lastPrice = await getLastPrice;
            return lastPrice;
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
                    _db.OrderDetails.Add(model);
                }
            });
            task.Start();
            await task;
            _db.SaveChanges();
        }
    }
}
