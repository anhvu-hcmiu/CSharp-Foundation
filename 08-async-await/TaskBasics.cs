namespace CSharpExercises.AsyncAwait;

// Why this exists: Task/async/await map onto JS Promise/async/await conceptually, but a
// few things are genuinely different - see inline comments below for each one.
public static class TaskBasics
{
    // Task<T> returned by an async method is "hot" the moment it's created (the method
    // body starts running immediately, up to the first await) - same as a JS Promise,
    // whose executor also runs synchronously the instant `new Promise(executor)` is called.
    // The one genuine difference: C# also has "cold" Tasks via `new Task(...)` (rare, and
    // NOT what `async` methods produce) that sit inert until an explicit `.Start()` call -
    // JS Promises have no equivalent inert/unstarted state.
    private static async Task<string> FetchUserNameAsync(int userId)
    {
        // Task.Delay simulates I/O latency, standing in for something like `await fetch(...)`.
        await Task.Delay(200);
        return $"user-{userId}";
    }

    private static async Task DemoBasicAwaitAsync()
    {
        Console.WriteLine("Fetching user name...");
        var name = await FetchUserNameAsync(42);
        Console.WriteLine($"Fetched: {name}");
    }

    // async Task (not void) - exceptions thrown inside propagate through the returned
    // Task and are catchable by whoever awaits it, same as a rejected JS Promise being
    // caught by `try { await p } catch {}`.
    private static async Task RiskyOperationAsync(bool shouldFail)
    {
        await Task.Delay(50);
        if (shouldFail)
        {
            throw new InvalidOperationException("Simulated failure inside async Task");
        }
    }

    private static async Task DemoCatchableExceptionAsync()
    {
        try
        {
            await RiskyOperationAsync(shouldFail: true);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Caught via awaited Task: {ex.Message}");
        }
    }

    // async void - the dangerous shape. There is no Task for the caller to await, so
    // there is nothing to attach a catch to. In JS, every async function returns a
    // Promise, so this failure mode literally cannot happen - `async void` has no
    // equivalent in JS at all. If this method throws and nothing inside it catches the
    // exception, the exception is raised directly on the SynchronizationContext (or, in
    // a plain console app, the thread pool) and typically crashes the process - a
    // try/catch wrapped around the *call site* below would NOT catch it.
    //
    // The only legitimate use for async void is a UI event handler (e.g. a button
    // Click handler), where the framework itself is the "caller" and can't await a
    // return value anyway. Everywhere else, use async Task.
    private static async void RiskyEventHandlerAsync()
    {
        try
        {
            await Task.Delay(10);
            throw new InvalidOperationException("Simulated failure inside async void");
        }
        catch (InvalidOperationException ex)
        {
            // Caught here, inside the method - this is the mitigation. Without this
            // internal try/catch, the exception above would NOT be catchable by any
            // caller and would likely terminate the process.
            Console.WriteLine($"async void must catch its own exceptions internally: {ex.Message}");
        }
    }

    public static void Run()
    {
        // Console apps have no SynchronizationContext, so blocking on .GetAwaiter().GetResult()
        // from a synchronous Run() is safe here (would risk deadlock in ASP.NET/UI contexts).
        DemoBasicAwaitAsync().GetAwaiter().GetResult();
        Console.WriteLine("after DemoBasicAwaitAsync");


        Console.WriteLine();
        DemoCatchableExceptionAsync().GetAwaiter().GetResult();
        Console.WriteLine();

        RiskyEventHandlerAsync();
        Console.WriteLine("after RiskyEventHandlerAsync");


        // async void is fire-and-forget - there is no Task here to await, so Run() has no
        // way to know when RiskyEventHandlerAsync() finishes. This sleep exists ONLY so the
        // demo's console output prints before this method returns; a real UI event handler
        // doesn't need this because it isn't racing the rest of Run() to produce output.
        Thread.Sleep(50);
    }
}
