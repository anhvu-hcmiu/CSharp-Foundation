namespace CSharpExercises.OopCore;

// Why this exists: JS classes have exactly one constructor path (you branch on arguments
// inside it). C# lets you declare multiple constructor overloads and have one call another
// via `: this(...)` to avoid duplicating initialization logic - closer to default parameters
// plus explicit delegation than anything JS classes offer natively.
public class BankAccount
{
    public string Owner { get; }
    public decimal Balance { get; private set; }

    public BankAccount(string owner, decimal openingBalance)
    {
        Owner = owner;
        Balance = openingBalance;
        Console.WriteLine($"Created account for {Owner} with balance {Balance:C}");
    }

    // Chains to the constructor above, supplying a default opening balance.
    // Runs the other constructor's body first, then nothing extra here.
    public BankAccount(string owner) : this(owner, 0m)
    {
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
}

public static class ClassesAndConstructors
{
    public static void Run()
    {
        var funded = new BankAccount("Alice", 100m);
        var fresh = new BankAccount("Bob"); // uses the chained constructor, opens at 0

        fresh.Deposit(25m);
        Console.WriteLine($"{funded.Owner}: {funded.Balance} hehehe");
        Console.WriteLine($"{fresh.Owner}: {fresh.Balance:C}");
    }
}
