using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Executor;

public class SolutionCollector
{
    private readonly IEnumerable<ISolution> _solutions;

    public SolutionCollector()
    {
        var callingAssembly = Assembly.GetCallingAssembly();

        _solutions = LoadSolutions(callingAssembly);
    }

    public ISolutionExecutor GetSolutionExecutor(int day)
    {
        var solution = _solutions.Single(s => s.Day == day);
        var executor = new SolutionExecutor(day, solution);

        return executor;
    }
        
    IEnumerable<ISolution> LoadSolutions(Assembly callingAssembly)
    {
        var baseType = typeof(ISolution);

        var solutions = callingAssembly
            .GetTypes()
            .Where(type => baseType.IsAssignableFrom(type));

        foreach(var solution in solutions) 
        {
            yield return (ISolution) Activator.CreateInstance(solution);
        }
    }
}