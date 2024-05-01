using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Interfaces;

namespace PaymentGateway.Api.Tests;

public class PaymentTests
{
    private readonly IPaymentService _sut;
    private readonly PaymentDetail _paymentDetail;
    private readonly ILogger<BankResponse> _logger;

    public PaymentTests()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:8080/") };

        var bankHttpClient = new BankHttpClient(httpClient);

        _logger = Substitute.For<ILogger<BankResponse>>();

        _sut = new PaymentService(bankHttpClient, _logger);

        _paymentDetail = CreateValidInstance();
    }

    [Fact]
    public async Task ProcessPayment_ShouldReturnValidStatus()
    {
        // Arrange
        PaymentDetail paymentDetail = CreateValidInstance();

        // Act
        var result = await _sut.ProcessPaymentAsync(paymentDetail);

        // Assert
        result.Status.Should().Be(PaymentStatus.Authorized.ToString());
    }

    [Fact]
    public async Task ProcessPayment_WhenCardNumberIsEmpty_ShouldReturnValidationError()
    {
        // Arrange
        _paymentDetail.CardNumber = string.Empty;

        // Act
        PaymentResponse result = await _sut.ProcessPaymentAsync(_paymentDetail);


        // Assert
        result.Status.Should().Be(PaymentStatus.Rejected.ToString());

    }

    [Fact]
    public async Task ProcessPayment_WhenExpiryMonthIsNotBetween1And12_ShouldHaveValidationError()
    {
        // Arrange
        _paymentDetail.ExpiryMonth = 13;

        // Act
        PaymentResponse result = await _sut.ProcessPaymentAsync(_paymentDetail);

        // Assert
        result.Status.Should().Be(PaymentStatus.Rejected.ToString());
    }

    [Fact]
    public async Task Validate_WhenExpiryYearIsNotInTheFuture_ShouldHaveValidationError()
    {
        // Arrange
        _paymentDetail.ExpiryYear = DateTime.Now.Year - 1;

        // Act
        PaymentResponse result = await _sut.ProcessPaymentAsync(_paymentDetail);

        // Assert
        result.Status.Should().Be(PaymentStatus.Rejected.ToString());
    }

    [Fact]
    public async Task Validate_WhenCurrencyIsNotValid_ShouldHaveValidationError()
    {
        // Arrange
        _paymentDetail.Currency = "XYZ";

        // Act
        PaymentResponse result = await _sut.ProcessPaymentAsync(_paymentDetail);

        // Assert
        result.Status.Should().Be(PaymentStatus.Rejected.ToString());
    }

    [Fact]
    public async Task Validate_WhenAmountIsNotWholeNumber_ShouldHaveValidationError()
    {
        // Arrange
        _paymentDetail.Amount = 10.5m;


        // Act
        PaymentResponse result = await _sut.ProcessPaymentAsync(_paymentDetail);

        // Assert
        result.Status.Should().Be(PaymentStatus.Rejected.ToString());
    }

    [Fact]
    public async Task Validate_WhenCVVIsNotNumeric_ShouldHaveValidationError()
    {
        // Arrange
        _paymentDetail.CVV = "ABC";

        // Act
        PaymentResponse result = await _sut.ProcessPaymentAsync(_paymentDetail);

        // Assert
        result.Status.Should().Be(PaymentStatus.Rejected.ToString());
    }

    private static PaymentDetail CreateValidInstance()
    {
        // Arrange
        return new PaymentDetail
        {
            Id = Guid.NewGuid(),
            CardNumber = "2222405343248877",
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            Currency = "GBP",
            Amount = 100,
            CVV = "123"
        };
    }
}