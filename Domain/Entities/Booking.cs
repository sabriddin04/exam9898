using Domain.Enums;

namespace Domain.Entities;

public class Booking:BaseEntity
{
   public int UserId { get; set; }
   public int RoomId { get; set; }
   public DateTime CheckInDate{ get; set; }
   public DateTime CheckOutDate { get; set; }
   public StatusBooking Status { get; set; }
   
   
   public User? User { get; set; }
   public Room? Room { get; set; }
}
