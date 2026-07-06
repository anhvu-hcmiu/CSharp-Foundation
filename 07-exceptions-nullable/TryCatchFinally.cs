namespace CSharpExercises.ExceptionsNullable;

// Custom exception: subclass Exception (or a more specific built-in subclass), same idea
// as JS `class InsufficientFundsError extends Error {}`. Constructors just forward to base.
public class InsufficientFundsException : Exception
{
    public decimal Shortfall { get; }

    public InsufficientFundsException(string message, decimal shortfall) : base(message)
    {
        Shortfall = shortfall;
    }
}

// Why this exists: try/catch/finally reads almost like JS, but catch blocks are typed and
// ordered most-specific-to-least-specific (the compiler errors if a general catch comes
// before a more specific one - JS has no equivalent since `catch (e)` only ever catches "any").
public static class TryCatchFinally
{
    private static void Withdraw(decimal balance, decimal amount)
    {
        if (amount > balance)
        {
            throw new InsufficientFundsException("Withdrawal exceeds balance", amount - balance);
        }

        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative");
        }

        Console.WriteLine($"Withdrew {amount:C}, remaining balance {balance - amount:C}");
    }

    private static void DemoOrderedCatch(decimal balance, decimal amount)
    {
        try
        {
            // Nested try: an inner operation that might itself throw before the outer
            // withdrawal logic runs. JS equivalent: nested try/catch blocks work the same way.
            try
            {
                Withdraw(balance, amount);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Inner catch - invalid amount: {ex.Message}");
                throw; // rethrow preserves the original stack trace, unlike `throw ex;`
            }
        }
        // Most specific first: InsufficientFundsException is caught before its base Exception.
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Caught InsufficientFundsException: {ex.Message} (short by {ex.Shortfall:C})");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Caught ArgumentOutOfRangeException: {ex.Message}");
        }
        // General catch-all must come last, or the compiler rejects the ordering.
        catch (Exception ex)
        {
            Console.WriteLine($"Caught general Exception: {ex.Message}");
        }
        finally
        {
            // finally always runs - success, caught exception, or rethrown exception.
            // JS equivalent: identical `finally` semantics.
            Console.WriteLine("finally: withdrawal attempt complete");
        }
    }

    public static void Run()
    {
        Console.WriteLine("-- Successful withdrawal --");
        DemoOrderedCatch(100m, 30m);

        Console.WriteLine();
        Console.WriteLine("-- Insufficient funds --");
        DemoOrderedCatch(100m, 150m);

        Console.WriteLine();
        Console.WriteLine("-- Negative amount --");
        DemoOrderedCatch(100m, -5m);
    }
}
