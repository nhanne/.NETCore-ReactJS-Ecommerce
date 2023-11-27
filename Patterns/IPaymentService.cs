using Clothings_Store.Models.Others;

namespace Clothings_Store.Patterns
{
    public interface IPaymentService
    {
        void COD();
        string VNPay();
        bool VNPayConfirm();
        Task<MomoCreatePaymentResponse> CreatePaymentAsync(OrderInfoSession model);
        MomoExecuteResponse PaymentExecuteAsync(IQueryCollection collection);
    }
}
