using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities;

public static class AocMath
{
    public static long Lcm(IEnumerable<long> numbers)
    {
        return numbers.Aggregate(Lcm);
    }
    
    public static long Lcm(long a, long b)
    {
        return Math.Abs(a * b) / Gcd(a, b);
    }
    
    public static long Gcd(long a, long b)
    {
        return b == 0 ? a : Gcd(b, a % b);
    }
}