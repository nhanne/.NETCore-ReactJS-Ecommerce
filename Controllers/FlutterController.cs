using Clothings_Store.Data;
using Clothings_Store.Models.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clothings_Store.Controllers
{
    public class FlutterController : Controller
    {
        private readonly StoreContext _db;
        public FlutterController(StoreContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<JsonResult> GetListOrdersById(string id)
        {
            var orders = _db.OrderFlutters.AsQueryable();
            var query = await orders.Where(o => o.FlutterAccountId == id).ToListAsync();
            return Json(query);
        }

        [HttpPost]
        public async Task<bool> CheckOut([FromBody] OrderFlutter model, 
                                         [FromBody] List<OrderDetailFlutter> listOrderDetails)
        {
            try
            {
                OrderFlutter newOrder = new OrderFlutter();
                newOrder.Id = DateTime.UtcNow.Ticks.ToString();
                newOrder.Name = model.Name;
                newOrder.Address = model.Address;
                newOrder.PhoneNumber = model.PhoneNumber;
                newOrder.FlutterAccountId = model.FlutterAccountId;
                await _db.OrderFlutters.AddAsync(model);
                await _db.SaveChangesAsync();
                foreach (var item in listOrderDetails)
                {
                    OrderDetailFlutter newOrderDetail = new OrderDetailFlutter
                    {
                        OrderFlutterId = newOrder.Id,
                        StockId = item.StockId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    await _db.OrderDetailFlutters.AddAsync(newOrderDetail);
                }
                await _db.SaveChangesAsync();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
