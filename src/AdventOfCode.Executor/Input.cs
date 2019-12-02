using System.Linq;

namespace AdventOfCode.Executor
{
    public class Input
    {
        private readonly string[] _lines;

        public Input(string[] lines)
        {
            _lines = lines;
        }

        public string[] GetLines()
        {
            return _lines;
        }

        public int[] GetLinesAsInt()
        {
            return _lines.Select(int.Parse).ToArray();
        }
        
        public int[] GetLineAsIntArray()
        {
            return _lines.First().Split(',').Select(int.Parse).ToArray();
        }
    }
}
