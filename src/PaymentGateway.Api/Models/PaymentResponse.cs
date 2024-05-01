namespace PaymentGateway.Api.Models;

/// <summary>
/// Represents a payment response.
/// </summary>
public class PaymentResponse
{
    /// <summary>
    /// Gets or sets the ID of the payment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the status of the payment.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Gets or sets the last four digits of the card used for the payment.
    /// </summary>
    public string LastFourCardDigits { get; set; }

    /// <summary>
    /// Gets or sets the expiry month of the card used for the payment.
    /// </summary>
    public string ExpiryMonth { get; set; }

    /// <summary>
    /// Gets or sets the expiry year of the card used for the payment.
    /// </summary>
    public int ExpiryYear { get; set; }

    /// <summary>
    /// Gets or sets the currency of the payment.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the amount of the payment.
    /// </summary>
    public int Amount { get; set; }
}

