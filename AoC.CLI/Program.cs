using AoC.CLI;
using AoC.CLI.Days;

Dictionary<int, IDay> days = new()
{
    { 1, new Day1() }
};

Console.Write("Day: ");
var day = int.Parse(Console.ReadLine());
Console.Write("Part: ");
var part = int.Parse(Console.ReadLine());

var input = await ReadInput(day);

var dayToExecute = days[day];
if (part == 1)
{
    await dayToExecute.ExecutePart1(input);
}
else
{
    await dayToExecute.ExecutePart2(input);
}

static async Task<List<string>> ReadInput(
    int day)
{
    var content = new List<string>();
    var filePath = $"Days/Input/Day{day}.txt";

    try
    {
        Console.WriteLine("Reading input...");
        using var sr = new StreamReader(filePath);
        
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
