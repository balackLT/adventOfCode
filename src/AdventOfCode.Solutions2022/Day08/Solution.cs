using AdventOfCode.Executor;
using AdventOfCode.Utilities.Map;

namespace AdventOfCode.Solutions2022.Day08;

public class Solution : ISolution
{
    public int Day { get; } = 8;

    public object SolveFirstPart(Input input)
    {
        var charMap = input.GetAsCoordinateMap();

        var treeDict = charMap
            .Select(c => new KeyValuePair<Coordinate, Tree>(c.Key, new Tree(c.Value - '0')))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var tree in treeDict)
        {
            foreach (var direction in Coordinate.Directions)
            {
                if (tree.Value.IsVisible)
                    break;
                var visibleInDirection =  VisibleInDirection(tree, direction, treeDict);
                if (!visibleInDirection.Any(t => t.Height >= tree.Value.Height))
                {
                    tree.Value.IsVisible = true;
                }
            }
        }

        var result = treeDict.Count(t => t.Value.IsVisible);
        
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var charMap = input.GetAsCoordinateMap();

        Dictionary<Coordinate, Tree> treeDict = charMap
            .Select(c => new KeyValuePair<Coordinate, Tree>(c.Key, new Tree(c.Value - '0')))
            .ToDictionary(kv => kv.Key, kv => kv.Value);

        foreach (var tree in treeDict)
        {
            var score = 1;
            
            foreach (var direction in Coordinate.Directions)
            {
                score *= VisibleInDirection(tree, direction, treeDict).Count();
            }

            tree.Value.Score = score;
        }

        var maxScoreTree = treeDict.MaxBy(t => t.Value.Score);

        return maxScoreTree.Value.Score.ToString();
    }

    private static IEnumerable<Tree> VisibleInDirection(KeyValuePair<Coordinate, Tree> tree, Coordinate direction, Dictionary<Coordinate, Tree> map)
    {
        var currentTree = tree.Key + direction;
        while (map.TryGetValue(currentTree, out var nextTree))
        {
            yield return nextTree;
            if (nextTree.Height >= tree.Value.Height)
            {
                break;
            }
            currentTree += direction;
        }
    }

    private record Tree(int Height)
    {
        public bool IsVisible { get; set; } = false;
        public int Score { get; set; } = 0;
    };
}