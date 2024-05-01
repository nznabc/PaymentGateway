using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Services.Interfaces;

/// <summary>
/// Represents a payment service.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment asynchronously.
    /// </summary>
    /// <param name="paymentDetails">The payment details.</param>
    /// <returns>PaymentResponse</returns>
    Task<PaymentResponse> ProcessPaymentAsync(PaymentDetail paymentDetails);
}
