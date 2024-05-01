using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Mappings;

public static class Mappings
{
    public static PaymentDetailRequest Map(this PaymentDetail request)
    {
        return new PaymentDetailRequest
        {
            CardNumber = request.CardNumber,
            ExpiryDate = $"{request.ExpiryMonth.ToString("D2")}/{request.ExpiryYear}",
            Currency = request.Currency,
            Amount = request.Amount,
            CVV = request.CVV
        };
    }

    public static PaymentResponse Map(this BankResponse response, PaymentDetail paymentDetail)
    {
        return new PaymentResponse
        {
            Amount = (int)paymentDetail.Amount,
            Currency = paymentDetail.Currency,
            Id = paymentDetail.Id,
            LastFourCardDigits = paymentDetail.CardNumber.Substring(paymentDetail.CardNumber.Length - 4),
            Status = (response.Authorized == true ? PaymentStatus.Authorized : PaymentStatus.Declined).ToString(),
            ExpiryMonth = paymentDetail.ExpiryMonth.ToString("D2"),
            ExpiryYear = paymentDetail.ExpiryYear
        };
    }
}
