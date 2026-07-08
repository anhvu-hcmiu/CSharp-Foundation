namespace CSharpExercises.TypesAndSyntax;

// Why this exists: JS is dynamically typed - a variable's type can change at runtime, and
// type mismatches usually surface as runtime bugs (or silent coercion). C# is statically
// typed - every variable has a fixed type checked at compile time, so most type mismatches
// are caught before the program ever runs. `var` does not weaken this; it just lets the
// compiler infer the type instead of you writing it out.
public static class StaticTyping
{
    public static void Run()
    {
        var count = 10; // inferred as int
        var name = "Alice"; // inferred as string
        Console.WriteLine($"count is {count.GetType().Name}, name is {name.GetType().Name}");

        // The following would NOT compile - uncomment to see the compiler catch it:
        // count = "not a number"; // error CS0029: cannot implicitly convert 'string' to 'int'
        // In JS, `count = "not a number"` would silently succeed and change count's type.

        // Boxing: converting a value type (int, on the stack conceptually) to `object`
        // (a reference type, on the heap). Unboxing is the reverse. Both cost a heap
        // allocation/copy, which is why boxing in hot loops is a common perf trap.
        object boxed = count; // boxing: int -> object
        var unboxed = (int)boxed; // unboxing: object -> int, must match the original type exactly
        Console.WriteLine($"boxed value: {boxed}, unboxed back: {unboxed}");

        try
        {
            var wrongUnbox = (double)boxed; // throws InvalidCastException at runtime -
            // unboxing requires an exact type match, unlike a normal C# numeric cast.
            Console.WriteLine($"unreachable: {wrongUnbox}");
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"Caught expected exception: {ex.Message}");
        }
    }
}
