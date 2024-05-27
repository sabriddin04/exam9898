using Domain.Enums;

namespace Domain.Filters;

public class BookingFilter : PaginationFilter
{
    public StatusBooking? Status { get; set; }
}
