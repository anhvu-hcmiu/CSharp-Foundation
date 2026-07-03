namespace CSharpExercises.InterfacesInheritance;

// Why this exists: JS has no interfaces at all - "duck typing" (an object works if it has
// the right shape) is the closest equivalent, but nothing enforces or documents the
// contract. A C# interface is a named, compiler-enforced contract: a class that implements
// it MUST provide every member, and code can be written against the interface type instead
// of the concrete class - decoupling callers from a specific implementation. Unlike a base
// class, an interface carries no state and (here) no implementation - just the shape.
public interface IPayable
{
    decimal AmountDue { get; }
    void Pay(decimal amount);
}

public class Invoice : IPayable
{
    public decimal AmountDue { get; private set; }

    public Invoice(decimal amountDue)
    {
        AmountDue = amountDue;
    }

    public void Pay(decimal amount)
    {
        AmountDue -= amount;
        Console.WriteLine($"Invoice paid {amount:C}, remaining {AmountDue:C}");
    }
}

public class Subscription : IPayable
{
    public decimal AmountDue { get; private set; } = 9.99m;

    public void Pay(decimal amount)
    {
        AmountDue = Math.Max(0, AmountDue - amount);
        Console.WriteLine($"Subscription paid {amount:C}, remaining {AmountDue:C}");
    }
}

public static class InterfacesBasics
{
    public static void Run()
    {
        // Both classes are wildly different (one takes a constructor arg, one doesn't), but
        // code written against IPayable doesn't need to know or care which one it's holding.
        IPayable[] payables = { new Invoice(100m), new Subscription() };

        foreach (var payable in payables)
        {
            payable.Pay(5m);
        }
    }
}
