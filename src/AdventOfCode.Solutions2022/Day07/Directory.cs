namespace AdventOfCode.Solutions2022.Day07;

public record Directory(string Name, Directory Parent, List<File> Files, List<Directory> Directories)
{
    private Lazy<long> Size => new(() => Files.Sum(f => f.Size) + Directories.Sum(d => d.TotalSize()));

    public long TotalSize()
    {
        return Size.Value;
    }

    public IEnumerable<Directory> AllDirectories()
    {
        yield return this;

        foreach (Directory child in Directories.SelectMany(directory => directory.AllDirectories()))
        {
            yield return child;
        }
    }
}

public record File(string Name, long Size);