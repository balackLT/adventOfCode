using System.Threading.Tasks;
using AdventOfCode.Executor;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Solutions2019;

static class Program
{
    // TODO: command line
    // TODO: separate downloader

    static async Task Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();
            
        var collector = new SolutionCollector();
        var inputGenerator = new InputFactory(config["year"], config["inputFolder"], config["cookie"]);

        var day = 1;
            
        var executor = collector.GetSolutionExecutor(day);
            
        // var input = inputGenerator.GetInputFromFile(day, "test1");
        var input = await inputGenerator.GetDefaultInputAsync(day);

        executor.ExecuteFirstPart(input);
        executor.ExecuteSecondPart(input);
    }
}