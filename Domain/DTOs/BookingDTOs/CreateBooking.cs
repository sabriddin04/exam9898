using Domain.Enums;

namespace Domain.DTOs.BookingDTOs;

public class CreateBooking
{
   public int UserId { get; set; }
   public int RoomId { get; set; }
   public DateTime CheckInDate{ get; set; }
   public DateTime CheckOutDate { get; set; }
   public StatusBooking Status { get; set; }
}
