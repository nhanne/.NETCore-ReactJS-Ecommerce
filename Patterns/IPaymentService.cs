using Clothings_Store.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Patterns
{
    public interface IPaymentService
    {
        void COD(AppUser userModel, Order orderModel);
        string VNPay(AppUser userModel, Order orderModel);
        bool VNPayConfirm();
    }
}
