using Domain.Enums;

namespace Domain.DTOs.BookingDTOs;

public class UpdateBooking
{
    public int Id { get; set; }
     public int UserId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public StatusBooking Status { get; set; }
}
