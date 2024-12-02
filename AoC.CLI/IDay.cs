namespace AoC.CLI;

internal interface IDay
{
    Task ExecutePart1(IEnumerable<string> input);
    Task ExecutePart2(IEnumerable<string> input);
}
