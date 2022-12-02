using AdventOfCode.Executor;

namespace AdventOfCode.Abstractions;

public abstract class SolutionBase : ISolution
{
    public abstract int Year { get; }
    public abstract int Day { get; }
    public virtual bool PartOneSolved { get; } = false;
    public virtual bool PartTwoSolved { get; } = false;

    public abstract string SolveFirstPart(Input input);

    public abstract string SolveSecondPart(Input input);
}