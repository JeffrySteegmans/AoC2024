using AoC.CLI.Enums;
using AoC.CLI.Extensions;
using AoC.CLI.Models;

namespace AoC.CLI.Days;

public class Day10
    :IDay
{
    public async Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var map = new TopographicMap(input);

        var trailHeads = await map.GetTrailHeads();

        var answer = trailHeads
            .Sum(x => x.Score)
            .ToString();

        return answer;
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var answer = "Not implemented";

        return Task.FromResult(answer);
    }
}

internal class TopographicMap
{
    private readonly int[,] _map;
    private List<Coordinate> _trailEnds;

    public TopographicMap(
        IEnumerable<string> input)
    {
        _map = input
            .ParseMap<int>();

        _trailEnds = GetTrailEnds();
    }

    public async Task<List<TrailHead>> GetTrailHeads()
    {
        var trailHeads = new List<TrailHead>();

        for (var row = 0; row < _map.GetLength(0); row++)
        {
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                if (_map[row, col] == 0)
                {
                    trailHeads
                        .Add(new TrailHead(new Coordinate(row, col), 0));
                }
            }
        }

        return await CalculateTrailHeadScores(trailHeads);
    }

    private List<Coordinate> GetTrailEnds()
    {
        var trailEnds = new List<Coordinate>();

        for (var row = 0; row < _map.GetLength(0); row++)
        {
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                if (_map[row, col] == 9)
                {
                    trailEnds
                        .Add(new Coordinate(row, col));
                }
            }
        }

        return trailEnds;
    }

    private async Task<List<TrailHead>> CalculateTrailHeadScores(
        List<TrailHead> trailHeads)
    {
        var trailHeadsWithScore = new List<TrailHead>();

        var options = new ParallelOptions { MaxDegreeOfParallelism = 6 };
        await Parallel.ForEachAsync(trailHeads, options, async (trailHead, _) =>
        {
            var trailHeadScore = await CalculateTrailHeadScore(trailHead);
            trailHeadsWithScore.Add(
                trailHead with { Score = trailHeadScore});
        });

        return trailHeadsWithScore;
    }

    private async Task<int> CalculateTrailHeadScore(
        TrailHead trailHead)
    {
        var score = 0;

        foreach (var trailEnd in _trailEnds)
        {
            var isHike = await IsHike(trailHead.Coordinate, trailEnd);
            if (isHike)
            {
                score++;
            }
        }

        return score;
    }

    private async Task<bool> IsHike(
        Coordinate start,
        Coordinate end)
    {
        Task<bool>[] tasks =
        [
            Task.Run(() => IsHike(start, end, Direction.Right)),
            Task.Run(() => IsHike(start, end, Direction.Down)),
            Task.Run(() => IsHike(start, end, Direction.Left)),
            Task.Run(() => IsHike(start, end, Direction.Up))
        ];

        var results = await Task.WhenAll(tasks);

        return results
            .Any(x => x);
    }

    private async Task<bool> IsHike(
        Coordinate currentLocation,
        Coordinate endLocation,
        Direction direction)
    {
        if (currentLocation == endLocation)
        {
            return true;
        }

        var nextLocation = currentLocation.Move(direction);
        if (IsInvalidLocation(nextLocation))
        {
            return false;
        }

        var currentHeight = GetHeight(currentLocation);
        var nextHeight = GetHeight(nextLocation);
        if (nextHeight - currentHeight != 1)
        {
            return false;
        }

        Task<bool>[] tasks =
        [
            Task.Run(() => IsHike(nextLocation, endLocation, Direction.Right)),
            Task.Run(() => IsHike(nextLocation, endLocation, Direction.Down)),
            Task.Run(() => IsHike(nextLocation, endLocation, Direction.Left)),
            Task.Run(() => IsHike(nextLocation, endLocation, Direction.Up))
        ];

        var results = await Task.WhenAll(tasks);

        return results
            .Any(x => x);
    }

    // private int Move(
    //     int currentScore,
    //     int currentHeight,
    //     Coordinate currentCoordinate,
    //     Direction direction)
    // {
    //     var nextCoordinate = currentCoordinate.Move(direction);
    //     if (IsInvalidCoordinate(nextCoordinate))
    //     {
    //         return currentScore;
    //     }
    //
    //     var nextHeight = GetHeight(nextCoordinate);
    //
    //     if (nextHeight == 9)
    //     {
    //         return ++currentScore;
    //     }
    //
    //     if (nextHeight - currentHeight == 1)
    //     {
    //         var score = Move(currentScore, nextHeight, nextCoordinate, Direction.Right);
    //         score += Move(currentScore, nextHeight, nextCoordinate, Direction.Down);
    //         score += Move(currentScore, nextHeight, nextCoordinate, Direction.Left);
    //         score += Move(currentScore, nextHeight, nextCoordinate, Direction.Up);
    //
    //         return currentScore + score;
    //     }
    //
    //     return currentScore;
    // }

    private bool IsInvalidLocation(
        Coordinate coordinate)
    {
        return coordinate.Row < 0 || coordinate.Row >= _map.GetLength(0) ||
               coordinate.Col < 0 || coordinate.Col >= _map.GetLength(1);
    }

    private int GetHeight(Coordinate coordinate)
    {
        return _map[coordinate.Row, coordinate.Col];
    }
}

internal record TrailHead(
    Coordinate Coordinate,
    int Score);