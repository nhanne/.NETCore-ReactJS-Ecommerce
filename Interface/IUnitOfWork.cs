using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Clothings_Store.Repositories;

namespace Clothings_Store.Interface;
public interface IUnitOfWork
{
    IRepository<Customer, int> CustomerRepository { get; }
    IRepository<Product, int> ProductRepository { get; }
    IRepository<Order, string> OrderRepository { get; }
    IRepository<OrderDetail, string> OrderDetailRepository { get; }
    void SaveChanges();
}
public class UnitOfWork : IUnitOfWork
{
    private StoreContext context;

    public UnitOfWork(StoreContext context)
    {
        this.context = context;
    }

    private IRepository<Customer, int> _customerRepository;
    public IRepository<Customer, int> CustomerRepository
    {
        get
        {
            if (_customerRepository == null)
                _customerRepository = new CustomerRepository(context);
            return _customerRepository;
        }
    }

    private IRepository<Order, string> _orderRepository;
    public IRepository<Order, string> OrderRepository
    {
        get
        {
            if (_orderRepository == null)
                _orderRepository = new OrderRepository(context);
            return _orderRepository;
        }
    }

    private IRepository<Product, int> _productRepository;
    public IRepository<Product, int> ProductRepository
    {
        get
        {
            if (_productRepository == null)
                _productRepository = new ProductRepository(context);
            return _productRepository;
        }
    }
    private IRepository<OrderDetail, string> _orderDetailRepository;
    public IRepository<OrderDetail, string> OrderDetailRepository
    {
        get
        {
            if (_orderDetailRepository == null)
                _orderDetailRepository = new OrderDetailRepository(context);
            return _orderDetailRepository;
        }
    }
    public void SaveChanges()
    {
        context.SaveChanges();
    }
}