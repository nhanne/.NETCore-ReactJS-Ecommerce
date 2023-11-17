namespace Clothings_Store.Patterns
{
    public interface IBillingStrategy
    {
        double GetActPrice(double rawPrice);
    }
    public class NormalStrategy : IBillingStrategy
    {
        public double GetActPrice(double rawPrice) => rawPrice;
    }
    public class CustomerBill
    {
        public IBillingStrategy Strategy { get; set; }
        public CustomerBill(IBillingStrategy strategy)
        {
            this.Strategy = strategy;
        }
        public double LastPrice(double price, double percent)
        {
            return this.Strategy.GetActPrice(price * (percent / 100));
        }
    }
}
