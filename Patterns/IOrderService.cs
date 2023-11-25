using Clothings_Store.Models;

namespace Clothings_Store.Patterns
{
    public interface IOrderService
    {
        void PlaceOrder(OrderInfoSession orderInfoModel);
    }

}
