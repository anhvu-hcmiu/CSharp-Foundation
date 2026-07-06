using System.Diagnostics;

namespace CSharpExercises.AsyncAwait;

// Reference implementation for the TODO half of AsyncPatternsTodo.cs. Not wired
// into Program.cs - run it manually or diff it against your own attempt if stuck.
public static class AsyncPatternsTodoSolution
{
    private static async Task<int> SimulateFetchAsync(string label, int delayMs)
    {
        await Task.Delay(delayMs);
        return label.Length;
    }

    private static async Task RunSequentialAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        var a = await SimulateFetchAsync("alpha", 150);
        var b = await SimulateFetchAsync("beta", 150);
        var c = await SimulateFetchAsync("gamma", 150);

        stopwatch.Stop();
        Console.WriteLine($"Sequential total lengths: {a + b + c}, elapsed: {stopwatch.ElapsedMilliseconds}ms");
    }

    private static async Task RunParallelAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        var taskA = SimulateFetchAsync("alpha", 150);
        var taskB = SimulateFetchAsync("beta", 150);
        var taskC = SimulateFetchAsync("gamma", 150);

        var results = await Task.WhenAll(taskA, taskB, taskC);

        stopwatch.Stop();
        Console.WriteLine($"Parallel total lengths: {results.Sum()}, elapsed: {stopwatch.ElapsedMilliseconds}ms");
    }

    public static void Run()
    {
        RunSequentialAsync().GetAwaiter().GetResult();
        RunParallelAsync().GetAwaiter().GetResult();
    }
}
