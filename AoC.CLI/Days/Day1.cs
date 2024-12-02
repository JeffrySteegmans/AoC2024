namespace AoC.CLI.Days;

internal class Day1
    : IDay
{
    public Task ExecutePart1(
        IEnumerable<string> input)
    {
        List<int> leftList = [];
        List<int> rightList = [];

        foreach (var line in input)
        {
            var numbers = line.Split("   ");

            leftList.Add(int.Parse(numbers.First().ToString()));
            rightList.Add(int.Parse(numbers.Last().ToString()));
        }

        var orderedLeftList = leftList.Order().ToList();
        var orderedRightList = rightList.Order().ToList();

        var answer = orderedLeftList
            .Select((x, i) =>
            {
                return (int)Math.Abs(x - orderedRightList[i]);
            })
            .Sum();

        Console.WriteLine($"answer: {answer}");

        return Task.CompletedTask;
    }

    public Task ExecutePart2(
        IEnumerable<string> input)
    {
        Console.WriteLine("Day 1, Part 2");

        return Task.CompletedTask;
    }
}
