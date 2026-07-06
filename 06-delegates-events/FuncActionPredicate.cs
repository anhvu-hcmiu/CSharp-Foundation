namespace CSharpExercises.DelegatesEvents;

// Why this exists: C# doesn't just pass functions as values like JS does implicitly -
// it has named delegate types for the common shapes. Func/Action/Predicate are the
// built-in generic delegates so you rarely need to declare a custom `delegate` type.
public static class FuncActionPredicate
{
    // Func<T1, T2, TResult>: takes T1, T2, returns TResult. Last type param is always the return type.
    // JS equivalent: const add = (a, b) => a + b;
    private static int Add(int a, int b) => a + b;

    // Action<T>: takes T, returns void (no return-type slot at all).
    // JS equivalent: const log = (msg) => console.log(msg);
    private static void Log(string message) => Console.WriteLine($"[log] {message}");

    // Predicate<T>: takes T, returns bool. Equivalent to Func<T, bool> - exists mostly
    // for historical reasons (List<T>.Find/FindAll use it) and for a bool-returning name.
    // JS equivalent: const isEven = (n) => n % 2 === 0;
    private static bool IsEven(int n) => n % 2 == 0;

    // Accepting a Func as a parameter - same idea as JS higher-order functions,
    // just with an explicit type instead of duck-typed "any callable".
    private static int ApplyOperation(int a, int b, Func<int, int, int> operation)
    {
        return operation(a, b);
    }

    private static void ForEachMatching(List<int> numbers, Predicate<int> predicate, Action<int> onMatch)
    {
        foreach (var number in numbers)
        {
            if (predicate(number))
            {
                onMatch(number);
            }
        }
    }

    public static void Run()
    {
        Func<int, int, int> add = Add;
        Console.WriteLine($"Func<int,int,int> Add(2, 3) = {add(2, 3)}");
        Console.WriteLine($"ApplyOperation with Add: {ApplyOperation(4, 5, Add)}");
        Console.WriteLine($"ApplyOperation with lambda: {ApplyOperation(4, 5, (a, b) => a * b)}");

        Action<string> log = Log;
        log("Action<string> invoked directly");

        Predicate<int> isEven = IsEven;
        Console.WriteLine($"Predicate<int> IsEven(4) = {isEven(4)}, IsEven(5) = {isEven(5)}");

        var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        Console.WriteLine("Even numbers via ForEachMatching:");
        ForEachMatching(numbers, IsEven, n => Console.WriteLine($"  {n}"));
    }
}
