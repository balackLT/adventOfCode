using System.Text.Json.Nodes;

namespace AdventOfCode.Solutions2022.Day13;

public class ArrayPair
{
    public JsonArray Left { get; set; }
    public JsonArray Right { get; set; }

    public bool Ordered => IsOrdered.Value;
    private Lazy<bool> IsOrdered => new(() => Compare(Left, Right) == ComparisonResult.Ordered);

    public static ComparisonResult Compare(JsonArray left, JsonArray right, int depth = 0)
    {
        Console.WriteLine($"{new string(' ', depth*2)}- Compare {left.ToJsonString()} vs {right.ToJsonString()}");
        for (int element = 0; element < left.Count; element++)
        {
            var leftElement = left[element];
            if (right.Count < element + 1)
            {
                return ComparisonResult.Disordered;
            }
            
            var rightElement = right[element];

            if (leftElement is JsonValue leftValue && rightElement is JsonValue rightValue)
            {
                Console.WriteLine($"{new string(' ', depth*2)}- Compare {leftValue} vs {rightValue}");
                var result = Compare(leftValue, rightValue);

                if (result == ComparisonResult.Ordered)
                {
                    Console.WriteLine($"{new string(' ', depth*2)}- Left side is smaller, so inputs are in the right order");
                }
                if (result == ComparisonResult.Disordered)
                {
                    Console.WriteLine($"{new string(' ', depth*2)}- Right side is smaller, so inputs are NOT in the right order");
                }
                
                if (result != ComparisonResult.Equal)
                    return result;
            }
            else if (leftElement is JsonArray leftArray && rightElement is JsonArray rightArray)
            {
                var result = Compare(leftArray, rightArray, depth + 1);
                if (result != ComparisonResult.Equal)
                    return result;
            }
            else if (leftElement is JsonValue)
            {
                var result = Compare(new JsonArray{leftElement.GetValue<int>()}, (rightElement as JsonArray)!, depth + 1);
                if (result != ComparisonResult.Equal)
                    return result;
            }
            else
            {
                var result = Compare((leftElement as JsonArray)!, new JsonArray{rightElement!.GetValue<int>()}, depth + 1);
                if (result != ComparisonResult.Equal)
                    return result;
            }
        }

        if (right.Count == left.Count)
        {
            return ComparisonResult.Equal;
        }

        Console.WriteLine($"{new string(' ', depth*2)}- Left side ran out of items, so inputs are in the right order");
        return ComparisonResult.Ordered;
    }

    private static ComparisonResult Compare(JsonValue leftValue, JsonValue rightValue)
    {
        if (leftValue.GetValue<int>() > rightValue.GetValue<int>())
        {
            return ComparisonResult.Disordered;
        }

        if (leftValue.GetValue<int>() < rightValue.GetValue<int>())
        {
            return ComparisonResult.Ordered;
        }

        return ComparisonResult.Equal;
    }

    public enum ComparisonResult
    {
        Ordered,
        Disordered,
        Equal
    }
}