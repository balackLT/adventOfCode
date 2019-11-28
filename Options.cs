using CommandLine;

namespace adventOfCode
{
    public class Options
    {
        [Option('s', Required = true)]
        public string SolutionId { get; set; }

        [Option('f', "file", Default = "input")]
        public string InputFile { get; set; }

        [Option('i', "input", Required = false)]
        public int? Input { get; set; }
    }
}
