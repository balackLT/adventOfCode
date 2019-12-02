using System;
using System.Diagnostics;

namespace AdventOfCode.Executor
{
    public interface ISolutionExecutor
    {
        void ExecuteFirstPart(Input input);
        void ExecuteSecondPart(Input input);
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

        public void ExecuteFirstPart(Input input)
        {
            Execute(_solution.SolveFirstPart, input, 1);
        }

        public void ExecuteSecondPart(Input input)
        {
            Execute(_solution.SolveSecondPart, input, 2);
        }

        private void Execute(Func<Input, string> solver, Input input, int part)
        {
            Console.WriteLine($"Starting solution for: Day {_day}, Part {part}:");

            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = solver.Invoke(input);
            stopWatch.Stop();

            var elapsed = stopWatch.Elapsed;

            Console.WriteLine();
            Console.WriteLine($"Solved in {elapsed.Milliseconds} ms");
            Console.WriteLine($"Result: {result}");
            Console.WriteLine();
        }
    }
}
