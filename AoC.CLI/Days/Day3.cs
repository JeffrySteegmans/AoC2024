using System.Text;
using System.Text.RegularExpressions;

namespace AoC.CLI.Days;

internal static partial class CustomRegex
{
    [GeneratedRegex(@"mul\([0-9]{1,3},[0-9]{1,3}\)")]
    public static partial Regex MulRegex();
}

internal class Day3
    : IDay
{
    private const string pattern = @"mul\([0-9]{1,3},[0-9]{1,3}\)";

    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var builder = new StringBuilder();

        foreach(var line in input)
        {
            builder.Append(line);
        }

        MatchCollection matches = CustomRegex.MulRegex()
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
        var regex = new Regex(pattern);

        Match m = regex.Match(input.First());

        return Task.FromResult(m.Groups.Count.ToString());
    }
}