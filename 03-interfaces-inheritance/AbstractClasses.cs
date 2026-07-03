namespace CSharpExercises.InterfacesInheritance;

// Why this exists: an abstract class sits between a regular base class and an interface -
// it CAN hold state and shared implementation (like a normal base class), but can also
// declare abstract members that subclasses MUST implement (like an interface). Use an
// interface (see InterfacesBasics.cs) when unrelated types just need to share a contract
// with no shared state/logic. Use an abstract class when types share both a contract AND
// real state or behavior worth inheriting - here, every Shape has a Name and a Describe()
// implementation, not just an Area() signature.
public abstract class Shape
{
    public string Name { get; }

    protected Shape(string name)
    {
        Name = name;
    }

    public abstract double Area(); // no body - every subclass must supply one

    public void Describe() // shared implementation, not possible in a plain interface member
    {
        Console.WriteLine($"{Name} has area {Area():F2}");
    }
}

public class Circle : Shape
{
    private readonly double _radius;

    public Circle(double radius) : base("Circle")
    {
        _radius = radius;
    }

    public override double Area() => Math.PI * _radius * _radius;
}

public class Rectangle : Shape
{
    private readonly double _width;
    private readonly double _height;

    public Rectangle(double width, double height) : base("Rectangle")
    {
        _width = width;
        _height = height;
    }

    public override double Area() => _width * _height;
}

public static class AbstractClasses
{
    public static void Run()
    {
        Shape[] shapes = { new Circle(3), new Rectangle(4, 5) };

        foreach (var shape in shapes)
        {
            shape.Describe(); // shared method, inherited rather than reimplemented per shape
        }

        // var shape = new Shape("x"); // would NOT compile - CS0144: cannot create an instance
        // of an abstract class/interface. Both abstract classes and interfaces block direct
        // instantiation; only concrete subclasses/implementers can be constructed.
    }
}
