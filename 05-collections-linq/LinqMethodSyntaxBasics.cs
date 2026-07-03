namespace CSharpExercises.CollectionsLinq;

public record Person(string Name, int Age);

// Why this exists: LINQ method syntax maps almost directly onto JS Array methods - same idea
// (chainable, non-mutating transformations), different names for a couple of them.
public static class LinqMethodSyntaxBasics
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

        var result = people
            .Where(p => p.Age >= 30)   // same as JS .filter()
            .Select(p => p.Name)       // same as JS .map()
            .OrderBy(name => name);    // same as JS .sort() (but non-mutating, unlike Array.sort)

        Console.WriteLine($"Adults 30+, sorted by name: {string.Join(", ", result)}");
    }
}
