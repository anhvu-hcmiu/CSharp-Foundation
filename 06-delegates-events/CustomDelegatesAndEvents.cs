namespace CSharpExercises.DelegatesEvents;

// Custom delegate type - declares a method "shape" you can point variables/events at.
// JS has no equivalent declaration; any function matching the call signature just works.
public delegate void OrderPlacedHandler(string customerName, decimal amount);

// Worked half: a custom delegate invoked directly (no event, just a typed function pointer).
public static class CustomDelegatesAndEventsWorked
{
    private static void PrintReceipt(string customerName, decimal amount)
    {
        Console.WriteLine($"Receipt: {customerName} paid {amount:C}");
    }

    public static void RunWorked()
    {
        OrderPlacedHandler handler = PrintReceipt;
        handler("Alice", 42.50m);
    }
}

// TODO exercise: publisher raises an `event`, a subscriber method reacts to it. Diff against
// CustomDelegatesAndEventsSolution.cs if stuck - that file isn't registered in the menu on purpose.
//
// This is the pattern behind .NET UI/event-driven code (button clicks, etc.) and roughly maps
// to JS EventEmitter.on(...) / addEventListener, except the event member is declared on the
// publisher type itself instead of attached ad hoc.
public class OrderProcessorTodo
{
    // TODO: declare an event of type OrderPlacedHandler named OrderPlaced.
    // JS equivalent: this.emitter = new EventEmitter() (but here the event lives directly on the class)
    public event OrderPlacedHandler? OrderPlaced;

    // TODO: raise OrderPlaced with the given arguments - guard against zero subscribers first.
    // JS equivalent: this.emitter.emit('orderPlaced', customerName, amount)
    public void PlaceOrder(string customerName, decimal amount)
    {
        if (OrderPlaced is not null)
        {
            OrderPlaced(customerName, amount);
        }

        // Idiomatic one-liner using the null-conditional operator:
        // OrderPlaced?.Invoke(customerName, amount);
    }
}

public static class CustomDelegatesAndEventsTodo
{
    // TODO: subscribe a handler to processor.OrderPlaced that prints
    // "Notified: {customerName} ordered for {amount:C}", then call PlaceOrder once.
    public static void RunTodo()
    {
        var processor = new OrderProcessorTodo();

        processor.OrderPlaced += (customerName, amount) =>
            Console.WriteLine($"Notified: {customerName} ordered for {amount:C}");

        processor.PlaceOrder("Carol", 99.99m);
    }
}

public static class CustomDelegatesAndEvents
{
    public static void Run()
    {
        CustomDelegatesAndEventsWorked.RunWorked();
        Console.WriteLine();
        CustomDelegatesAndEventsTodo.RunTodo();
    }
}
