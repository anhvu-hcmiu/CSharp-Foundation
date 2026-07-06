namespace CSharpExercises.DelegatesEvents;

// Reference implementation for the TODO half of CustomDelegatesAndEvents.cs. Not wired
// into Program.cs - run it manually or diff it against your own attempt if stuck.
public class OrderProcessorSolution
{
    public event OrderPlacedHandler? OrderPlaced;

    public void PlaceOrder(string customerName, decimal amount)
    {
        OrderPlaced?.Invoke(customerName, amount);
    }
}

public static class CustomDelegatesAndEventsSolution
{
    public static void Run()
    {
        var processor = new OrderProcessorSolution();

        processor.OrderPlaced += (customerName, amount) =>
            Console.WriteLine($"Notified: {customerName} ordered for {amount:C}");

        processor.PlaceOrder("Carol", 99.99m);
    }
}
