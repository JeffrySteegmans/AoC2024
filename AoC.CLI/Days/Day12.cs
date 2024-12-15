using AoC.CLI.Enums;
using AoC.CLI.Extensions;
using AoC.CLI.Models;
using Microsoft.CSharp.RuntimeBinder;

namespace AoC.CLI.Days;

public class Day12
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = new Garden(input)
            .CalculateRegions()
            .CalculatePerimeters()
            .CalculatePrice()
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var answer = new Garden(input)
            .CalculateRegions()
            .CalculateSides()
            .CalculatePriceWithBulkDiscount()
            .ToString();

        return Task.FromResult(answer);
    }
}

internal class Garden(IEnumerable<string> input)
{
    private readonly char[,] _garden = input.ParseMap();
    private readonly List<Region> _regions = [];

    public Garden CalculateRegions()
    {
        var visited = new bool[_garden.GetLength(0), _garden.GetLength(1)];

        for (var row = 0; row < _garden.GetLength(0); row++)
        {
            for (var col = 0; col < _garden.GetLength(1); col++)
            {
                var currentCoordinate = new Coordinate(row, col);
                var plant = _garden[row, col];

                if (visited[currentCoordinate.Row, currentCoordinate.Col])
                {
                    continue;
                }

                var region = new Region(plant)
                {
                    Coordinates = FindNeighbours(_garden, plant, currentCoordinate, [], visited)
                };
                _regions.Add(region);
            }
        }

        return this;
    }

    private static List<Coordinate> FindNeighbours(
        char[,] source,
        char plant,
        Coordinate currentCoordinate,
        List<Coordinate> regionCoordinates,
        bool[,] visited)
    {
        if (currentCoordinate.Row < 0 || currentCoordinate.Row >= source.GetLength(0) ||
            currentCoordinate.Col < 0 || currentCoordinate.Col >= source.GetLength(1))
        {
            return regionCoordinates;
        }

        if (visited[currentCoordinate.Row, currentCoordinate.Col])
        {
            return regionCoordinates;
        }

        if (regionCoordinates.Any(x => x == currentCoordinate))
        {
            return regionCoordinates;
        }

        if (source[currentCoordinate.Row, currentCoordinate.Col] != plant)
        {
            return regionCoordinates;
        }

        regionCoordinates.Add(currentCoordinate);
        visited[currentCoordinate.Row, currentCoordinate.Col] = true;

        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Right), regionCoordinates, visited);
        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Down), regionCoordinates, visited);
        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Left), regionCoordinates, visited);
        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Up), regionCoordinates, visited);

        return regionCoordinates;
    }

    public Garden CalculatePerimeters()
    {
        foreach (var region in _regions)
        {
            region.CalculatePerimeter();
        }

        return this;
    }

    public Garden CalculateSides()
    {
        foreach (var region in _regions)
        {
            region.CalculateSides(_garden);
        }

        return this;
    }

    public int CalculatePrice()
    {
        return _regions
            .Select(x => x.Price)
            .Sum();
    }

    public int CalculatePriceWithBulkDiscount()
    {
        return _regions
            .Select(x => x.PriceBulkDiscount)
            .Sum();
    }
}

internal class Region(
    char plantType)
{
    private int _perimeter = 0;
    private int _sides = 0;

    public char PlantType => plantType;
    public List<Coordinate> Coordinates { get; init; } = [];
    public int Price => Coordinates.Count * _perimeter;
    public int PriceBulkDiscount => Coordinates.Count * _sides;

    public void CalculatePerimeter()
    {
        foreach (var coordinate in Coordinates)
        {
            var neighbours = Coordinates
                .GetNeighbours(coordinate)
                .Count;

            _perimeter += 4 - neighbours;
        }
    }

    public void CalculateSides(
        char [,] garden)
    {
        foreach (var coordinate in Coordinates)
        {
            _sides += GetCorners(garden, coordinate);
        }
    }

    private int GetCorners(
        char[,] garden,
        Coordinate coordinate)
    {
        var up = coordinate.Move(Direction.Up);
        var upRight = coordinate.Move(Direction.Up).Move(Direction.Right);
        var right = coordinate.Move(Direction.Right);
        var downRight = coordinate.Move(Direction.Down).Move(Direction.Right);
        var down = coordinate.Move(Direction.Down);
        var downLeft = coordinate.Move(Direction.Down).Move(Direction.Left);
        var left = coordinate.Move(Direction.Left);
        var upLeft = coordinate.Move(Direction.Up).Move(Direction.Left);

        var corners = 0;

        if ((!garden.IsInside(up) || garden[up.Row, up.Col] != plantType) &&
            (!garden.IsInside(left) || garden[left.Row, left.Col] != plantType))
        {
            corners++;
        }

        if ((!garden.IsInside(up) || garden[up.Row, up.Col] != plantType) &&
            (!garden.IsInside(right) || garden[right.Row, right.Col] != plantType))
        {
            corners++;
        }

        if ((!garden.IsInside(down) || garden[down.Row, down.Col] != plantType) &&
            (!garden.IsInside(left) || garden[left.Row, left.Col] != plantType))
        {
            corners++;
        }

        if ((!garden.IsInside(down) || garden[down.Row, down.Col] != plantType) &&
            (!garden.IsInside(right) || garden[right.Row, right.Col] != plantType))
        {
            corners++;
        }

        if (garden.IsInside(up) && garden[up.Row, up.Col] == plantType &&
            garden.IsInside(right) && garden[right.Row, right.Col] == plantType &&
            garden[upRight.Row, upRight.Col] != plantType)
        {
            corners++;
        }

        if (garden.IsInside(down) && garden[down.Row, down.Col] == plantType &&
            garden.IsInside(right) && garden[right.Row, right.Col] == plantType &&
            garden[downRight.Row, downRight.Col] != plantType)
        {
            corners++;
        }

        if (garden.IsInside(down) && garden[down.Row, down.Col] == plantType &&
            garden.IsInside(left) && garden[left.Row, left.Col] == plantType &&
            garden[downLeft.Row, downLeft.Col] != plantType)
        {
            corners++;
        }

        if (garden.IsInside(up) && garden[up.Row, up.Col] == plantType &&
            garden.IsInside(left) && garden[left.Row, left.Col] == plantType &&
            garden[upLeft.Row, upLeft.Col] != plantType)
        {
            corners++;
        }

        return corners;
    }
}