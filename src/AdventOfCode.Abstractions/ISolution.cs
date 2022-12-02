namespace AdventOfCode.Executor;

public interface ISolution
{
    int Year { get; }
    int Day { get; }
    bool PartOneSolved { get; }
    bool PartTwoSolved { get; }
    string SolveFirstPart(Input input);
    string SolveSecondPart(Input input);
}