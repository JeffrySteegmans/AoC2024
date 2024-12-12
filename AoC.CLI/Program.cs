using System.Diagnostics;
using System.Reflection;
using AoC.CLI;
using AoC.CLI.Days;

Dictionary<int, IDay> days = new()
{
    { 1, new Day1() },
    { 2, new Day2() },
    { 3, new Day3() },
    { 4, new Day4() },
    { 5, new Day5() },
    { 6, new Day6() },
    { 7, new Day7() },
    { 8, new Day8() },
    { 9, new Day9() },
    { 10, new Day10() },
    { 11, new Day11() }
};

Console.Write("Day: ");
var day = int.Parse(Console.ReadLine());
Console.WriteLine("---------------------");

var input = await ReadInput(day);

var dayToExecute = days[day];

var stopWatch = new Stopwatch();

Console.WriteLine("Calculating part 1...");
Console.WriteLine("---------------------");
stopWatch.Start();
var answerPart1 = await dayToExecute.ExecutePart1(input);
stopWatch.Stop();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(answerPart1);
Console.ResetColor();
Console.WriteLine("---------------------");
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine($"Calculated in {stopWatch.ElapsedMilliseconds}ms");
Console.ResetColor();
Console.WriteLine("");

stopWatch.Reset();

Console.WriteLine("Calculating part 2...");
Console.WriteLine("---------------------");
stopWatch.Start();
var answerPart2 = await dayToExecute.ExecutePart2(input);
stopWatch.Stop();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(answerPart2);
Console.ResetColor();
Console.WriteLine("---------------------");
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine($"Calculated in {stopWatch.ElapsedMilliseconds}ms");
Console.ResetColor();
Console.WriteLine("");

static async Task<List<string>> ReadInput(
    int day)
{
    var content = new List<string>();

    var assembly = Assembly.GetExecutingAssembly();
    var resourcePath = assembly
        .GetManifestResourceNames()
        .Single(str => str.EndsWith($"Day{day}.txt"));

    try
    {
        Console.WriteLine("");
        Console.Write("Reading input...");
        using var stream = assembly.GetManifestResourceStream(resourcePath);
        using var sr = new StreamReader(stream);
        
        var line = await sr.ReadLineAsync();
        while (line is not null)
        {
            content.Add(line);
            line = await sr.ReadLineAsync();
        }
        sr.Close();
        Console.WriteLine("DONE");
        Console.WriteLine("");
    }
    catch (Exception e)
    {
        Console.WriteLine("Exception: " + e.Message);
    }

    return content;
}
