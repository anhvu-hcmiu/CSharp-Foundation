using System.Text.Json.Serialization;

namespace PersonalLedger.Models;

public readonly record struct Money
{
    public decimal Amount { get; }

    [JsonConstructor]
    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Money amount cannot be negative.");
        }

        Amount = amount;
    }
}
