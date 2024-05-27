using Domain.DTOs.PaymentDTOs;
using Domain.Filters;
using Infrastructure.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentController(IPaymentService PaymentService) : ControllerBase
{
    [HttpGet("Payments")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPayments([FromQuery] PaymentFilter filter)
    {
        var res1 = await PaymentService.GetPaymentsAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{PaymentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetPaymentById(int PaymentId)
    {
        var res1 = await PaymentService.GetPaymentByIdAsync(PaymentId);
        return StatusCode(res1.StatusCode, res1);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePayment createPayment)
    {
        var result = await PaymentService.CreatePaymentAsync(createPayment);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdatePayment([FromBody] UpdatePayment updatePayment)
    {
        var result = await PaymentService.UpdatePaymentAsync(updatePayment);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{PaymentId:int}")]
    public async Task<IActionResult> DeletePayment(int paymentId)
    {
        var result = await PaymentService.DeletePaymentAsync(paymentId);
        return StatusCode(result.StatusCode, result);
    }
}