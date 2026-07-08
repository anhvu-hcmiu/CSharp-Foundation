namespace CSharpExercises.RecordsPatternMatching;

// Why this exists: records are designed to be immutable by default (positional properties
// are get-only). Instead of mutating one in place, you produce a modified copy with a
// `with` expression - closest JS equivalent is `{ ...original, field: newValue }` on a
// frozen object, except the compiler enforces the "don't mutate the original" part for you.
public record OrderLine(string Sku, int Quantity, decimal UnitPrice);

public static class RecordsWithMutation
{
    public static void Run()
    {
        var original = new OrderLine("SKU-100", 2, 9.99m);
        var updated = original with { Quantity = 5 };

        Console.WriteLine($"original: {original}");
        Console.WriteLine($"updated:  {updated}");
        Console.WriteLine($"original.Quantity unchanged -> {original.Quantity}");

        // original.Quantity = 5; // would NOT compile - CS8852: positional property is init-only.
    }
}
