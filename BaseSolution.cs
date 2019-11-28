using System;
using System.Diagnostics;
using System.Threading.Tasks;
using adventOfCode.utilities;

namespace adventOfCode
{
    public abstract class BaseSolution
    {
        public TimeSpan Elapsed { get; private set; }

        public async Task<string> SolveAsync(Input input)
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            var result = await SolveCoreAsync(input);
            stopWatch.Stop();

            Elapsed = stopWatch.Elapsed;

            return result;
        }

        public abstract Task<string> SolveCoreAsync(Input input);
    }
}