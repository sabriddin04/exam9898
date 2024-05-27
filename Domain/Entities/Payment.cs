using Domain.Enums;

namespace Domain.Entities;

public class Payment:BaseEntity
{
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public StatusPayment Status { get; set; }


    public User? User { get; set; }
    public Booking? Booking { get; set; }
}
