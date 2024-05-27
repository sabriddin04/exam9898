using System.Net;
using Domain.DTOs.BookingDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.BookingService;

public class BookingService(ILogger<BookingService> logger, DataContext context) : IBookingService
{
    #region GetBookingsAsync

    public async Task<PagedResponse<List<GetBooking>>> GetBookingsAsync(BookingFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetBookingsAsync} in time:{DateTime} ", "GetBookingsAsync",
                DateTimeOffset.UtcNow);
            var bookings = context.Bookings.AsQueryable();

            if (filter.Status != null)
                bookings = bookings.Where(x => x.Status == filter.Status);

            var response = await bookings.Select(x => new GetBooking()
            {
                Status = x.Status,
                RoomId = x.RoomId,
                UserId = x.UserId,
                CheckInDate = x.CheckInDate,
                CheckOutDate = x.CheckOutDate,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await bookings.CountAsync();

            logger.LogInformation("Finished method {GetBookingsAsync} in time:{DateTime} ", "GetBookingsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetBooking>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetBooking>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetBookingByIdAsync

    public async Task<Response<GetBooking>> GetBookingByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Starting method {GetBookingByIdAsync} in time:{DateTime} ", "GetBookingByIdAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Bookings.Select(x => new GetBooking()
            {
                Status = x.Status,
                RoomId = x.RoomId,
                UserId = x.UserId,
                CheckInDate = x.CheckInDate,
                CheckOutDate = x.CheckOutDate,
                Id = x.Id
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (existing is null)
            {
                logger.LogWarning("Could not find Booking with Id:{Id},time:{DateTimeNow}", id, DateTimeOffset.UtcNow);
                return new Response<GetBooking>(HttpStatusCode.BadRequest, $"Not found Booking by id:{id}");
            }


            logger.LogInformation("Finished method {GetBookingByIdAsync} in time:{DateTime} ", "GetBookingByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetBooking>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetBooking>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateBookingAsync

    public async Task<Response<string>> CreateBookingAsync(CreateBooking createBooking)
    {
        try
        {
            logger.LogInformation("Starting method {CreateBookingAsync} in time:{DateTime} ", "CreateBookingAsync",
                DateTimeOffset.UtcNow);

            var newBooking = new Booking()
            {
                Status = createBooking.Status,
                RoomId = createBooking.RoomId,
                UserId = createBooking.UserId,
                CheckInDate = createBooking.CheckInDate,
                CheckOutDate = createBooking.CheckOutDate,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
            };
            await context.Bookings.AddAsync(newBooking);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {CreateBookingAsync} in time:{DateTime} ", "CreateBookingAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created Booking by Id:{newBooking.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateBookingAsync

    public async Task<Response<string>> UpdateBookingAsync(UpdateBooking updateBooking)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateBookingAsync} in time:{DateTime} ", "UpdateBookingAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Bookings.Where(x => x.Id == updateBooking.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(r => r.Status, updateBooking.Status)
                    .SetProperty(r => r.RoomId, updateBooking.RoomId)
                    .SetProperty(r => r.UserId, updateBooking.UserId)
                    .SetProperty(r => r.CheckInDate, updateBooking.CheckOutDate)
                    .SetProperty(r => r.UpdateAt, DateTimeOffset.UtcNow));

            logger.LogInformation("Finished method {UpdateBookingAsync} in time:{DateTime} ", "UpdateBookingAsync",
                DateTimeOffset.UtcNow);

            return existing == 0
                ? new Response<string>(HttpStatusCode.BadRequest, $"Not found Booking by id:{updateBooking.Id}")
                : new Response<string>($"Successfully updated Booking by id:{updateBooking.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteBookingAsync

    public async Task<Response<bool>> DeleteBookingAsync(int bookingId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteBookingAsync} in time:{DateTime} ", "DeleteBookingAsync",
                DateTimeOffset.UtcNow);

            var booking = await context.Bookings.Where(x => x.Id == bookingId).ExecuteDeleteAsync();

            logger.LogInformation("Finished method {DeleteBookingAsync} in time:{DateTime} ", "DeleteBookingAsync",
                DateTimeOffset.UtcNow);
            return booking == 0
                ? new Response<bool>(HttpStatusCode.BadRequest, $"Booking not found by id:{bookingId}")
                : new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}
