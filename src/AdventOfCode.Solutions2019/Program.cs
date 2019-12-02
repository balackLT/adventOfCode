using AdventOfCode.Executor;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Solutions2019
{
    static class Program
    {
        // TODO: asserter (maybe list of input/output in ISolution)
        // TODO: command line
        // TODO: separate downloader

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            
            var collector = new SolutionCollector();
            var inputGenerator = new InputFactory(config["year"], config["inputFolder"], config["cookie"]);

            var executor = collector.GetSolutionExecutor(2);

            //var input = inputGenerator.GetInputFromFile(1, "test");
            var input = inputGenerator.GetDefaultInput(2);

            executor.ExecuteFirstPart(input);
            executor.ExecuteSecondPart(input);
        }
    }
}