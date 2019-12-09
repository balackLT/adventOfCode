using System.Collections.Generic;
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
        
        public List<string> GetLinesAsList()
        {
            return _lines.ToList();
        }

        public string GetAsString()
        {
            return _lines.First();
        }

        public int[] GetLinesAsInt()
        {
            return _lines.Select(int.Parse).ToArray();
        }
        
        public int[] GetLineAsIntArray()
        {
            return _lines.First().Split(',').Select(int.Parse).ToArray();
        }
        
        public long[] GetLineAsLongArray()
        {
            return _lines.First().Split(',').Select(long.Parse).ToArray();
        }

        public List<List<string>> GetLinesAsLists()
        {
            var result = new List<List<string>>();
            
            foreach (var line in _lines)
            {
                var list = line.Split(',').ToList();
                result.Add(list);
            }

            return result;
        }
    }
}
