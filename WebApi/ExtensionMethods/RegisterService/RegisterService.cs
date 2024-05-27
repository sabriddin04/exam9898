using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Seed;
using Infrastructure.Services.AuthService;
using Infrastructure.Services.BookingService;
using Infrastructure.Services.EmailService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using Infrastructure.Services.PaymentService;
using Infrastructure.Services.RoleService;
using Infrastructure.Services.RoomService;
using Infrastructure.Services.UserRoleService;
using Infrastructure.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IRoomService,RoomService>();
        services.AddScoped<IBookingService,BookingService>();
        services.AddScoped<IPaymentService,PaymentService>();
        services.AddScoped<IRoleService,RoleService>();
        services.AddScoped<IUserRoleService,UserRoleService>();
        
    }
}