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

        await map.CalculateScores();

        var answer = map.TrailHeads
            .Sum(x => x.Score)
            .ToString();

        return answer;
    }

    public async Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var map = new TopographicMap(input);

        await map.CalculateRatings();

        var answer = map.TrailHeads
            .Sum(x => x.Rating)
            .ToString();

        return answer;
    }
}

internal class TopographicMap
{
    private readonly int[,] _map;
    private List<TrailHead> _trailHeads;
    private readonly List<Coordinate> _trailEnds;

    public IReadOnlyCollection<TrailHead> TrailHeads => _trailHeads;

    public TopographicMap(
        IEnumerable<string> input)
    {
        _map = input
            .ParseMap<int>();

        _trailHeads = GetTrailHeads();
        _trailEnds = GetTrailEnds();
    }

    private List<TrailHead> GetTrailHeads()
    {
        var trailHeads = new List<TrailHead>();

        for (var row = 0; row < _map.GetLength(0); row++)
        {
            for (var col = 0; col < _map.GetLength(1); col++)
            {
                if (_map[row, col] == 0)
                {
                    trailHeads
                        .Add(new TrailHead(new Coordinate(row, col), 0, 0));
                }
            }
        }

        return trailHeads;
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

    public async Task CalculateScores()
    {
        _trailHeads = await CalculateTrailHeadScores();
    }

    private async Task<List<TrailHead>> CalculateTrailHeadScores()
    {
        var trailHeadsWithScore = new List<TrailHead>();

        var options = new ParallelOptions { MaxDegreeOfParallelism = 6 };
        await Parallel.ForEachAsync(_trailHeads, options, async (trailHead, _) =>
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

    public async Task CalculateRatings()
    {
        _trailHeads = await CalculateTrailHeadRatings();
    }

    private async Task<List<TrailHead>> CalculateTrailHeadRatings()
    {
        var trailHeadsWithRating = new List<TrailHead>();

        var options = new ParallelOptions { MaxDegreeOfParallelism = 6 };
        await Parallel.ForEachAsync(_trailHeads, options, async (trailHead, _) =>
        {
            var trailHeadRating = await CalculateTrailHeadRating(trailHead);
            trailHeadsWithRating.Add(
                trailHead with { Rating = trailHeadRating});
        });

        return trailHeadsWithRating;
    }

    private async Task<int> CalculateTrailHeadRating(
        TrailHead trailHead)
    {
        Task<int>[] tasks =
        [
            Task.Run(() => Move(0, trailHead.Coordinate, Direction.Right)),
            Task.Run(() => Move(0, trailHead.Coordinate, Direction.Down)),
            Task.Run(() => Move(0, trailHead.Coordinate, Direction.Left)),
            Task.Run(() => Move(0, trailHead.Coordinate, Direction.Up))
        ];

        var results = await Task.WhenAll(tasks);

        return results
            .Sum(x => x);
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

    private async Task<int> Move(
        int rating,
        Coordinate currentLocation,
        Direction direction)
    {
        var nextLocation = currentLocation.Move(direction);
        if (IsInvalidLocation(nextLocation))
        {
            return rating;
        }

        var currentHeight = GetHeight(currentLocation);
        var nextHeight = GetHeight(nextLocation);
        if (nextHeight - currentHeight != 1)
        {
            return rating;
        }

        if (nextHeight == 9)
        {
            rating += 1;
            return rating;
        }

        Task<int>[] tasks =
        [
            Task.Run(() => Move(0, nextLocation, Direction.Right)),
            Task.Run(() => Move(0, nextLocation, Direction.Down)),
            Task.Run(() => Move(0, nextLocation, Direction.Left)),
            Task.Run(() => Move(0, nextLocation, Direction.Up))
        ];

        var results = await Task.WhenAll(tasks);

        return rating + results
            .Sum(x => x);
    }

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
    int Score,
    int Rating);