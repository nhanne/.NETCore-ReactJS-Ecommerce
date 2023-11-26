using Clothings_Store.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Patterns
{
    public interface IPaymentService
    {
        void COD();
        string VNPay();
        bool VNPayConfirm();
        string Momo();
        bool MomoConfirm();
    }
}
