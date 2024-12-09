using System.Data;

using AoC.CLI.Extensions;
using AoC.CLI.Models;

namespace AoC.CLI.Days;

internal class Day8
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var map = input
            .ParseMap();

        var answer = map
            .FindAntennas()
            .CalculateAntiNodes(map)
            .Distinct()
            .Count()
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("Not implemented");
    }
}

internal static class Day8Extensions
{
    public static List<Antenna> FindAntennas(
        this char[,] map)
    {
        var antennas = new List<Antenna>();

        for (var row = 0; row < map.GetLength(0); row++)
        {
            for (var col = 0; col < map.GetLength(1); col++)
            {
                var frequency = map[row, col];

                if (frequency == '.')
                {
                    continue;
                }

                antennas
                    .Add(new Antenna(frequency, new Coordinate(row, col)));
            }
        }

        return antennas;
    }

    public static List<Coordinate> CalculateAntiNodes(
        this List<Antenna> antennas,
        char[,] map)
    {
        var rows = map.GetLength(0);
        var cols = map.GetLength(1);

        var antiNodes = new List<Coordinate>();

        foreach (var frequencyCoordinates in antennas.GroupBy(x => x.Frequency))
        {
            foreach (var coordinate in frequencyCoordinates.Select(x => x.Coordinate))
            {
                var temp = frequencyCoordinates
                    .Select(x => x.Coordinate)
                    .Where(x => x != coordinate);

                foreach (var item in temp)
                {
                    var newRow = coordinate.Row + (coordinate.Row - item.Row);
                    var newCol = coordinate.Col + (coordinate.Col - item.Col);

                    if (newRow < 0 || newRow >= rows)
                    {
                        continue;
                    }

                    if (newCol < 0 || newCol >= cols)
                    {
                        continue;
                    }

                    antiNodes
                        .Add(new Coordinate(newRow, newCol));
                }
            }
        }

        return antiNodes;
    }
}

internal sealed record Antenna(
    char Frequency,
    Coordinate Coordinate);
