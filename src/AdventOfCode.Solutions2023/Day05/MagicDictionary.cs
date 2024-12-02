namespace AdventOfCode.Solutions2023.Day05;

public class MagicDictionary
{
    private readonly List<Range> _ranges = new();
    
    public void AddRange(long destination, long source, long rangeLength)
    {
        var range = new Range(destination, source, rangeLength);
        
        _ranges.Add(range);
    }

    private record Range(long Destination, long Source, long RangeLength);

    public long ReverseIndex(long index)
    {
        var range = _ranges.FirstOrDefault(r => r.Destination <= index && r.Destination + r.RangeLength - 1 >= index);
        if (range is null)
            return index;

        return index - range.Destination + range.Source;
    }
    
    public long this[long index]   
    {
        get
        {
            var range = _ranges.FirstOrDefault(r => r.Source <= index && r.Source + r.RangeLength - 1 >= index);
            if (range is null)
                return index;

            return index - range.Source + range.Destination;
        }
    }
}