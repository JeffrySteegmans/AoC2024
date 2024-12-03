using System.Reflection;
using AoC.CLI;
using AoC.CLI.Days;

Dictionary<int, IDay> days = new()
{
    { 1, new Day1() },
    { 2, new Day2() },
    { 3, new Day3() }
};

Console.Write("Day: ");
var day = int.Parse(Console.ReadLine());

var input = await ReadInput(day);

var dayToExecute = days[day];

var answerPart1 = await dayToExecute.ExecutePart1(input);
var answerPart2 = await dayToExecute.ExecutePart2(input);

Console.WriteLine($"Part 1: {answerPart1}");
Console.WriteLine($"Part 2: {answerPart2}");

static async Task<List<string>> ReadInput(
    int day)
{
    var content = new List<string>();

    var assembly = Assembly.GetExecutingAssembly();
    var resourcePath = assembly.GetManifestResourceNames()
            .Single(str => str.EndsWith($"Day{day}.txt"));

    try
    {
        Console.WriteLine("Reading input...");
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using var sr = new StreamReader(stream);
        
        var line = await sr.ReadLineAsync();
        while (line is not null)
        {
            content.Add(line);
            line = await sr.ReadLineAsync();
        }
        sr.Close();
    }
    catch (Exception e)
    {
        Console.WriteLine("Exception: " + e.Message);
    }

    return content;
}
