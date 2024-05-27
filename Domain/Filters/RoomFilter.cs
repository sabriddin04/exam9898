using Domain.Enums;

namespace Domain.Filters;

public class RoomFilter : PaginationFilter
{
    public string? RoomNumber { get; set; }
    public RoomType? Type { get; set; }
    public StatusRoom? Status { get; set; }
    public decimal PricePerNight { get; set; }

}
