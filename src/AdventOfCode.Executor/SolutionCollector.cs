using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Executor;

public class SolutionCollector
{
    private readonly IDictionary<int, ISolution> _solutions = new Dictionary<int, ISolution>();

    public SolutionCollector()
    {
        var callingAssembly = Assembly.GetCallingAssembly();

        LoadSolutions(callingAssembly);
    }

    public ISolutionExecutor GetSolutionExecutor(int day)
    {
        var solution = _solutions[day];
        var executor = new SolutionExecutor(solution);

        return executor;
    }

    private void LoadSolutions(Assembly callingAssembly)
    {
        var baseType = typeof(ISolution);

        var solutions = callingAssembly
            .GetTypes()
            .Where(type => baseType.IsAssignableFrom(type));

        foreach(var solution in solutions) 
        {
            var split = solution.Namespace!.Split('.');
            var year = split[1][^4..];
            var day = int.Parse(split[2][^2..]);
            _solutions[day] = (ISolution) Activator.CreateInstance(solution);
        }
    }
}