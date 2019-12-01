using System;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019
{
    class Program
    {
        private const string Year = "2019";
        private const string InputFolder = "input";
        private const string Cookie = "53616c7465645f5f7523b578561c5a8265ec6de392f37d758a299aa5472c45ad65091a14972262a22956c118e5d8eb2c"; 
        // TODO: from config

        // TODO: asserter (maybe list of input/output in ISolution)
        // TODO: command line
        // TODO: separate downloader

        static void Main(string[] args)
        {
            var collector = new SolutionCollector();
            var inputGenerator = new InputFactory(Year, InputFolder, Cookie);

            var executor = collector.GetSolutionExecutor(1);

            //var input = inputGenerator.GetInputFromFile(1, "test");
            var input = inputGenerator.GetDefaultInput(1);

            executor.ExecuteFirstPart(input);
            executor.ExecuteSecondPart(input);
        }
    }
}