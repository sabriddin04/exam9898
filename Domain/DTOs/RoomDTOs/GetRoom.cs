using Domain.Enums;

namespace Domain.DTOs.RoomDTOs;

public class GetRoom
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = null!;
    public string? Description { get; set; }
    public RoomType Type { get; set; }
    public StatusRoom Status { get; set; }
    public decimal PricePerNight { get; set; }
    public string PhotoPath { get; set; } = null!;
}
