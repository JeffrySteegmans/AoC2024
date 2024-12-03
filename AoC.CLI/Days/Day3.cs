using System.Text.RegularExpressions;

namespace AoC.CLI.Days;

internal class Day3
    : IDay
{
    private const string pattern = @"mul\([0-9]{1,3},[0-9]{1,3}\)";

    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var regex = new Regex(pattern);

        Match m = regex.Match(input.First());

        return Task.FromResult(m.Groups.Count.ToString());
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult(String.Join("-", input));
    }
}