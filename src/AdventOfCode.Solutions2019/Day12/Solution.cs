using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Executor;

namespace AdventOfCode.Solutions2019.Day12;

public class Moon
{
    public int Id;
    public Vector3 Location;
    public Vector3 Velocity;
    public List<Moon> Friends;

    public void CalculateVelocity()
    {
        foreach (var friend in Friends)
        {
            if (friend.Location.X > Location.X)
                Velocity.X++;
            else if (friend.Location.X < Location.X)
                Velocity.X--;
                
            if (friend.Location.Y > Location.Y)
                Velocity.Y++;
            else if (friend.Location.Y < Location.Y)
                Velocity.Y--;
                
            if (friend.Location.Z > Location.Z)
                Velocity.Z++;
            else if (friend.Location.Z < Location.Z)
                Velocity.Z--;
        }
    }

    public void Move() => Location += Velocity;

    public long TotalEnergy()
    {
        var kinetic = (long)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z));
        var potential = (long)(Math.Abs(Location.X) + Math.Abs(Location.Y) + Math.Abs(Location.Z));

        return kinetic * potential;
    }

    public override string ToString()
    {
        return $"{Id}: {Location.ToString()} --- {Velocity.ToString()}";
    }
}
    
public class Solution : ISolution
{
    public int Day { get; } = 12;
        
    public object SolveFirstPart(Input input)
    {
        var r = new Regex(@"-?\d+");
        var moons = input.GetLines()
            .Select(l => r.Matches(l).Select(m => m.Value))
            .Select(l => l.Select(int.Parse).ToList())
            .Select((m, i) => new Moon{Id = i, Location = new Vector3(m[0], m[1], m[2])})
            .ToList();

        foreach (var moon in moons)
        {
            moon.Friends = moons.Where(m => m.Id != moon.Id).ToList();
        }

        for (var i = 0; i < 1000; i++)
        {
            foreach (var moon in moons)
            {
                moon.CalculateVelocity();
            }
                
            foreach (var moon in moons)
            {
                moon.Move();
            }
        }

        var result = moons.Sum(m => m.TotalEnergy());
            
        return result.ToString();
    }

    public object SolveSecondPart(Input input)
    {
        var r = new Regex(@"-?\d+");
        var moons = input.GetLines()
            .Select(l => r.Matches(l).Select(m => m.Value))
            .Select(l => l.Select(int.Parse).ToList())
            .Select((m, i) => new Moon{Id = i, Location = new Vector3(m[0], m[1], m[2])})
            .ToList();

        foreach (var moon in moons)
        {
            moon.Friends = moons.Where(m => m.Id != moon.Id).ToList();
        }

        var originalX = GetIdentityX(moons);
        var originalY = GetIdentityY(moons);
        var originalZ = GetIdentityZ(moons);

        long periodX = 0;
        long periodY = 0;
        long periodZ = 0;
            
        var iteration = 0;
        while (true)
        {
            iteration++;
                
            foreach (var moon in moons)
            {
                moon.CalculateVelocity();
            }
                
            foreach (var moon in moons)
            {
                moon.Move();
            }
                
            var uniqueX = GetIdentityX(moons);
            var uniqueY = GetIdentityY(moons);
            var uniqueZ = GetIdentityZ(moons);

            if (periodX == 0 && originalX == uniqueX)
                periodX = iteration + 1; //Console.WriteLine($"X: {iteration}");
            if (periodY == 0 && originalY == uniqueY)
                periodY = iteration + 1; //Console.WriteLine($"Y: {iteration}");
            if (periodZ == 0 && originalZ == uniqueZ)
                periodZ = iteration + 1;  //Console.WriteLine($"Z: {iteration}");

            if (periodX > 0 && periodY > 0 && periodZ > 0)
                break;
        }

        var lcm = Lcm(periodX, Lcm(periodY, periodZ));
            
        return lcm.ToString();
    }

    public string GetIdentityX(List<Moon> moons)
    {
        var sb = new StringBuilder();

        foreach (var moon in moons)
        {
            sb.Append(moon.Location.X);
            sb.Append('|');
        }

        return sb.ToString();
    }
        
    public string GetIdentityY(List<Moon> moons)
    {
        var sb = new StringBuilder();

        foreach (var moon in moons)
        {
            sb.Append(moon.Location.Y);
            sb.Append('|');
        }

        return sb.ToString();
    }
        
    public string GetIdentityZ(List<Moon> moons)
    {
        var sb = new StringBuilder();

        foreach (var moon in moons)
        {
            sb.Append(moon.Location.Z);
            sb.Append('|');
        }

        return sb.ToString();
    }
        
    private long Gcf(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private long Lcm(long a, long b)
    {
        return (a / Gcf(a, b)) * b;
    }
}