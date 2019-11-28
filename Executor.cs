using System;
using System.Threading.Tasks;
using adventOfCode.utilities;

namespace adventOfCode
{
    public static class Executor
    {
        public static async Task ExecuteAsync(Options options)
        {
            //if (args.Length > 1)
            //{
            //    userInput = int.Parse(args[1]);
            //    input = new Input(userInput);
            //}
            //else
            //{
            //    input = Input.GetDefaultInput(solutionId);
            //}

            Console.WriteLine($"Starting solution for: {options.SolutionId}");
            var input = Input.GetDefaultInput(options.SolutionId);
            
            var solution = new solutions._2018.day01.Solution1();

            var result = await solution.SolveCoreAsync(input);

            Console.WriteLine();
            Console.WriteLine($"Solved in {solution.Elapsed.Milliseconds} ms");
            Console.WriteLine($"Result: {result}");
        }
    }
}
