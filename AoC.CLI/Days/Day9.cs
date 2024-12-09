using System.Numerics;

namespace AoC.CLI.Days;

public class Day9
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = input
            .CreateDiskMap()
            .GenerateBlocks()
            .Defragment()
            .CalculateChecksum()
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("Not implemented");
    }
}

internal static class Day9Extensions
{
    public static List<int> CreateDiskMap(
        this IEnumerable<string> input)
    {
        var diskMap = input
            .First()
            .Select(x => (int)Char.GetNumericValue(x))
            .ToList();

        return diskMap;
    }

    public static List<string> GenerateBlocks(
        this List<int> diskMap)
    {
        var blocks = new List<string>();

        bool isFile = true;
        int fileId = 0;
        foreach (var digit in diskMap)
        {
            for (var i = 0; i < digit; i++)
            {
                var block = ".";

                if (isFile)
                {
                    block = fileId.ToString();
                }

                blocks.Add(block);
            }

            if (isFile)
            {
                fileId++;
            }
            isFile = !isFile;
        }

        return blocks;
    }

    public static List<string> Defragment(
        this List<string> fragmentedBlocks)
    {
        List<string> defragmentedBlocks = [..fragmentedBlocks];

        for (var i = 0; i < defragmentedBlocks.Count; i++)
        {
            var lastFileIndex = defragmentedBlocks.LastIndexOf(defragmentedBlocks.Last(x => x != "."));

            if (lastFileIndex <= i)
            {
                break;
            }

            if (defragmentedBlocks[i] == ".")
            {
                (defragmentedBlocks[i], defragmentedBlocks[lastFileIndex]) = (defragmentedBlocks[lastFileIndex], defragmentedBlocks[i]);
            }
        }

        return defragmentedBlocks;
    }

    public static long CalculateChecksum(
        this List<string> blocks)
    {
        var multipliedBlocks = blocks
            .Where(x => x != ".")
            .Select((x, index) => int.Parse(x) * index);

        long sum = 0;
        foreach (var b in multipliedBlocks)
        {
            sum += b;
        }

        return sum;
    }
}