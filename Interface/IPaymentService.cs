﻿using Clothings_Store.Models.Others;

namespace Clothings_Store.Interface
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