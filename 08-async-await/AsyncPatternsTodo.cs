using System.Diagnostics;

namespace CSharpExercises.AsyncAwait;

// TODO exercise: fill in the blanks below. Diff against AsyncPatternsTodoSolution.cs
// if stuck - that file isn't registered in the menu on purpose.
//
// JS comparison: sequential `await` calls here are like awaiting Promises one at a time
// in a for-loop; Task.WhenAll below is the C# equivalent of `Promise.all([...])` - both
// run the underlying work concurrently and wait for every one to finish.
public static class AsyncPatternsTodo
{
    private static async Task<int> SimulateFetchAsync(string label, int delayMs)
    {
        await Task.Delay(delayMs);
        return label.Length;
    }

    // Worked half: sequential awaits - each call fully completes before the next starts.
    // Total time is roughly the sum of all delays.
    private static async Task RunSequentialAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        var a = await SimulateFetchAsync("alpha", 150);
        var b = await SimulateFetchAsync("beta", 150);
        var c = await SimulateFetchAsync("gamma", 150);

        stopwatch.Stop();
        Console.WriteLine($"Sequential total lengths: {a + b + c}, elapsed: {stopwatch.ElapsedMilliseconds}ms");
    }

    // TODO: rewrite using Task.WhenAll so all three calls run concurrently. Start all
    // three tasks first (without awaiting yet), THEN await Task.WhenAll(...) - awaiting
    // each call individually before starting the next one defeats the purpose, same
    // mistake as writing sequential `await` in a loop instead of `Promise.all(...)` in JS.
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
