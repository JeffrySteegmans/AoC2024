namespace AoC.CLI.Days;

internal class Day1
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var (leftList, rightList) = ParseLists(input);

        leftList = [.. leftList.Order()];
        rightList = [.. rightList.Order()];

        var answer = leftList
            .Select((x, i) =>
            {
                return Math.Abs(x - rightList[i]);
            })
            .Sum();

        return Task.FromResult(answer.ToString());
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var (leftList, rightList) = ParseLists(input);

        var answer = leftList
            .Select(x => x * rightList.Count(y => y == x))
            .Sum();

        return Task.FromResult(answer.ToString());
    }

    private static (List<int>, List<int>) ParseLists(
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

        return (
            leftList,
            rightList
        );
    }
}
