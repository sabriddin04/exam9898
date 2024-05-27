using Domain.DTOs.RoomDTOs;
using Domain.Filters;
using Infrastructure.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class RoomController(IRoomService roomService) : ControllerBase
{
    [HttpGet("Rooms")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetRooms([FromQuery] RoomFilter filter)
    {
        var res1 = await roomService.GetRoomsAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{RoomId:int}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetRoomById(int RoomId)
    {
        var res1 = await roomService.GetRoomByIdAsync(RoomId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateRoom([FromForm] CreateRoom createRoom)
    {
        var result = await roomService.CreateRoomAsync(createRoom);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateRoom([FromForm] UpdateRoom updateRoom)
    {
        var result = await roomService.UpdateRoomAsync(updateRoom);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{RoomId:int}")]
    public async Task<IActionResult> DeleteRoom(int roomId)
    {
        var result = await roomService.DeleteRoomAsync(roomId);
        return StatusCode(result.StatusCode, result);
    }
}