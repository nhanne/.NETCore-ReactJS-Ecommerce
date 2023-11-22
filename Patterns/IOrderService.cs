using Clothings_Store.Models;

namespace Clothings_Store.Patterns
{
    public interface IOrderService
    {
        Order PlaceOrder(AppUser userModel, Order orderModel, string code);
    }

}
