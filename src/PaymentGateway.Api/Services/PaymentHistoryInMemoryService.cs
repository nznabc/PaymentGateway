using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services.Interfaces;

namespace PaymentGateway.Api.Services;

internal class PaymentHistoryInMemoryService(PaymentResponseList paymentResponses) : IPaymentHistoryInMemoryService
{
    public PaymentResponse? GetPaymentHistory(Guid paymentDetailId)
    {
        return paymentResponses.PaymentResponses.FirstOrDefault(x => x.Id == paymentDetailId);
    }

    public void SavePaymentHistory(PaymentResponse paymentResponse)
    {
        paymentResponses.PaymentResponses.Add(paymentResponse);
    }
}

public class PaymentResponseList
{
    public List<PaymentResponse> PaymentResponses { get; set; } = new List<PaymentResponse>();
}
