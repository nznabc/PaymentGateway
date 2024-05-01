using PaymentGateway.Api.Models;

using Polly;

namespace PaymentGateway.Api.Services;

internal class BankHttpClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

    public BankHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(5));
    }

    // Add the policy to the HttpClient
    public async Task<BankResponse?> PostPaymentDetailsAsync(PaymentDetailRequest paymentDetailRequest)
    {
        HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => _httpClient.PostAsJsonAsync("payments", paymentDetailRequest));

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<BankResponse>();
        }

        return null;
    }

    public void Dispose() => _httpClient?.Dispose();
}
