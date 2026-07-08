using CSharpExercises.InterfacesInheritance;

namespace CSharpExercises.RecordsPatternMatching;

// TODO exercise: fill in the blanks below. Diff against SwitchExpressionsTodoSolution.cs
// if stuck - that file isn't registered in the menu on purpose.
//
// Why this exists: JS has no switch EXPRESSION (only switch statements with fall-through
// and no return value) and no type/property pattern matching - the closest you get is a
// chain of `if (x instanceof Foo)` checks. C# pattern matching lets a single expression
// branch on both an object's runtime TYPE and its property VALUES, and the compiler warns
// if a match arm is unreachable or a case is missing.
public static class SwitchExpressionsTodo
{
    // Worked half - type pattern: `Circle c` matches when shape's runtime type is Circle AND
    // binds it to `c` in the same step - no separate cast needed, unlike
    // `if (shape is Circle) { var c = (Circle)shape; }`
    private static string Describe(Shape shape) => shape switch
    {
        Circle c => $"Circle, area {c.Area():F2}",
        Rectangle r => $"Rectangle, area {r.Area():F2}",
        _ => $"Unknown shape: {shape.Name}", // discard pattern - required for exhaustiveness
    };

    private static void RunTypePatternDemo()
    {
        Shape[] shapes = { new Circle(2), new Rectangle(3, 4) };

        foreach (var shape in shapes)
        {
            Console.WriteLine(Describe(shape));
        }
    }

    // TODO: classify a Point (see RecordsBasics.cs) into a quadrant using PROPERTY patterns
    // on X and Y, instead of an if/else chain like:
    //   if (point.X > 0 && point.Y > 0) return "Quadrant I";
    //   else if (point.X < 0 && point.Y > 0) return "Quadrant II";
    //   ... etc.
    // `{ X: > 0, Y: > 0 }` matches an object whose X property is > 0 AND Y property is > 0,
    // checked directly in the pattern - no if/else, no intermediate booleans.
    private static string Quadrant(Point point) => point switch
    {
        { X: 0, Y: 0 } => "Origin",
        { X: > 0, Y: > 0 } => "Quadrant I",
        { X: < 0, Y: > 0 } => "Quadrant II",
        { X: < 0, Y: < 0 } => "Quadrant III",
        { X: > 0, Y: < 0 } => "Quadrant IV",
        _ => "On an axis",
    };

    private static void RunPropertyPatternDemo()
    {
        Point[] points = { new(0, 0), new(2, 3), new(-2, 3), new(-2, -3), new(2, -3), new(5, 0) };

        foreach (var point in points)
        {
            Console.WriteLine($"{point} -> {Quadrant(point)}");
        }
    }

    public static void Run()
    {
        RunTypePatternDemo();
        Console.WriteLine();
        RunPropertyPatternDemo();
    }
}
