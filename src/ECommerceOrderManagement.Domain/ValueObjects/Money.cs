namespace ECommerceOrderManagement.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; private init; }

    public string Currency { get; private init; } = string.Empty;

    private Money()
    {
    }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount),"Money amount cannot be negative.");
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        var normalizedCurrency = currency.Trim().ToUpperInvariant();

        if (normalizedCurrency.Length != 3)
        {
            throw new ArgumentException( "Currency code must contain three characters.", nameof(currency));   
        }

        return new Money(amount, normalizedCurrency);
    }
}