namespace AoC.CLI.Days;

public class Day11
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var stones = input
            .First()
            .Split(" ")
            .Select(x => new Stone(long.Parse(x)))
            .ToList();

        long numberOfStones = 0;

        foreach (var stone in stones)
        {
            numberOfStones += TransformSequence(stone, 25);
        }

        var answer = numberOfStones
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("Not implemented");
    }

    private static long TransformSequence(
        Stone stone,
        int numberOfTransformations)
    {
        List<Stone> stones = [ stone ];

        for (var i = 0; i < numberOfTransformations; i++)
        {
            stones = Transform(stones);
        }

        return stones.Count;
    }

    private static List<Stone> Transform(
        List<Stone> stones)
    {
        var transformedStones = new List<Stone>();

        foreach (var stone in stones)
        {
            if (stone.Value == 0)
            {
                transformedStones.Add(new Stone(1));
                continue;
            }

            if (stone.Value.ToString().Length % 2 == 0)
            {
                var spanValue = stone.Value.ToString().AsSpan();
                var firstStoneValue = spanValue[..(spanValue.Length/2)];
                var secondStoneValue = spanValue[(spanValue.Length/2)..spanValue.Length];

                transformedStones.Add(new Stone(long.Parse(firstStoneValue.ToString())));
                transformedStones.Add(new Stone(long.Parse(secondStoneValue.ToString())));
                continue;
            }

            transformedStones.Add(new Stone(stone.Value * 2024));
        }

        return transformedStones;
    }
}

internal class Stone(
    long value)
{
    public long Value { get; set; } = value;
}