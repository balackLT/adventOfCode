using System.Threading.Tasks;

namespace AdventOfCode.Executor
{
    public interface ISolution
    {
        int Day { get; }
        Task<string> SolveFirstPartAsync(Input input);
        Task<string> SolveSecondPartAsync(Input input);
    }
}
