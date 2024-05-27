using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RoomDTOs;

public class CreateRoom
{
    public string RoomNumber { get; set; } = null!;
    public string? Description { get; set; }
    public RoomType Type { get; set; }
    public StatusRoom Status { get; set; }
    public decimal PricePerNight { get; set; }
    public IFormFile PhotoPath { get; set; } = null!;
}
