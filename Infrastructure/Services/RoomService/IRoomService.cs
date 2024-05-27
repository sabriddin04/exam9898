using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.RoomService;

public interface IRoomService
{
    Task<PagedResponse<List<GetRoom>>> GetRoomsAsync(RoomFilter filter);
    Task<Response<GetRoom>> GetRoomByIdAsync(int roomId);
    Task<Response<string>> CreateRoomAsync(CreateRoom createRoom);
    Task<Response<string>> UpdateRoomAsync(UpdateRoom updateRoom);
    Task<Response<bool>> DeleteRoomAsync(int roomId);
}
