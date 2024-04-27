using Clothings_Store.Data;
using Clothings_Store.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Repositories;
public class StockRepository : GenericRepository<Stock, int>
{
    public StockRepository(StoreContext context) : base(context)
    {
    }
 
}
