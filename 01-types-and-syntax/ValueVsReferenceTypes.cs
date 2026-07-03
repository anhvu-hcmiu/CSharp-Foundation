namespace CSharpExercises.TypesAndSyntax;

// Why this exists: JS has no value/reference distinction - every object is a reference,
// and primitives are immutable so copy-vs-reference never comes up. C# splits types into
// two categories with different assignment semantics: structs (value types) copy their
// data on assignment; classes (reference types) copy the pointer. This bites people who
// pass a struct expecting mutation to be visible to the caller, like passing an object in JS.
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class PointRef
{
    public int X { get; set; }
    public int Y { get; set; }
}

public static class ValueVsReferenceTypes
{
    public static void Run()
    {
        var original = new Point { X = 1, Y = 1 };
        var copy = original; // struct: this duplicates the data
        copy.X = 99;
        Console.WriteLine($"Struct (value type): original.X={original.X}, copy.X={copy.X} -> unaffected");

        var originalRef = new PointRef { X = 1, Y = 1 };
        var alias = originalRef; // class: this duplicates the reference, not the data
        alias.X = 99;
        Console.WriteLine($"Class (reference type): originalRef.X={originalRef.X}, alias.X={alias.X} -> same object");

        // var infers the compile-time type from the right-hand side; it is NOT dynamic typing.
        // Once inferred, the variable's type is fixed - unlike JS `let`, which can hold anything.
        var inferredInt = 42; // type is `int`, permanently
        Console.WriteLine($"var inferredInt has compile-time type: {inferredInt.GetType().Name}");
    }
}
