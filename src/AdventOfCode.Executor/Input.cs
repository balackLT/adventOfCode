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
            return _lines.Select(line => line.Split(',').ToList()).ToList();
        }
        
        public Dictionary<(int X, int Y), char> GetAsMap()
        {
            var inputMap = GetLines();
            
            var map = new Dictionary<(int X, int Y), char>();
            
            var y = 0;
            foreach (var lines in inputMap)
            {
                var x = 0;
                foreach (var pixel in lines)
                {
                    var coordinate = (x, y);
                    map[coordinate] = pixel;
                    
                    x++;
                }
                y++;
            }

            return map;
        }
    }
}
