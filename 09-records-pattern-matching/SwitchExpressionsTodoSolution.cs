using CSharpExercises.InterfacesInheritance;

namespace CSharpExercises.RecordsPatternMatching;

// Reference implementation for the TODO half of SwitchExpressionsTodo.cs. Not wired
// into Program.cs - run it manually or diff it against your own attempt if stuck.
public static class SwitchExpressionsTodoSolution
{
    private static string Describe(Shape shape) => shape switch
    {
        Circle c => $"Circle, area {c.Area():F2}",
        Rectangle r => $"Rectangle, area {r.Area():F2}",
        _ => $"Unknown shape: {shape.Name}",
    };

    private static void RunTypePatternDemo()
    {
        Shape[] shapes = { new Circle(2), new Rectangle(3, 4) };

        foreach (var shape in shapes)
        {
            Console.WriteLine(Describe(shape));
        }
    }

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
