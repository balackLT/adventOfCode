using System.Threading.Tasks;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions._2018.day01
{
    public class Solution : ISolution
    {
        public int Day { get; } = 1;

        public async Task<string> SolveFirstPartAsync(Input input)
        {
            string[] lines = await input.GetLinesAsync();

            var result = 0;

            foreach (string line in lines)
            {
                if (line[0] == '+')
                {
                    result += int.Parse(line.Substring(1));
                }
                else
                {
                    result -= int.Parse(line.Substring(1));
                }
            }

            return result.ToString();
        }

        public async Task<string> SolveSecondPartAsync(Input input)
        {
            throw new System.NotImplementedException();
        }
    }
}
