using System.Threading.Tasks;

namespace adventOfCode.utilities
{
    public class Input
    {
        private readonly string _path;
        private string _year;
        private string _day;
        private string _part;

        private string[] _lines;
        private readonly int _simpleInput;

        public Input(string solutionId)
        {
            ParseSolutionId(solutionId);

            _path = ConstructPath();
        }

        public Input(int integerInput)
        {
            _simpleInput = integerInput;
        }

        public static Input GetDefaultInput(string solutionId)
        {
            return new Input(solutionId);
        }

        public async Task<string[]> GetLinesAsync()
        {
            _lines = await System.IO.File.ReadAllLinesAsync(_path);
            return _lines;
        }

        public int GetInput()
        {
            return _simpleInput;
        }

        private string ConstructPath()
        {
            var path = $"./solutions/{_year}/day{_day}/input/";

            return path;
        }

        private void ParseSolutionId(string solutionId)
        {
            var splitId = solutionId.Split('-');
            _year = splitId[0];
            _day = splitId[1];
            _part = splitId[2];
        }
        }
}
