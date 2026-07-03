namespace CSharpExercises.Generics;

// Why this exists: JS functions accept anything and only fail at runtime if a comparison
// doesn't make sense. C# generics are checked at COMPILE TIME - the compiler doesn't know
// what `T` will be, so `a > b` is illegal unless you tell it every possible T supports it.
// That's what a generic constraint (`where T : ...`) does: it narrows the unknown type down
// to "anything implementing this interface," at which point `a.CompareTo(b)` becomes legal.
public static class GenericMethods
{
    // Constrained: T must implement IComparable<T>, so CompareTo is guaranteed to exist.
    // An UNCONSTRAINED version - `public static T Max<T>(T a, T b) => a > b ? a : b;` - fails
    // to compile with "Operator '>' cannot be applied to operands of type 'T' and 'T'" because
    // the compiler has no idea what T is at compile time and can't assume it supports `>`.
    public static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }

    public static void Run()
    {
        Console.WriteLine($"Max(3, 7) = {Max(3, 7)}");
        Console.WriteLine($"Max(\"apple\", \"banana\") = {Max("apple", "banana")}");
        Console.WriteLine($"Max(3.5, 2.1) = {Max(3.5, 2.1)}");
    }
}
