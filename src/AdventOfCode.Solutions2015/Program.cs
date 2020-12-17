using AdventOfCode.Executor;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Solutions2015
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

            var day = 11;
            
            var executor = collector.GetSolutionExecutor(day);
            
            // var input = inputGenerator.GetInputFromFile(day, "test1");
            var input = inputGenerator.GetDefaultInput(day);

            executor.ExecuteFirstPart(input);
            executor.ExecuteSecondPart(input);
        }
    }
}