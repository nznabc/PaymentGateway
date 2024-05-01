using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Interfaces;

namespace PaymentGateway.Api.Tests;

public class PaymentHistoryTests
{
    private readonly IPaymentHistoryInMemoryService _sut;
    private readonly PaymentResponseList _paymentResponseList;

    public PaymentHistoryTests()
    {
        _paymentResponseList = new PaymentResponseList();
        _sut = new PaymentHistoryInMemoryService(_paymentResponseList);
    }

    [Fact]
    public void SavePaymentHistory_Should_Add_PaymentResponse_To_PaymentResponses()
    {
        // Arrange
        var paymentResponse = new PaymentResponse
        {
            Id = Guid.NewGuid(),
            Status = PaymentStatus.Authorized.ToString(),
            LastFourCardDigits = "8877",
            ExpiryMonth = 4.ToString("D2"),
            ExpiryYear = 2025,
            Currency = "GBP",
            Amount = 100
        };

        // Act
        _sut.SavePaymentHistory(paymentResponse);

        // Assert
        Assert.Contains(paymentResponse, _paymentResponseList.PaymentResponses);
    }

    [Fact]
    public void GetPaymentHistory_ReturnsPaymentHistory()
    {
        // Arrange
        Guid paymentDetailId = Guid.NewGuid();

        var paymentReponse = new PaymentResponse
        {
            Id = paymentDetailId,
            Status = PaymentStatus.Authorized.ToString(),
            LastFourCardDigits = "8877",
            ExpiryMonth = 4.ToString("D2"),
            ExpiryYear = 2025,
            Currency = "GBP",
            Amount = 100
        };

        _paymentResponseList.PaymentResponses.Add(paymentReponse);
        //_sut.SavePaymentHistory(paymentReponse);

        // Act
        var result = _sut.GetPaymentHistory(paymentDetailId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PaymentResponse>(result);
        Assert.Equal(paymentReponse, result);
    }

    [Fact]
    public void GetPaymentHistory_ThrowsException_WhenIdDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        PaymentResponse? response = _sut.GetPaymentHistory(nonExistentId);

        // Assert
        Assert.Null(response);
    }
}
