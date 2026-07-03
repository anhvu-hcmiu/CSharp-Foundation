namespace CSharpExercises.CollectionsLinq;

// Why this exists: JS .map()/.filter() run immediately and return a new array. LINQ method
// syntax (aside from a few terminal methods like ToList/Count/Sum) builds a QUERY, not a
// result - nothing runs until you enumerate it (foreach, ToList(), etc.). Enumerate the same
// query twice and it re-runs against whatever the source collection looks like AT THAT MOMENT.
public static class LinqDeferredExecution
{
    public static void Run()
    {
        List<int> numbers = new() { 1, 2, 3 };

        // No filtering happens on this line - `query` is a recipe, not a result.
        IEnumerable<int> query = numbers.Where(n => n > 1);

        Console.WriteLine("First enumeration (source is [1, 2, 3]):");
        foreach (var n in query)
        {
            Console.WriteLine($"  {n}");
        }

        numbers.Add(10);
        numbers.Add(20);

        Console.WriteLine("Second enumeration, same query object, source now [1, 2, 3, 10, 20]:");
        foreach (var n in query)
        {
            Console.WriteLine($"  {n}"); // includes 10 and 20 - the query re-ran, it didn't cache
        }

        // Force eager evaluation with ToList() when you want a JS-.map()-like snapshot instead.
        var snapshot = numbers.Where(n => n > 1).ToList();
        numbers.Add(30);
        Console.WriteLine($"Snapshot taken before adding 30, unaffected: [{string.Join(", ", snapshot)}]");
    }
}
