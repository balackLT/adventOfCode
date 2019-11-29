using System.Threading.Tasks;

namespace AdventOfCode.Executor
{
    public class Input
    {
        private readonly string[] _lines;

        public Input(string[] lines)
        {
            _lines = lines;
        }

        public async Task<string[]> GetLinesAsync()
        {
            _lines = await System.IO.File.ReadAllLinesAsync(_path);
            return _lines;
        }

        private string ConstructPath()
        {
            var path = $"./solutions/{_year}/day{_day}/input/input.txt";

            return path;
        }

    }
}
