namespace CSharpExercises.CollectionsLinq;

public record Order(string Category, decimal Amount);

// TODO exercise: fill in each method below (replace the throw). Diff against
// LinqAggregatesTodoSolution.cs if stuck - that file isn't registered in the menu on purpose.
public static class LinqAggregatesTodo
{
    private static readonly List<Order> Orders = new()
    {
        new Order("Books", 15.00m),
        new Order("Books", 22.50m),
        new Order("Electronics", 120.00m),
        new Order("Electronics", 45.00m),
        new Order("Groceries", 8.75m),
    };

    // TODO: return the total of all Amount values.
    // JS equivalent: orders.reduce((sum, o) => sum + o.Amount, 0)
    public static decimal TotalAmount()
    {
        return Orders.Sum(o => o.Amount);
    }

    // TODO: return how many orders there are.
    // JS equivalent: orders.length
    public static int OrderCount()
    {
        return Orders.Count();
    }

    // TODO: group orders by Category, return a Dictionary<string, decimal> of category -> total amount.
    // JS equivalent: orders.reduce building a plain object keyed by category
    public static Dictionary<string, decimal> TotalByCategory()
    {

        var totals = new Dictionary<string, decimal>();

        foreach (var order in Orders)
        {
            if (totals.ContainsKey(order.Category))
            {
                totals[order.Category] += order.Amount;
            }
            else
            {
                totals[order.Category] = order.Amount;
            }
        }

        return totals;


        // return Orders.GroupBy(o => o.Category)
        //             .ToDictionary(g => g.Key, g => g.Sum(o => o.Amount));
    }

    // TODO: use Aggregate to build a single comma-separated string of all categories, in order
    // (duplicates included, not deduped).
    // JS equivalent: orders.map(o => o.Category).reduce((acc, c) => `${acc}, ${c}`)
    public static string CategoryTrail()
    {
        return Orders.Select(o => o.Category)
                     .Aggregate((acc, c) => $"{acc}, {c}");
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
