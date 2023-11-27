using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;

namespace Clothings_Store.Interface
{
    public interface ICartService
    {
        List<Cart> GetCart();
        double TotalPrice();
        int TotalItems();
        void ClearCart();
        void SaveCartSession(List<Cart> listCart);
        void AddToCart(Stock stock);
        int RemoveFromCart(int IdCart);
        void UpdateCart(int IdCart, int Quantity);
    }
}
