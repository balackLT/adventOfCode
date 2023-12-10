using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2016.Day08;

public class Solution : ISolution
{
    public int Day { get; } = 8;
        
    public object SolveFirstPart(Input input)
    {
        var lines = input.GetLines();

        var map = new Dictionary<Coordinate, char>();
        var maxX = 50;
        var maxY = 6;
        
        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                map[new Coordinate(x, y)] = '.';
            }
        }

        foreach (var line in lines)
        {
            var splitLine = line.Split();

            if (splitLine[0] == "rect")
            {
                var rectX = int.Parse(splitLine[1].Split('x')[0]);
                var rectY = int.Parse(splitLine[1].Split('x')[1]);

                for (int x = 0; x < rectX; x++)
                {
                    for (int y = 0; y < rectY; y++)
                    {
                        map[new Coordinate(x, y)] = '#';
                    }
                }
            }

            if (splitLine[0] == "rotate")
            {
                var distance = int.Parse(splitLine[4]);
                var index = int.Parse(splitLine[2].Split('=')[1]);

                var newMap = new Dictionary<Coordinate, char>();
                
                if (splitLine[1] == "column")
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        newMap[new Coordinate(index, (y + distance) % maxY)] = map[new Coordinate(index, y)];
                    }
                }
                
                if (splitLine[1] == "row")
                {
                    for (int x = 0; x < maxX; x++)
                    {
                        newMap[new Coordinate((x + distance) % maxX, index)] = map[new Coordinate(x, index)];
                    }
                }
                
                foreach (var coordinate in newMap.Keys)
                {
                    map[coordinate] = newMap[coordinate];
                }
            }
            
            //map.Print();
        }
        
        map.Print();
        
        return map.Count(c => c.Value == '#').ToString();
    }

    public object SolveSecondPart(Input input)
    {
        // solve "by hand" aka looking at output
        var lines = input.GetLines();

        return 0.ToString();
    }
}