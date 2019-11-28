using System;
using System.Threading.Tasks;
using adventOfCode.utilities;
using CommandLine;

namespace adventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(async o => await Executor.ExecuteAsync(o));
        }
    }
}
