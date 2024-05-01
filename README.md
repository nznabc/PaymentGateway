# Payment Gateway API

This project is a simple .Net version of the Payment Gateway API that allows processing payments between banks. It includes validation of payment details, communication with the bank, and mapping of responses.

## Structure
```
src/
    PaymentGateway.Api - ASP.NET Core Web API 
test/
    PaymentGateway.Api.Tests - xUnit test project. Contains unit tests for the payment service.
imposters/ - contains the bank simulator configuration. Don't change this

.editorconfig - It ensures a consistent set of rules for submissions when reformatting code
docker-compose.yml - configures the bank simulator
PaymentGateway.sln
```