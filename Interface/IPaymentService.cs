using Clothings_Store.Models.Others;

namespace Clothings_Store.Interface
{
    public interface IPaymentService
    {
        Task Cod();
        string VnPay();
        Task<bool> VnPayConfirm();
        Task<MomoCreatePaymentResponse> CreatePaymentAsync(OrderInfoSession model);
        MomoExecuteResponse PaymentExecuteAsync(IQueryCollection collection);
    }
}
