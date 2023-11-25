using Clothings_Store.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Patterns
{
    public interface IPaymentService
    {
        void COD(OrderInfoSession orderInfoModel);
        string VNPay(OrderInfoSession orderInfoModel);
        bool VNPayConfirm();
    }
}
