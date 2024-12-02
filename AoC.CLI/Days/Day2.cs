
namespace AoC.CLI.Days;

internal class Day2
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = input
            .Count(IsSave);

        return Task.FromResult(answer.ToString());
    }

    private bool IsSave(
        string report)
    {
        var parts = report
            .Split(' ')
            .Select(x => int.Parse(x))
            .ToList();

        bool? increasing = null;

        for (int i = 1; i < parts.Count; i++)
        {
            var difference = Math.Abs(parts[i] - parts[i - 1]);
            if (difference > 3 || difference < 1) {
                return false;
            }

            if (parts[i] > parts[i - 1])
            {
                if (increasing == false)
                {
                    return false; // Was decreasing, now increasing
                }
                increasing = true;
            }
            else if (parts[i] < parts[i - 1])
            {
                if (increasing == true)
                {
                    return false; // Was increasing, now decreasing
                }
                increasing = false;
            }
        }

        return true;
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("Part 2 not implemented");
    }
}
