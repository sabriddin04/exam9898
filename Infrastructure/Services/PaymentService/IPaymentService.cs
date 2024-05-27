using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.PaymentService;

public interface IPaymentService
{ 
    Task<PagedResponse<List<GetPayment>>> GetPaymentsAsync(PaymentFilter filter);
    Task<Response<GetPayment>> GetPaymentByIdAsync(int paymentId);
    Task<Response<string>> CreatePaymentAsync(CreatePayment createPayment);
    Task<Response<string>> UpdatePaymentAsync(UpdatePayment updatePayment);
    Task<Response<bool>> DeletePaymentAsync(int paymentId);
}
