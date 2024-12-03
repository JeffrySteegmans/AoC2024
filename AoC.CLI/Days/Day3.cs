using System.Text;
using System.Text.RegularExpressions;

namespace AoC.CLI.Days;

internal static partial class CustomRegex
{
    [GeneratedRegex(@"mul\([0-9]{1,3},[0-9]{1,3}\)")]
    public static partial Regex Part1Regex();

    [GeneratedRegex(@"mul\([0-9]{1,3},[0-9]{1,3}\)|don't\(\)|do\(\)")]
    public static partial Regex Part2Regex();
}

internal class Day3
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var builder = new StringBuilder();

        foreach(var line in input)
        {
            builder.Append(line);
        }

        MatchCollection matches = CustomRegex.Part1Regex()
            .Matches(builder.ToString());

        var answer = matches
            .Select(x => {
                var instructions = x.ToString().Replace("mul(", "").Replace(")", "").Split(',');
                return int.Parse(instructions[0]) * int.Parse(instructions[1]);
            })
            .Sum();

        return Task.FromResult(answer.ToString());
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var builder = new StringBuilder();

        foreach(var line in input)
        {
            builder.Append(line);
        }

        MatchCollection matches = CustomRegex.Part2Regex()
            .Matches(builder.ToString());

        var doCalculation = true;

        var answer = matches
            .Select(x => {
                if(x.ToString() == "do()")
                {
                    doCalculation = true;
                    return 0;
                }
                else if(x.ToString() == "don't()")
                {
                    doCalculation = false;
                    return 0;
                }

                if(!doCalculation)
                {
                    return 0;
                }
                
                var instructions = x.ToString().Replace("mul(", "").Replace(")", "").Split(',');
                return int.Parse(instructions[0]) * int.Parse(instructions[1]);
            })
            .Sum();

        return Task.FromResult(answer.ToString());
    }
}