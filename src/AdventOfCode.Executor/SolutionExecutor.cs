using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode.Executor;

public interface ISolutionExecutor
{
    void ExecuteFirstPart(Input input);
    void ExecuteSecondPart(Input input);
    Task ExecuteBothPartsAsync(Input input);
}

public class SolutionExecutor : ISolutionExecutor
{
    private readonly ISolution _solution;

    public SolutionExecutor(ISolution solution)
    {
        _solution = solution;
    }

    public async Task ExecuteBothPartsAsync(Input input)
    {
        ExecuteFirstPart(input);
        ExecuteSecondPart(input);
        
        // TODO: old solutions aren't stateless, so we can't run them in parallel
        // var firstPart = Task.Run(() => ExecuteFirstPart(input));
        // var secondPart = Task.Run(() => ExecuteSecondPart(input));
        
        // var tasks = new List<Task> {firstPart, secondPart};
        // await Task.WhenAll(tasks);

        await Task.CompletedTask;
    }
    
    public void ExecuteFirstPart(Input input)
    {
        Execute(_solution.SolveFirstPart, input, 1);
    }

    public void ExecuteSecondPart(Input input)
    {
        Execute(_solution.SolveSecondPart, input, 2);
    }

    private static void Execute(Func<Input, string> solver, Input input, int part)
    {
        var stopWatch = new Stopwatch();

        stopWatch.Start();
        var result = solver.Invoke(input);
        stopWatch.Stop();

        var elapsed = stopWatch.Elapsed;

        Console.WriteLine();
        Console.WriteLine($"Part {part} solved in {elapsed.Minutes}min, {elapsed.Seconds}s, {elapsed.TotalMilliseconds}ms");
        Console.WriteLine($"Result for part {part}: {result}");
        Console.WriteLine();
    }
}