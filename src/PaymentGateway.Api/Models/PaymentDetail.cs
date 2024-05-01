namespace PaymentGateway.Api.Models;


/// <summary>
/// Represents the payment details.
/// </summary>
public class PaymentDetail
{
    /// <summary>
    /// Gets or sets the unique identifier for the payment.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the card number.
    /// </summary>
    public string CardNumber { get; set; }

    /// <summary>
    /// Gets or sets the expiry month of the card.
    /// </summary>
    public int ExpiryMonth { get; set; }

    /// <summary>
    /// Gets or sets the expiry year of the card.
    /// </summary>
    public int ExpiryYear { get; set; }

    /// <summary>
    /// Gets or sets the currency of the payment.
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the amount of the payment.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the CVV (Card Verification Value) of the card.
    /// </summary>
    public string CVV { get; set; }
}

