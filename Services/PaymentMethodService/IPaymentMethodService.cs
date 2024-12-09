using Pacifica.API.Dtos.PaymentMethod;
using Pacifica.API.Models.Transaction;

namespace Pacifica.API.Services.PaymentMethodService
{
    public interface IPaymentMethodService
    {
        Task<ApiResponse<IEnumerable<PaymentMethod>>> GetAllPaymentMethodsAsync();
        Task<ApiResponse<IEnumerable<PaymentMethod>>> GetPaymentMethodByPageAsync(int page, int pageSize, string sortField, int sortOrder);
        Task<ApiResponse<PaymentMethod>> GetPaymentMethodByIdAsync(int id);
        Task<ApiResponse<PaymentMethod>> CreatePaymentMethodAsync(PaymentMethod paymentMethodDto);
        Task<ApiResponse<PaymentMethod>> UpdatePaymentMethodAsync(int id, PaymentMethod paymentMethodDto);
        Task<ApiResponse<bool>> DeletePaymentMethodAsync(int id);
    }
}
