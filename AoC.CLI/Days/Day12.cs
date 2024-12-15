﻿using AoC.CLI.Enums;
using AoC.CLI.Extensions;
using AoC.CLI.Models;

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
        return Task.FromResult("Not implemented");
    }
}

internal class Garden(IEnumerable<string> input)
{
    private readonly char[,] _garden = input.ParseMap();
    private List<Region> _regions = [];

    public Garden CalculateRegions()
    {
        for (var row = 0; row < _garden.GetLength(0); row++)
        {
            for (var col = 0; col < _garden.GetLength(1); col++)
            {
                var currentCoordinate = new Coordinate(row, col);
                var plant = _garden[row, col];

                if (_regions.Any(x => x.Coordinates.Any(y => y == currentCoordinate)))
                {
                    continue;
                }

                var region = new Region(plant)
                {
                    Coordinates = FindNeighbours(_garden, plant, currentCoordinate, [])
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
        List<Coordinate> regionCoordinates)
    {
        if (currentCoordinate.Row < 0 || currentCoordinate.Row >= source.GetLength(0) ||
            currentCoordinate.Col < 0 || currentCoordinate.Col >= source.GetLength(1))
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

        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Right), regionCoordinates);
        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Down), regionCoordinates);
        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Left), regionCoordinates);
        regionCoordinates = FindNeighbours(source, plant, currentCoordinate.Move(Direction.Up), regionCoordinates);

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

    public int CalculatePrice()
    {
        return _regions
            .Select(x => x.Price)
            .Sum();
    }

    private static List<Coordinate> FindNeighbours(
        List<Coordinate> source,
        Coordinate currentCoordinate,
        List<Coordinate> regionCoordinates)
    {
        if (source.All(x => x != currentCoordinate))
        {
            return regionCoordinates;
        }

        if (regionCoordinates.Any(x => x == currentCoordinate))
        {
            return regionCoordinates;
        }

        regionCoordinates.Add(currentCoordinate);

        regionCoordinates = FindNeighbours(source, currentCoordinate.Move(Direction.Right), regionCoordinates);
        regionCoordinates = FindNeighbours(source, currentCoordinate.Move(Direction.Down), regionCoordinates);
        regionCoordinates = FindNeighbours(source, currentCoordinate.Move(Direction.Left), regionCoordinates);
        regionCoordinates = FindNeighbours(source, currentCoordinate.Move(Direction.Up), regionCoordinates);

        return regionCoordinates;
    }
}

internal class Region(
    char plantType)
{
    public char PlantType => plantType;
    public List<Coordinate> Coordinates { get; set; } = [];
    public int Area => Coordinates.Count;
    public int Perimeter { get; private set; } = 0;
    public int Price => Area * Perimeter;

    public void CalculatePerimeter()
    {
        foreach (var coordinate in Coordinates)
        {
            var neighbours = Coordinates
                .GetNeighbours(coordinate)
                .Count;

            Perimeter += 4 - neighbours;
        }
    }
}

internal static class CoordinateExtensions{
    public static List<Coordinate> GetNeighbours(
        this List<Coordinate> coordinates,
        Coordinate coordinate)
    {
        return coordinates
            .FindAll(x => x.Row == coordinate.Row && x.Col == coordinate.Col - 1 ||
                          x.Row == coordinate.Row && x.Col == coordinate.Col + 1 ||
                          x.Row == coordinate.Row - 1 && x.Col == coordinate.Col ||
                          x.Row == coordinate.Row + 1 && x.Col == coordinate.Col);
    }
}