using Clothings_Store.Controllers;
using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;

namespace Clothings_Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _db;
        private readonly ILogger<OrderService> _logger;
        public OrderService(StoreContext db, ILogger<OrderService> logger)
        {
            try
            {
                _db = db;
                _logger = logger;
                _logger.LogInformation("Inject thành công db và logger.");
            }
            catch(Exception ex) {
                throw;
            }
        }
        public void OrderCustomer(Order order, AppUser userModel)
        {
            try
            {
                var user = _db.Users.SingleOrDefault(m => m.Email!.Equals(userModel.Email));
                if (user != null)
                {
                    order.UserId = user.Id;
                }
                else
                {
                    Customer noAccount = new Customer();
                    noAccount.Email = userModel.Email;
                    noAccount.Phone = userModel.PhoneNumber;
                    noAccount.FullName = userModel.Name;
                    noAccount.Address = userModel.Address;
                    noAccount.Password = "A2cja2xi#@!35nx/.?";
                    noAccount.Member = false;
                    _db.Customers.Add(noAccount);
                    _db.SaveChanges();
                    order.CustomerId = noAccount.Id;
                }
                _logger.LogInformation("Lưu thông tin khách hàng thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Thất bại khi lưu thông tin khách hàng.");
                throw;
            }
        }

        public void OrderDetail(int orderId, List<Cart> listCart)
        {
            try
            {
                foreach (var item in listCart)
                {
                    OrderDetail model = new OrderDetail();
                    model.OrderId = orderId;
                    model.StockId = item.IdCart;
                    model.Quantity = item.quantity;
                    model.UnitPrice = item.unitPrice;
                    _db.OrderDetails.Add(model);
                    _db.SaveChanges();
                    _logger.LogInformation("Thêm vào bảng chi tiết đơn hàng thành công.");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Thất bại khi thêm vào bảng chi tiết đơn hàng.");
                throw;
            }
        }

        public void OrderInfo(Order order, Order orderModel, string code, double totalPrice, int TotalItems)
        {
            if (orderModel == null || _db == null) return;
            order.OrdTime = DateTime.Now;
            order.DeliTime = order.OrdTime.AddDays(3);
            order.Status = "Chờ xác nhận";
            order.PaymentId = orderModel.PaymentId;
            order.Address = orderModel.Address;
            order.Note = orderModel.Note;
            order.TotalQuantity = TotalItems;
            // Get Promotion
            DateTime now = DateTime.Now;
            double percent = 100;
            var codeKM = _db.Promotions.SingleOrDefault(m => m.PromotionName == code && m.EndDate > now);
            if (codeKM != null)
            {
                percent = (double)codeKM.DiscountPercentage;
            }
            // How to apply strategy pattern to calculate TotalPrice
            try
            {
                IBillingStrategy strategy = new NormalStrategy();
                var CustomerBill = new CustomerBill(strategy);
                order.TotalPrice = CustomerBill.LastPrice(totalPrice, percent);
                _logger.LogInformation("Áp dụng mẫu thiết kế Strategy thành công.");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Thất bại khi sử dụng mẫu thiết kế Strategy.");
                throw;
            }
          
        }
    }
}
