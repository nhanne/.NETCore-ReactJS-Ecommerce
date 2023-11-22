using Clothings_Store.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Patterns
{
    public interface IPaymentService
    {
        string VNPay(Order orderModel);
    }
}
