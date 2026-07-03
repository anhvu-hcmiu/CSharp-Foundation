namespace CSharpExercises.OopCore;

using System.Text.Json;

// Why this exists: JS objects/classes just have plain fields you read/write directly (or
// fake getters/setters with #private + methods). C# properties are first-class syntax for
// that pattern - `get`/`set` accessors, auto-properties that skip writing the backing field
// yourself, computed (get-only) properties, and `init` for "settable only during
// construction/object-initializer, immutable after". `readonly` on a *field* is stricter
// than `private set` on a property: readonly can only be assigned in the constructor,
// while `private set` still allows the class's own methods to mutate it later - use
// readonly when nothing, not even internal code, should change the value post-construction.
public class Customer
{
    // readonly field: only assignable in the constructor or a field initializer (as below),
    // never after - use for values that must never change for the lifetime of the object
    // (e.g. a generated ID).
    public readonly Guid Id = Guid.NewGuid();

    // Auto-property: compiler generates the backing field for you.
    public string First { get; set; } = "";
    public string Last { get; set; }

    // Computed (get-only) property: recalculated every access, no backing field.
    public string FullName => $"{First} {Last}";

    // init-only property: settable via constructor or object initializer, immutable after -
    // stricter than `private set` (which still lets the class itself mutate it later).
    public string AccountNumber { get; init; }

    public Customer(string first, string last)
    {
        First = first;
        Last = last;
    }
}

public static class PropertiesAndFields
{
    public static void Run()
    {
        var customer = new Customer("Grace", "Hopper")
        {
            AccountNumber = "ACC-001", // allowed: object initializer runs during construction
        };

        var customer2 = new Customer("Alan", "Turing");

        Console.WriteLine($"{customer.FullName} (id={customer.Id}, acct={customer.AccountNumber})");

        Console.WriteLine($"{customer2.FullName} (id={customer2.Id}, acct={customer2.AccountNumber})");

        Console.WriteLine(JsonSerializer.Serialize(customer2));

        Console.WriteLine(customer2);

        customer.First = "Ada"; // allowed: regular auto-property
        Console.WriteLine($"After rename: {customer.FullName}");

        // customer.AccountNumber = "ACC-002"; // would NOT compile - init-only after construction
        // customer.Id = Guid.NewGuid();       // would NOT compile - readonly field after construction
    }
}
