using Domain.DTOs.BookingDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.BookingService;

public interface IBookingService
{
    Task<PagedResponse<List<GetBooking>>> GetBookingsAsync(BookingFilter filter);
    Task<Response<GetBooking>> GetBookingByIdAsync(int bookingId);
    Task<Response<string>> CreateBookingAsync(CreateBooking createBooking);
    Task<Response<string>> UpdateBookingAsync(UpdateBooking updateBooking);
    Task<Response<bool>> DeleteBookingAsync(int bookingId);
}
