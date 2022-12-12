using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<List<T>> SlidingWindows<T>(this IEnumerable<T> enumerable, int windowSize)
    {
        var currentWindow = new Queue<T>();
        
        foreach (var item in enumerable)
        {
            currentWindow.Enqueue(item);
            if (currentWindow.Count > windowSize)
            {
                currentWindow.Dequeue();
            }

            if (currentWindow.Count == windowSize)
            {
                yield return new List<T>(currentWindow);
            }
        }
    }

    public static string ConcatToString(this IEnumerable<char> enumerable)
    {
        return string.Concat(enumerable);
    }
}