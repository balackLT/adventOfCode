using AdventOfCode.Executor;
using AdventOfCode.Utilities.Extensions;

namespace AdventOfCode.Solutions2022.Day07;

public class Solution : ISolution
{
    public int Day { get; } = 7;

    public string SolveFirstPart(Input input)
    {
        var lines = new Queue<string>(input.GetLines());

        Directory root = ParseFileSystem(lines);

        var allDirs = root.AllDirectories().ToList();

        var result = allDirs.Where(d => d.TotalSize() <= 100000).Sum(d => d.TotalSize());
        
        return result.ToString();
    }
    
    public string SolveSecondPart(Input input)
    {
        var lines = new Queue<string>(input.GetLines());

        Directory root = ParseFileSystem(lines);

        var allDirs = root.AllDirectories().ToList();

        var spaceToReclaim = 30000000 - (70000000 - root.TotalSize());

        var dirToDelete = allDirs
            .Where(d => d.TotalSize() >= spaceToReclaim)
            .MinBy(d => d.TotalSize());
        
        return dirToDelete!.TotalSize().ToString();
    }
    
    private static Directory ParseFileSystem(Queue<string> lines)
    {
        var root = new Directory("/", null!, new List<File>(), new List<Directory>());
        var currentDir = root;
        lines.Dequeue();

        while (lines.TryDequeue(out var line))
        {
            if (line.StartsWith("$ ls"))
            {
                while (lines.TryPeek(out var lsResult) && !lsResult.StartsWith("$"))
                {
                    lines.Dequeue();
                    var split = lsResult.Split();

                    // directory
                    if (split[0] == "dir")
                    {
                        currentDir.Directories.Add(new Directory(split[1], currentDir, new List<File>(),
                            new List<Directory>()));
                    }
                    // file
                    else
                    {
                        currentDir.Files.Add(new File(split[1], int.Parse(split[0])));
                    }
                }
            }
            else if (line.StartsWith("$ cd"))
            {
                var target = line.Split()[2];
                currentDir = target == ".." ? currentDir.Parent : currentDir.Directories.Single(d => d.Name == target);
            }
        }

        return root;
    }

    
}