using Clothings_Store.Controllers;
using Clothings_Store.Data;
using Clothings_Store.Models;
using Clothings_Store.Patterns;

namespace Clothings_Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreContext _db;
        public OrderService(StoreContext db)
        {
            _db = db;
        }
        public void OrderCustomer(Order order, AppUser userModel)
        {
            var user = _db.Users.SingleOrDefault(m => m.Email.Equals(userModel.Email));
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
        }

        public void OrderDetail(int orderId, List<Cart> listCart)
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
            }
        }

        public void OrderInfo(Order order, Order orderModel, string code, double totalPrice, int TotalItems)
        {
            order.OrdTime = DateTime.Now;
            order.DeliTime = order.OrdTime.AddDays(3);
            order.Status = "Chờ xác nhận";
            order.PaymentId = orderModel.PaymentId;
            order.Address = orderModel.Address;
            order.Note = orderModel.Note;
            order.TotalQuantity = TotalItems;
            // Get Promotion
            DateTime now = DateTime.Now;
            var codeKM = _db.Promotions.SingleOrDefault(m => m.PromotionName == code && m.EndDate > now);
            double percent = (code != null) ? (double)codeKM.DiscountPercentage : 100;
            // How to apply strategy pattern to calculate TotalPrice;=
            IBillingStrategy strategy = new NormalStrategy();
            var CustomerBill = new CustomerBill(strategy);
            order.TotalPrice = CustomerBill.LastPrice(totalPrice, percent);
        }
    }
}
