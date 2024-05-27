using Domain.Enums;

namespace Domain.Filters;

public class PaymentFilter:PaginationFilter
{
    public decimal Amount { get; set; }
    public StatusPayment? Status { get; set; }
}
