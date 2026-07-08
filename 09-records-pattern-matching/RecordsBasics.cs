using CSharpExercises.TypesAndSyntax;

namespace CSharpExercises.RecordsPatternMatching;

// Why this exists: JS has no built-in value-equality type - two object literals with the
// same fields are never `===`. C# records give you that for free: `==` on a record compares
// field values, not references. Contrast with `PointRef` (01-types-and-syntax/ValueVsReferenceTypes.cs),
// an ordinary class where `==` falls back to reference equality - same-looking objects are
// still "different" unless they're literally the same instance.
public record Point(double X, double Y);

public static class RecordsBasics
{
    public static void Run()
    {
        var a = new Point(1, 1);
        var b = new Point(1, 1);
        Console.WriteLine($"record Point: a == b -> {a == b} (compares X/Y values, not identity)");

        var refA = new PointRef { X = 1, Y = 1 };
        var refB = new PointRef { X = 1, Y = 1 };
        Console.WriteLine($"class PointRef: refA == refB -> {refA == refB} (same values, but different objects - reference equality)");

        // ToString() is also auto-generated for records and prints field values,
        // unlike a plain class which prints its type name by default.
        Console.WriteLine($"record ToString(): {a}");
        Console.WriteLine($"class ToString(): {refA}");
    }
}
