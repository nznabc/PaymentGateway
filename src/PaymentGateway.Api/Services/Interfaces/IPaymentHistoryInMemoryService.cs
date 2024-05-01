using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Services.Interfaces;

/// <summary>
/// Represents an interface for an in-memory payment history service.
/// </summary>
public interface IPaymentHistoryInMemoryService
{
    /// <summary>
    /// Saves the payment history.
    /// </summary>
    /// <param name="paymentResponse">The payment response to be saved.</param>
    void SavePaymentHistory(PaymentResponse paymentResponse);

    /// <summary>
    /// Retrieves the payment history by payment detail ID.
    /// </summary>
    /// <param name="paymentDetailId">The ID of the payment detail.</param>
    /// <returns>The payment response associated with the specified payment detail ID, or null if not found.</returns>
    PaymentResponse? GetPaymentHistory(Guid paymentDetailId);
}
