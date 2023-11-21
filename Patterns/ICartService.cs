using Clothings_Store.Models;

namespace Clothings_Store.Patterns
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
