using System.Threading.Tasks;
using AdventOfCode.Executor;
using CommandLine;

namespace AdventOfCode.Solutions._2018
{
    class Program
    {
        private const string Year = "2018";
        private const string Cookie = "adsfdasf"; // TODO: from config

        static async Task Main(string[] args)
        {
            var collector = new SolutionCollector();
            var executor = collector.GetSolutionExecutor(1);

            // TODO: asserter (maybe list of input/output in ISolution)
            // TODO: input downloader
            // TODO: command line

            await executor.ExecuteFirstPartAsync();
            await executor.ExecuteSecondPartAsync();
        }
    }
}
