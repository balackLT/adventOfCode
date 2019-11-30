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
    }
}
