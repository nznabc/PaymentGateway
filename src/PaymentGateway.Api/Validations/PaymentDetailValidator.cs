using FluentValidation;

using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Validations;

public class PaymentDetailValidator : AbstractValidator<PaymentDetail>
{
    private static readonly List<string> ValidCurrencyCodes = new List<string> { "USD", "EUR", "GBP" };

    public PaymentDetailValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required")
            .Length(14, 19).WithMessage("Card number must be between 14 and 19 digits")
            .Must(BeNumeric).WithMessage("Card number must be numeric");

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty().WithMessage("Expiry month is required")
            .InclusiveBetween(1, 12).WithMessage("Expiry Month must be between 1 and 12");

        RuleFor(x => x.ExpiryYear)
            .NotEmpty().WithMessage("Expiry year is required")
            .Must(BeFutureYear).WithMessage("Expiry year must be in the future");

        RuleFor(x => x)
            .Must(BeFutureDate).WithMessage("The combination of expiry month and year must be in the future");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).MaximumLength(3)
            .Must(BeValidCurrency).WithMessage("Currency must be a valid ISO currency code");

        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .Must(BeWholeNumber).WithMessage("Amount must be a whole number representing the minor currency unit");

        RuleFor(x => x.CVV)
            .NotEmpty().WithMessage("CVV is required")
            .Length(3, 4).WithMessage("CVV must be between 3 and 4 digits")
            .Must(BeNumeric).WithMessage("CVV must be numeric"); ;
    }

    private bool BeNumeric(string cardNumber)
    {
        return long.TryParse(cardNumber, out _);
    }

    private bool BeFutureYear(int expiryYear)
    {
        int currentYear = DateTime.Now.Year;
        return expiryYear > currentYear;
    }

    private bool BeFutureDate(PaymentDetail paymentDetail)
    {
        var currentYear = DateTime.Now.Year;
        var currentMonth = DateTime.Now.Month;

        if (paymentDetail.ExpiryYear > currentYear)
        {
            return true;
        }

        if (paymentDetail.ExpiryYear == currentYear && paymentDetail.ExpiryMonth > currentMonth)
        {
            return true;
        }

        return false;
    }

    private bool BeValidCurrency(string currency)
    {
        return ValidCurrencyCodes.Contains(currency);
    }

    private bool BeWholeNumber(decimal amount)
    {
        return amount % 1 == 0;
    }

}
