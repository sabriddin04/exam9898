using System.Net;
using Domain.DTOs.RoomDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RoomService;

public class RoomService(ILogger<RoomService> logger, DataContext context,IFileService fileService): IRoomService
{
    #region GetRoomsAsync

    public async Task<PagedResponse<List<GetRoom>>> GetRoomsAsync(RoomFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetRoomsAsync} in time:{DateTime} ", "GetRoomsAsync",
                DateTimeOffset.UtcNow);

            var rooms = context.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(filter.RoomNumber))
                rooms = rooms.Where(x => x.RoomNumber.ToLower().Contains(filter.RoomNumber.ToLower()));

            if (filter.PricePerNight!=0)
                rooms = rooms.Where(x => x.PricePerNight==filter.PricePerNight);

            if (filter.Status != null)
                rooms = rooms.Where(x => x.Status == filter.Status);

            var response = await rooms.Select(x => new GetRoom()
            {
                RoomNumber = x.RoomNumber,
                Description = x.Description,
                Type = x.Type,
                Status = x.Status,
                PricePerNight = x.PricePerNight,
                PhotoPath=x.PhotoPath,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await rooms.CountAsync();

            logger.LogInformation("Finished method {GetRoomsAsync} in time:{DateTime} ", "GetRoomsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetRoom>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetRoom>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion


    #region GetRoomByIdAsync

    public async Task<Response<GetRoom>> GetRoomByIdAsync(int roomId)
    {
        try
        {
            logger.LogInformation("Starting method {GetRoomByIdAsync} in time:{DateTime} ", "GetRoomByIdAsync",
                DateTimeOffset.UtcNow);
            var room = await context.Rooms.Select(x => new GetRoom()
            {
                RoomNumber = x.RoomNumber,
                Description = x.Description,
                Type = x.Type,
                Status = x.Status,
                PricePerNight = x.PricePerNight,
                PhotoPath=x.PhotoPath,
                Id = x.Id,
            }).FirstOrDefaultAsync(x => x.Id == roomId);

            logger.LogInformation("Finished method {GetRoomByIdAsync} in time:{DateTime} ", "GetRoomByIdAsync",
                DateTimeOffset.UtcNow);
            return room == null
                ? new Response<GetRoom>(HttpStatusCode.BadRequest, $"Room not found by ID:{roomId}")
                : new Response<GetRoom>(room);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetRoom>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    

     #region CreateRoomAsync

    public async Task<Response<string>> CreateRoomAsync(CreateRoom createRoom)
    {
        try
        {
            logger.LogInformation("Starting method {CreateRoomAsync} in time:{DateTime} ", "CreateRoomAsync",
                DateTimeOffset.UtcNow);
            var newRoom = new Room()
            {
                UpdateAt = DateTimeOffset.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                Description = createRoom.Description,
                RoomNumber = createRoom.RoomNumber,
                PricePerNight = createRoom.PricePerNight,
                Type=createRoom.Type,
                Status=createRoom.Status,
                PhotoPath = createRoom.PhotoPath == null ? "null" : await fileService.CreateFile(createRoom.PhotoPath)
            };
            await context.Rooms.AddAsync(newRoom);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {createRoomAsync} in time:{DateTime} ", "createRoomAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Room by id:{newRoom.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion




    
    #region UpdateRoomAsync

    public async Task<Response<string>> UpdateRoomAsync(UpdateRoom updateRoom)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateRoomAsync} in time:{DateTime} ", "UpdateRoomAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Rooms.FirstOrDefaultAsync(x => x.Id == updateRoom.Id);
            if (existing is null)
            {
                logger.LogWarning("Room not found by id:{Id},time:{DateTimeNow} ", updateRoom.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Room not found");
            }

            if (updateRoom.PhotoPath != null)
            {
                if (existing.PhotoPath != null) fileService.DeleteFile(existing.PhotoPath);
                existing.PhotoPath = await fileService.CreateFile(updateRoom.PhotoPath);
            }

            existing.Description = updateRoom.Description;
            existing.Status = updateRoom.Status!;
            existing.PricePerNight = existing.PricePerNight;
            existing.RoomNumber = existing.RoomNumber;
            existing.Status = existing.Status;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateRoomAsync} in time:{DateTime} ", "UpdateRoomAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Room by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion



    #region DeleteRoomAsync

    public async Task<Response<bool>> DeleteRoomAsync(int roomId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteRoomAsync} in time:{DateTime} ", "DeleteRoomAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Room not found by id:{roomId}");
            if (existing.PhotoPath != null)
                fileService.DeleteFile(existing.PhotoPath);
            context.Rooms.Remove(existing);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {DeleteRoomAsync} in time:{DateTime} ", "DeleteRoomAsync",
                DateTimeOffset.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion










}
