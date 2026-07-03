namespace CSharpExercises.CollectionsLinq;

// Reference implementation for LinqAggregatesTodo.cs. Not wired into Program.cs -
// run it manually or diff it against your own attempt if stuck.
public static class LinqAggregatesTodoSolution
{
    private static readonly List<Order> Orders = new()
    {
        new Order("Books", 15.00m),
        new Order("Books", 22.50m),
        new Order("Electronics", 120.00m),
        new Order("Electronics", 45.00m),
        new Order("Groceries", 8.75m),
    };

    public static decimal TotalAmount()
    {
        return Orders.Sum(o => o.Amount);
    }

    public static int OrderCount()
    {
        return Orders.Count();
    }

    public static Dictionary<string, decimal> TotalByCategory()
    {
        return Orders
            .GroupBy(o => o.Category)
            .ToDictionary(g => g.Key, g => g.Sum(o => o.Amount));
    }

    public static string CategoryTrail()
    {
        return Orders
            .Select(o => o.Category)
            .Aggregate((acc, category) => $"{acc}, {category}");
    }

    public static void Run()
    {
        Console.WriteLine($"Total amount: {TotalAmount():C}");
        Console.WriteLine($"Order count: {OrderCount()}");
        Console.WriteLine("Total by category:");
        foreach (var (category, total) in TotalByCategory())
        {
            Console.WriteLine($"  {category}: {total:C}");
        }
        Console.WriteLine($"Category trail: {CategoryTrail()}");
    }
}
