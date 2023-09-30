using Clothings_Store.Models;

namespace Clothings_Store.Patterns.Strategy
{
    public interface IDiscountStrategy
    {
        decimal CalculateDiscount(Promotion promotion, decimal originalPrice);
    }
    public class MorningDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Promotion promotion, decimal originalPrice)
        {
            return originalPrice * 0.9m;
        }
    }

    public class EveningDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Promotion promotion, decimal originalPrice)
        {
            return originalPrice * 0.95m;
        }
    }

}
