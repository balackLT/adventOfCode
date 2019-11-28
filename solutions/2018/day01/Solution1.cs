using System.Threading.Tasks;
using adventOfCode.utilities;

namespace adventOfCode.solutions._2018.day01
{
    class Solution1 : BaseSolution
    {
        public override async Task<string> SolveCoreAsync(Input input)
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
    }
}
