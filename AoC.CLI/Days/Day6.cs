using AoC.CLI.Extensions;
using AoC.CLI.Models;

namespace AoC.CLI.Days;

internal class Day6
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = input
            .ParseMap()
            .Travel()
            .CountVisited();

        return Task.FromResult(answer.ToString());
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var answer = input
            .ParseMap()
            .PossibleInfiniteLoops();

        return Task.FromResult(answer.ToString());
    }
}

internal static class Day6Extensions
{
    public static Coordinate FindStart(
        this char[,] map)
    {
        for (var rowIndex = 0; rowIndex < map.GetLength(0); rowIndex++)
        {
            for (var colIndex = 0; colIndex < map.GetLength(1); colIndex++)
            {
                if (map[rowIndex, colIndex] == '^')
                {
                    return new(rowIndex, colIndex);
                }
            }
        }

        return new(-1, -1);
    }

    public static bool[,] Travel(
        this char[,] map)
    {
        var visited = new bool[map.GetLength(0), map.GetLength(1)];
        var currentCoordinate = map.FindStart();
        var direction = Direction.Up;

        while (map.InsideArea(currentCoordinate.Row, currentCoordinate.Col))
        {
            visited[currentCoordinate.Row, currentCoordinate.Col] = true;

            var nextCoordinate = GetCoordinate(currentCoordinate.Row, currentCoordinate.Col, direction);

            while (map.IsObstacle(nextCoordinate))
            {
                direction = direction.Turn();
                nextCoordinate = GetCoordinate(currentCoordinate.Row, currentCoordinate.Col, direction);
            };

            currentCoordinate = nextCoordinate;
        };

        return visited;
    }

    private static bool IsObstacle(
        this char[,] map,
        Coordinate coordinate)
    {
        return map.InsideArea(coordinate.Row, coordinate.Col) && 
            map[coordinate.Row, coordinate.Col] == '#';
    }

    private static Coordinate GetCoordinate(
        int row,
        int col,
        Direction direction)
    {
        return direction switch
        {
            Direction.Up => new(row-1, col),
            Direction.Right => new(row, col + 1),
            Direction.Down => new(row + 1, col),
            Direction.Left => new(row, col - 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private static bool InsideArea(
        this char[,] map,
        int x,
        int y)
    {
        return x >= 0 && x < map.GetLength(0) && 
            y >= 0 && y < map.GetLength(1);
    }

    private static Direction Turn(
        this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public static int CountVisited(
        this bool[,] visited)
    {
        var count = 0;

        for (var rowIndex = 0; rowIndex < visited.GetLength(0); rowIndex++)
        {
            for (var colIndex = 0; colIndex < visited.GetLength(1); colIndex++)
            {
                if (visited[rowIndex, colIndex])
                {
                    count++;
                }
            }
        }

        return count;
    }

    public static int PossibleInfiniteLoops(
        this char[,] map)
    {
        var startCoordinate = map.FindStart();

        var infiniteLoops = 0;
        for (var rowIndex = 0; rowIndex < map.GetLength(0); rowIndex++)
        {
            for (var colIndex = 0; colIndex < map.GetLength(1); colIndex++)
            {
                if (startCoordinate.Row == rowIndex && startCoordinate.Col == colIndex)
                {
                    continue;
                }

                if (map.IsInfinitLoop(new Coordinate(rowIndex, colIndex)))
                {
                    infiniteLoops++;
                }
            }
        }

        return infiniteLoops;
    }

    public static bool IsInfinitLoop(
        this char[,] map,
        Coordinate newObstacleCoordinate)
    {
        var currentCoordinate = map.FindStart();
        var direction = Direction.Up;
        var obstacleHistory = new List<(Coordinate coordinate, Direction direction)>();
        while (map.InsideArea(currentCoordinate.Row, currentCoordinate.Col))
        {
            var nextCoordinate = GetCoordinate(currentCoordinate.Row, currentCoordinate.Col, direction);

            while (nextCoordinate == newObstacleCoordinate || map.IsObstacle(nextCoordinate))
            {
                if (obstacleHistory.Any(x => x.coordinate == nextCoordinate && x.direction == direction))
                {
                    return true;
                }

                obstacleHistory.Add((nextCoordinate, direction));

                direction = direction.Turn();
                nextCoordinate = GetCoordinate(currentCoordinate.Row, currentCoordinate.Col, direction);
            };

            currentCoordinate = nextCoordinate;
        };

        return false;
    }
}

internal enum Direction
{
    Up,
    Right,
    Down,
    Left
}
