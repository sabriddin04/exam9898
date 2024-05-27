using Domain.Enums;

namespace Domain.DTOs.PaymentDTOs;

public class CreatePayment
{
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public StatusPayment Status { get; set; }
}
