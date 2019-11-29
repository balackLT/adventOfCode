using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Executor
{
    public class SolutionCollector
    {
        private readonly IEnumerable<ISolution> _solutions;

        public SolutionCollector()
        {
            _solutions = LoadSolutions();
        }

        public ISolutionExecutor GetSolutionExecutor(int day)
        {
            var solution = _solutions.Single(s => s.Day == day);
            var executor = new SolutionExecutor(day, solution);

            return executor;
        }
        
        IEnumerable<ISolution> LoadSolutions() 
        {
            var solutions = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.BaseType == typeof(ISolution));

            foreach(var solution in solutions) 
            {
                yield return (ISolution) Activator.CreateInstance(solution);
            }
        }
    }
}
