namespace AdventOfCode.Executor
{
    public interface ISolution
    {
        int Day { get; }
        string SolveFirstPart(Input input);
        string SolveSecondPart(Input input);
    }
}
