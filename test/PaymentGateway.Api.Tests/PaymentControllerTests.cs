using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services.Interfaces;

namespace PaymentGateway.Api.Tests
{
    public class PaymentControllerTests
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentHistoryInMemoryService _paymentHistoryInMemoryService;
        private readonly PaymentDetail _paymentDetail;
        private readonly PaymentController _controllerUT;

        public PaymentControllerTests()
        {
            _paymentService = Substitute.For<IPaymentService>();
            _logger = Substitute.For<ILogger<PaymentController>>();
            _paymentHistoryInMemoryService = Substitute.For<IPaymentHistoryInMemoryService>();
            _paymentDetail = CreateValidInstance();
            _controllerUT = new PaymentController(_paymentService, _paymentHistoryInMemoryService, _logger);

        }

        [Fact]
        public async Task PostPayment_ShouldReturnValidStatus()
        {
            // Arrange
            var paymentResponse = new PaymentResponse { Status = PaymentStatus.Authorized.ToString() };//fake response data

            _paymentService.ProcessPaymentAsync(_paymentDetail).Returns(Task.FromResult(paymentResponse));

            // Act
            Task<ActionResult<PaymentResponse>> result = _controllerUT.PostPayment(_paymentDetail);

            // Assert
            result.Result.Result.Should().BeOfType<OkObjectResult>();

            var okResult = result.Result.Result as OkObjectResult;

            var returnResponse = okResult!.Value as PaymentResponse;

            returnResponse.Should().BeEquivalentTo(paymentResponse);

            await _paymentService.Received(1).ProcessPaymentAsync(_paymentDetail);
            
            _paymentHistoryInMemoryService.Received(1).SavePaymentHistory(paymentResponse);

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
}
