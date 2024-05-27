using Domain.DTOs.BookingDTOs;
using Domain.Filters;
using Infrastructure.Services.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    [HttpGet("Bookings")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetBookings([FromQuery] BookingFilter filter)
    {
        var res1 = await bookingService.GetBookingsAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{BookingId:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetBookingById(int BookingId)
    {
        var res1 = await bookingService.GetBookingByIdAsync(BookingId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBooking createBooking)
    {
        var result = await bookingService.CreateBookingAsync(createBooking);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateBooking([FromBody] UpdateBooking updateBooking)
    {
        var result = await bookingService.UpdateBookingAsync(updateBooking);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{BookingId:int}")]
    public async Task<IActionResult> DeleteBooking(int BookingId)
    {
        var result = await bookingService.DeleteBookingAsync(BookingId);
        return StatusCode(result.StatusCode, result);
    }
}