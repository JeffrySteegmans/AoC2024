namespace AoC.CLI.Days;

internal class Day7
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = input
            .ParseEquations()
            .Where(x => x.IsValid())
            .Sum(x => x.Value)
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("Not implemented yet");
    }
}

internal static class Day7Extensions
{
    public static List<Equation> ParseEquations(
        this IEnumerable<string> input)
    {
        return input
            .Select(ParseEquation)
            .ToList();
    }

    private static Equation ParseEquation(
        string input)
    {
        var parts = input.Split(":", StringSplitOptions.RemoveEmptyEntries);

        var value = long.Parse(parts[0].Trim());
        var parameters = parts[1]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        return new Equation(value, parameters);
    }
}

internal record Equation(
    long Value,
    List<int> Parameters)
{
    public bool IsValid()
    {
        var result = Parameters[0];

        if(Validate(result, 1, (a, b) => a + b))
        {
            return true;
        }

        if (Validate(result, 1, (a, b) => a * b))
        {
            return true;
        }

        return false;
    }

    private bool Validate(
        long result,
        int index,
        Func<long, int, long> calculate)
    {
        result = calculate(result, Parameters[index]);

        if (index == Parameters.Count - 1)
        {
            return result == Value;
        }

        if (Validate(result, index + 1, (a, b) => a + b))
        {
            return true;
        }

        if (Validate(result, index + 1, (a, b) => a * b))
        {
            return true;
        }

        return false;
    }
};
