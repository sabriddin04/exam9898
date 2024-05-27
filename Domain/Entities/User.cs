
using Domain.Enums;

namespace Domain.Entities;

public class User:BaseEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Status Status { get; set; }

    public string? Code { get; set; }
    public DateTimeOffset CodeTime { get; set; }
    public List<UserRole>? UserRoles { get; set; }
    public List<Payment>? Payments { get; set; }
    public List<Booking>? Bookings { get; set; }
}