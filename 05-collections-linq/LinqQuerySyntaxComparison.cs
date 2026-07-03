namespace CSharpExercises.CollectionsLinq;

// Why this exists: query syntax looks like SQL and is sugar over method syntax - the compiler
// translates it into the same method calls under the hood. Real-world C# (including ASP.NET
// Core / EF Core code) is dominated by method syntax; query syntax mostly shows up for
// genuinely complex joins where it reads more clearly. One comparison is enough here - don't
// over-invest in query syntax fluency.
public static class LinqQuerySyntaxComparison
{
    public static void Run()
    {
        List<Person> people = new()
        {
            new Person("Alice", 30),
            new Person("Bob", 25),
            new Person("Carol", 35),
            new Person("Dave", 25),
        };

        // Method syntax - what you'll actually write day to day.
        var methodResult = people
            .Where(p => p.Age >= 30)
            .OrderBy(p => p.Name)
            .Select(p => p.Name);

        // Query syntax - same query, SQL-like shape. Compiles down to the same Where/OrderBy/Select calls.
        var queryResult =
            from p in people
            where p.Age >= 30
            orderby p.Name
            select p.Name;

        Console.WriteLine($"Method syntax:  {string.Join(", ", methodResult)}");
        Console.WriteLine($"Query syntax:   {string.Join(", ", queryResult)}");
    }
}
