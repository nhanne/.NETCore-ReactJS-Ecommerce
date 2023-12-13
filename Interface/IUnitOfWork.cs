using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Clothings_Store.Repositories;

namespace Clothings_Store.Interface;
public interface IUnitOfWork
{
    IRepository<Customer> CustomerRepository { get; }
    IRepository<Order> OrderRepository { get; }
    IRepository<Product> ProductRepository { get; }
    IRepository<OrderDetail> OrderDetailsRepository { get; }
    void SaveChanges();
}
public class UnitOfWork : IUnitOfWork
{
    private StoreContext context;

    public UnitOfWork(StoreContext context)
    {
        this.context = context;
    }

    private IRepository<Customer> customerRepository;
    public IRepository<Customer> CustomerRepository
    {
        get
        {
            if (customerRepository == null)
            {
                customerRepository = new CustomerRepository(context);
            }

            return customerRepository;
        }
    }

    private IRepository<Order> orderRepository;
    public IRepository<Order> OrderRepository
    {
        get
        {
            if (orderRepository == null)
            {
                orderRepository = new OrdersRepository(context);
            }

            return orderRepository;
        }
    }

    private IRepository<Product> productRepository;
    public IRepository<Product> ProductRepository
    {
        get
        {
            if (productRepository == null)
            {
                productRepository = new ProductsRepository(context);
            }

            return productRepository;
        }
    }
    private IRepository<OrderDetail> orderDetailsRepository;
    public IRepository<OrderDetail> OrderDetailsRepository
    {
        get
        {
            if (orderDetailsRepository == null)
            {
                orderDetailsRepository = new OrderDetailsRepository(context);
            }

            return orderDetailsRepository;
        }
    }

    public void SaveChanges()
    {
        context.SaveChanges();
    }
}