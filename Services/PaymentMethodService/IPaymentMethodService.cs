using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pacifica.API.Dtos.PaymentMethod;

namespace Pacifica.API.Services.PaymentMethodService
{
    public interface IPaymentMethodService
    {
        Task<ApiResponse<IEnumerable<PaymentMethodDto>>> GetAllPaymentMethodsAsync();
        Task<ApiResponse<PaymentMethodDto>> GetPaymentMethodByIdAsync(int id);
        Task<ApiResponse<PaymentMethodDto>> CreatePaymentMethodAsync(PaymentMethodDto paymentMethodDto);
        Task<ApiResponse<PaymentMethodDto>> UpdatePaymentMethodAsync(int id, PaymentMethodDto paymentMethodDto);
        Task<ApiResponse<bool>> DeletePaymentMethodAsync(int id);
    }
}


