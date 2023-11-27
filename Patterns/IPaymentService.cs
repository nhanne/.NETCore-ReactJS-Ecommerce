using Clothings_Store.Models.Others;

namespace Clothings_Store.Patterns
{
    public interface IPaymentService
    {
        Task COD();
        Task<string> VNPay();
        Task<bool> VNPayConfirm();
        Task<MomoCreatePaymentResponse> CreatePaymentAsync(OrderInfoSession model);
        MomoExecuteResponse PaymentExecuteAsync(IQueryCollection collection);
    }
}
