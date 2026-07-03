namespace CSharpExercises.Generics;

// Why this exists: quick map from C#'s built-in generic collections to their closest JS
// equivalents, since the names don't line up 1:1.
public static class GenericCollectionsBuiltIn
{
    public static void Run()
    {
        // List<T> - closest to a JS Array: ordered, index-accessible, resizable.
        List<string> fruits = new() { "apple", "banana", "cherry" };
        fruits.Add("date");
        Console.WriteLine($"List<T>: [{string.Join(", ", fruits)}], fruits[1] = {fruits[1]}");

        // Dictionary<TKey, TValue> - closest to a JS Map (or a plain object used as a map),
        // but keys are strongly typed, not limited to strings/symbols.
        Dictionary<string, int> ages = new()
        {
            ["Alice"] = 30,
            ["Bob"] = 25,
        };
        ages["Carol"] = 40;
        Console.WriteLine($"Dictionary<TKey,TValue>: Alice is {ages["Alice"]}, contains Bob: {ages.ContainsKey("Bob")}");

        // HashSet<T> - closest to a JS Set: unique values, no guaranteed order, fast Contains.
        HashSet<int> uniqueIds = new() { 1, 2, 3 };
        var reAdded = uniqueIds.Add(2); // duplicate, ignored - Add returns false
        var addedNew = uniqueIds.Add(4);
        Console.WriteLine($"HashSet<T>: [{string.Join(", ", uniqueIds)}], re-adding 2 succeeded: {reAdded}, adding 4 succeeded: {addedNew}");
    }
}
