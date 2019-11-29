using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode.Executor
{
    public interface ISolutionExecutor
    {
        Task ExecuteFirstPartAsync(Input input);
        Task ExecuteSecondPartAsync(Input input);
    }

    public class SolutionExecutor : ISolutionExecutor
    {
        private readonly ISolution _solution;
        private readonly int _day;

        public SolutionExecutor(int day, ISolution solution)
        {
            _day = day;
            _solution = solution;
        }

        public async Task ExecuteFirstPartAsync(Input input)
        {
            await ExecuteAsync(_solution.SolveFirstPartAsync, input, 1);
        }

        public async Task ExecuteSecondPartAsync(Input input)
        {
            await ExecuteAsync(_solution.SolveSecondPartAsync, input, 2);
        }

        private async Task ExecuteAsync(Func<Input, Task<string>> solver, Input input, int part)
        {
            Console.WriteLine($"Starting solution for: Day {_day}, Part {part}");

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = await solver.Invoke(input);
            stopWatch.Stop();

            var elapsed = stopWatch.Elapsed;

            Console.WriteLine();
            Console.WriteLine($"Solved in {elapsed.Milliseconds} ms");
            Console.WriteLine($"Result: {result}");
        }
    }
}
