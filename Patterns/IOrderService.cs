using Clothings_Store.Models;

namespace Clothings_Store.Patterns
{
    public interface IOrderService
    {
        void OrderCustomer(Order order, AppUser userModel);
        void OrderInfo(Order order, Order orderModel, string code, double totalPrice, int TotalItems);
        void OrderDetail(int orderId, List<Cart> listCart);
    }

}
