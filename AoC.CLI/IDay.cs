namespace AoC.CLI;

internal interface IDay
{
    Task<string> ExecutePart1(IEnumerable<string> input);
    Task<string> ExecutePart2(IEnumerable<string> input);
}
