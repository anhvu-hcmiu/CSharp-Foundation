namespace CSharpExercises.ExceptionsNullable;

// Reference implementation for the TODO half of NullableReferenceTypes.cs. Not wired
// into Program.cs - run it manually or diff it against your own attempt if stuck.
public static class NullableReferenceTypesSolution
{
    public static void PrintWelcome(string? customerName)
    {

        if (customerName is null)
        {
            throw new ArgumentNullException(nameof(customerName));
        }

        Console.WriteLine($"Welcome, {customerName}!");
    }

    public static void Run()
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
