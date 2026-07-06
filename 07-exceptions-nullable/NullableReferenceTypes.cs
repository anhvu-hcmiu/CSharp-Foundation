namespace CSharpExercises.ExceptionsNullable;


// Why this exists: with <Nullable>enable</Nullable> (set project-wide in this repo),
// `string` and `string?` are different types - the compiler warns if you might be
// dereferencing null. JS has no such distinction; every value can be null/undefined
// and you find out at runtime.
public static class NullableReferenceTypesWorked
{
    public record Customer(string Name, string? PhoneNumber);

    public static void RunWorked()
    {
        var withPhone = new Customer("Alice", "555-0100");
        var withoutPhone = new Customer("Bob", null);

        // ?. - null-conditional: short-circuits to null instead of throwing.
        // Dereferencing PhoneNumber directly without ?. here would trigger CS8602
        // ("dereference of a possibly null reference") because the compiler tracks that
        // Customer.PhoneNumber is string? and hasn't been null-checked - confirmed by
        // temporarily writing `withoutPhone.PhoneNumber.Length` and rebuilding.
        // JS equivalent: customer.phoneNumber?.length
        Console.WriteLine($"Alice phone length: {withPhone.PhoneNumber?.Length}");
        Console.WriteLine($"Bob phone length: {withoutPhone.PhoneNumber?.Length}");

        // ?? - null-coalescing: fall back to a default when the left side is null.
        // JS equivalent: customer.phoneNumber ?? "no phone on file"
        Console.WriteLine($"Alice phone: {withPhone.PhoneNumber ?? "no phone on file"}");
        Console.WriteLine($"Bob phone: {withoutPhone.PhoneNumber ?? "no phone on file"}");

        // ??= - null-coalescing assignment: assign only if currently null.
        string? nickname = null;
        nickname ??= "Anonymous";
        Console.WriteLine($"Nickname: {nickname}");
    }
}

// TODO exercise: fill in the blanks below. Diff against NullableReferenceTypesSolution.cs
// if stuck - that file isn't registered in the menu on purpose.
public static class NullableReferenceTypesTodo
{
    // TODO: throw ArgumentNullException if customerName is null, using the ArgumentNullException.ThrowIfNull
    // helper (shorter than `if (x is null) throw new ArgumentNullException(nameof(x));`).
    // JS has no equivalent guard - a null customerName would just silently become the
    // string "null" in a template literal instead of failing fast.
    public static void PrintWelcome(string? customerName)
    {
        if (customerName is null)
        {
            throw new ArgumentNullException(nameof(customerName));
        }

        Console.WriteLine($"Welcome, {customerName}!");
    }

    public static void RunTodo()
    {
        PrintWelcome("Carol");

        try
        {
            PrintWelcome(null);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"Caught expected exception: {ex.Message}");
        }
    }
}

public static class NullableReferenceTypes
{
    public static void Run()
    {
        NullableReferenceTypesWorked.RunWorked();
        Console.WriteLine();
        NullableReferenceTypesTodo.RunTodo();
    }
}
