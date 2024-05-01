using PaymentGateway.Api.Mappings;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services.Interfaces;

namespace PaymentGateway.Api.Services;

internal class PaymentService(
    BankHttpClient bankHttpClient, ILogger<BankResponse> logger) : IPaymentService
{

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentDetail? paymentDetails)
    {
        try
        {
            if (paymentDetails == null)
            {
                throw new ArgumentNullException(nameof(paymentDetails));
            }

            var validationResult = PaymentValidation(paymentDetails);

            if (!validationResult.IsValid)
            {
                IEnumerable<string> validationErrors = validationResult.Errors.Select(x => x.ErrorMessage);

                logger.LogWarning("Payment validation failed: {ValidationErrors}", validationErrors);

                return new PaymentResponse { Status = PaymentStatus.Rejected.ToString() };
            }

            var paymentDetailRequest = paymentDetails.Map();

            BankResponse? response = await bankHttpClient.PostPaymentDetailsAsync(paymentDetailRequest);

            if (response is not null)
            {
                var paymentResponse = response.Map(paymentDetails);

                return paymentResponse;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        return new PaymentResponse { Status = PaymentStatus.Rejected.ToString() };
    }

    private static FluentValidation.Results.ValidationResult PaymentValidation(PaymentDetail paymentDetails)
    {
        var validator = new PaymentDetailValidator();

        var validationResult = validator.Validate(paymentDetails);

        return validationResult;
    }
}