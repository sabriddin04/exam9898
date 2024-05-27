using System.Net;
using Domain.DTOs.PaymentDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.PaymentService;

public class PaymentService(ILogger<PaymentService>logger,DataContext context):IPaymentService
{


 #region GetPaymentsAsync

    public async Task<PagedResponse<List<GetPayment>>> GetPaymentsAsync(PaymentFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetPaymentsAsync} in time:{DateTime} ", "GetPaymentsAsync",
                DateTimeOffset.UtcNow);
            var payments = context.Payments.AsQueryable();

            if (filter.Status != null)
                payments = payments.Where(x => x.Status == filter.Status);
            if (filter.Amount != 0)
                payments = payments.Where(x => x.Amount == filter.Amount);

            var response = await payments.Select(x => new GetPayment()
            {
                Status = x.Status,
                Amount = x.Amount,
                UserId = x.UserId,
                Date = x.Date,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await payments.CountAsync();

            logger.LogInformation("Finished method {GetPaymentsAsync} in time:{DateTime} ", "GetPaymentsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetPayment>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetPayment>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetPaymentByIdAsync

    public async Task<Response<GetPayment>> GetPaymentByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Starting method {GetPaymentByIdAsync} in time:{DateTime} ", "GetPaymentByIdAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Payments.Select(x => new GetPayment()
            {
                Status = x.Status,
                Amount = x.Amount,
                UserId = x.UserId,
                Date = x.Date,
                Id = x.Id,
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (existing is null)
            {
                logger.LogWarning("Could not find Payment with Id:{Id},time:{DateTimeNow}", id, DateTimeOffset.UtcNow);
                return new Response<GetPayment>(HttpStatusCode.BadRequest, $"Not found Payment by id:{id}");
            }


            logger.LogInformation("Finished method {GetPaymentByIdAsync} in time:{DateTime} ", "GetPaymentByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetPayment>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetPayment>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreatePaymentAsync

    public async Task<Response<string>> CreatePaymentAsync(CreatePayment createPayment)
    {
        try
        {
            logger.LogInformation("Starting method {CreatePaymentAsync} in time:{DateTime} ", "CreatePaymentAsync",
                DateTimeOffset.UtcNow);

            var newPayment = new Payment()
            {
                Status = createPayment.Status,
                Amount = createPayment.Amount,
                UserId = createPayment.UserId,
                Date = DateTime.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
            };
            await context.Payments.AddAsync(newPayment);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {CreatePaymentAsync} in time:{DateTime} ", "CreatePaymentAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created Payment by Id:{newPayment.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdatePaymentAsync

    public async Task<Response<string>> UpdatePaymentAsync(UpdatePayment updatePayment)
    {
        try
        {
            logger.LogInformation("Starting method {UpdatePaymentAsync} in time:{DateTime} ", "UpdatePaymentAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Payments.Where(x => x.Id == updatePayment.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(r => r.Status, updatePayment.Status)
                    .SetProperty(r => r.Amount, updatePayment.Amount)
                    .SetProperty(r => r.UserId, updatePayment.UserId)
                    .SetProperty(r => r.Date, updatePayment.Date)
                    .SetProperty(r => r.UpdateAt, DateTimeOffset.UtcNow));

            logger.LogInformation("Finished method {UpdatePaymentAsync} in time:{DateTime} ", "UpdatePaymentAsync",
                DateTimeOffset.UtcNow);

            return existing == 0
                ? new Response<string>(HttpStatusCode.BadRequest, $"Not found Payment by id:{updatePayment.Id}")
                : new Response<string>($"Successfully updated Payment by id:{updatePayment.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeletePaymentAsync

    public async Task<Response<bool>> DeletePaymentAsync(int id)
    {
        try
        {
            logger.LogInformation("Starting method {DeletePaymentAsync} in time:{DateTime} ", "DeletePaymentAsync",
                DateTimeOffset.UtcNow);

            var payment = await context.Payments.Where(x => x.Id == id).ExecuteDeleteAsync();

            logger.LogInformation("Finished method {DeletePaymentAsync} in time:{DateTime} ", "DeletePaymentAsync",
                DateTimeOffset.UtcNow);
            return payment == 0
                ? new Response<bool>(HttpStatusCode.BadRequest, $"Payment not found by id:{id}")
                : new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion


}
