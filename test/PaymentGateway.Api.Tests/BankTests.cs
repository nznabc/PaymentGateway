using System.Net;
using System.Net.Http.Json;

using FluentAssertions;

using NSubstitute;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;

using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace PaymentGateway.Api.Tests
{
    /// <summary>
    /// Represents a test class for the BankHttpClient for the integration tests.
    /// </summary>
    public class BankTests : IDisposable
    {
        private readonly WireMockServer _wireMockServer;
        private readonly BankHttpClient _sut;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BankTests"/> class.
        /// </summary>
        public BankTests()
        {
            _wireMockServer = WireMockServer.Start();

            _httpClient = new HttpClient { BaseAddress = new Uri(_wireMockServer.Urls[0]) };

            _sut = new BankHttpClient(_httpClient);
        }

        /// <summary>
        /// Tests the PostPaymentDetailsAsync method of the BankHttpClient.
        /// </summary>
        [Fact]
        public async Task PostPaymentDetailsAsync_ShouldReturnBankResponse()
        {
            // Arrange
            var paymentDetailRequest = CreateValidInstance();

            var bankResponse = new BankResponse
            {
                Authorized = true,
                AuthorizationCode = ""
            };

            _wireMockServer
                .Given(Request.Create().WithPath("/payments").UsingPost())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.OK).WithBodyAsJson(bankResponse));

            // Act
            BankResponse? result = await _sut.PostPaymentDetailsAsync(paymentDetailRequest);

            // Assert
            result.Should().NotBeNull();

            result?.Authorized.Should().BeTrue();
        }

        /// <summary>
        /// Creates a valid instance of the PaymentDetailRequest.
        /// </summary>
        /// <returns>A valid instance of the PaymentDetailRequest.</returns>
        private static PaymentDetailRequest CreateValidInstance()
        {
            // Arrange
            return new PaymentDetailRequest
            {
                Amount = 100,
                CardNumber = "2222405343248877",
                Currency = "GBP",
                CVV = "123",
                ExpiryDate = "04/2025"
            };
        }

        /// <summary>
        /// Disposes the resources used by the test class.
        /// </summary>
        public void Dispose()
        {
            _httpClient.Dispose();

            _wireMockServer.Stop();
        }
    }
}