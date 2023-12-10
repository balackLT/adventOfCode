using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Solutions2015.Day12;

public class Solution : ISolution
{
    public int Day { get; } = 12;

    public object SolveFirstPart(Input input)
    {
        var json = input.GetAsString();

        var regex = new Regex(@"[-]?\d+");
        var result = regex
            .Matches(json)
            .Select(m => m.Groups.Values.First().Value)
            .Sum(int.Parse);

        return result.ToString();
    }
        
    public object SolveSecondPart(Input input)
    {
        var text = input.GetAsString();
            
        var json = JObject.Parse(@"{""fake"":" + text + "}");

        var result = GetSum(json);
            
        return result.ToString();
    }

    private int GetSum(JToken json)
    {
        switch (json)
        {
            case JValue value when int.TryParse(value.Value.ToString(), out var result):
                return result;
            case JArray array:
            {
                var result = 0;
                
                foreach (var token in array)
                {
                    result += GetSum(token);
                }

                return result;
            }
            case JObject obj:
            {
                var result = 0;
                
                foreach (var kvp in obj)
                {
                    if (kvp.Value is JValue value && value.ToString() == "red")
                        return 0;
                        
                    result += GetSum(kvp.Value);
                }

                return result;
            }
            default:
                return 0;
        }
    }
}