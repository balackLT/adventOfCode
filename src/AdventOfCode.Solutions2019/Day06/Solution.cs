using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day06
{
    public class Planet
    {
        public string Name;
        public int OrbitCount;
        public List<Planet> OrbitedBy = new List<Planet>();
        public Planet Orbits;
        public bool Visited = false;
    }
    
    public class Solution : ISolution
    {
        public int Day { get; } = 6;
        
        private Dictionary<string, Planet> _orbitMap;
        
        public string SolveFirstPart(Input input)
        {
            var map = input.GetLinesAsList();
            
            _orbitMap = new Dictionary<string, Planet>();

            ConstructMap(map);

            var root = _orbitMap.Values.FirstOrDefault(p => p.Orbits == null);

            TraverseAndCount(root, 0);

            var result = _orbitMap.Values.Select(p => p.OrbitCount).Sum();
            
            return result.ToString();
        }
        
        public string SolveSecondPart(Input input)
        {
            var map = input.GetLinesAsList();

            _orbitMap = new Dictionary<string, Planet>();
            
            ConstructMap(map);
            
            var target = _orbitMap.Values.FirstOrDefault(p => p.Name == "SAN")?.Orbits;
            var source = _orbitMap.Values.FirstOrDefault(p => p.Name == "YOU")?.Orbits;

            var root = _orbitMap.Values.FirstOrDefault(p => p.Orbits == null);
            TraverseAndCount(root, 0);
            var result = NaiveBfs(source, target, 0);
            
            return result.ToString();
        }

        private int NaiveBfs(Planet source, Planet target, int distanceTraveled)
        {
            source.Visited = true;
            
            if (source.Name == target.Name)
                return distanceTraveled;

            foreach (var planet in source.OrbitedBy.Where(o => o.Visited == false))
            {
                if (planet.Visited)
                    continue;

                var distanceToTarget = NaiveBfs(planet, target, distanceTraveled + 1);
                if (distanceToTarget > 0)
                    return distanceToTarget;
            }

            if (source.Orbits != null && source.Orbits.Visited == false)
            {
                if (source.Orbits.Visited)
                    return 0;
                
                var distanceToTarget = NaiveBfs(source.Orbits, target, distanceTraveled + 1);
                if (distanceToTarget > 0)
                    return distanceToTarget;
            }

            return 0;
        }

        private void TraverseAndCount(Planet current, int count)
        {
            current.OrbitCount = count;

            foreach (var planet in current.OrbitedBy)
            {
                TraverseAndCount(planet, count + 1);
            }
        }

        private void ConstructMap(List<string> map)
        {
            foreach (var orbit in map)
            {
                var current = orbit.Split(')');

                if (!_orbitMap.TryGetValue(current[1], out var planet2))
                {
                    planet2 = new Planet()
                    {
                        Name = current[1]
                    };
                    
                    _orbitMap.Add(planet2.Name, planet2);
                }
                
                if (!_orbitMap.TryGetValue(current[0], out var planet1))
                {
                    planet1 = new Planet
                    {
                        Name = current[0]
                    };
                    
                    _orbitMap.Add(planet1.Name, planet1);
                }

                planet1.OrbitedBy.Add(planet2);
                planet2.Orbits = planet1;
            }
        }
    }
}