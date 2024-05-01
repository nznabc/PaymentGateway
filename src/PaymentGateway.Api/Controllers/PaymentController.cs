using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services.Interfaces;

namespace PaymentGateway.Api.Controllers;

//TODO: Add versioneing to the controller

/// <summary>
/// Controller for handling payment related requests.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IPaymentHistoryInMemoryService _paymentHistoryInMemoryService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IPaymentService paymentService
        , IPaymentHistoryInMemoryService paymentHistoryInMemoryService
        , ILogger<PaymentController> logger)
    {
        _paymentService = paymentService;
        _paymentHistoryInMemoryService = paymentHistoryInMemoryService;
        _logger = logger;
    }

    /// <summary>
    /// Posts the payment.
    /// </summary>
    /// <param name="paymentDetails">The payment details.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost]
    public async Task<ActionResult<PaymentResponse>> PostPayment(PaymentDetail paymentDetails)
    {
        _logger.LogInformation("Payment Process starting");

        PaymentResponse paymentResponse = await _paymentService.ProcessPaymentAsync(paymentDetails);

        if (paymentResponse.Status != PaymentStatus.Rejected.ToString())
        {
            _paymentHistoryInMemoryService.SavePaymentHistory(paymentResponse);
        }

        return Ok(paymentResponse);
    }

    /// <summary>
    /// Gets the payment.
    /// </summary>
    /// <param name="paymentDetailId">The payment detail identifier.</param>
    /// <returns>The payment response.</returns>
    [HttpGet("{paymentDetailId:guid}")]
    public ActionResult<PaymentResponse> GetPayment(Guid paymentDetailId)
    {
        var result = _paymentHistoryInMemoryService.GetPaymentHistory(paymentDetailId);

        if (result is null)
        {
            return BadRequest("Payment not found.");
        }

        return Ok(result);
    }
}