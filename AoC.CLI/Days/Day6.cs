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
        return Task.FromResult("Not implemented yet.");
    }
}

internal static class Day6Extensions
{
    public static char[,] ParseMap(
        this IEnumerable<string> input)
    {
        var inputArray = input.ToArray();
        var map = new char[inputArray[0].Length, inputArray.Length];

        for (var rowIndex = 0; rowIndex < inputArray.Length; rowIndex++)
        {
            var line = inputArray[rowIndex].ToCharArray();
            for (var colIndex = 0; colIndex < line.Length; colIndex++)
            {
                map[rowIndex, colIndex] = line[colIndex];
            }
        }

        return map;
    }

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
}

internal record Coordinate(int Row, int Col);

internal enum Direction
{
    Up,
    Right,
    Down,
    Left
}
