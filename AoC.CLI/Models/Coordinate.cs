using AoC.CLI.Enums;

namespace AoC.CLI.Models;

internal record Coordinate(
    int Row,
    int Col)
{
    public override string ToString()
    {
        return $"({Row}, {Col})";
    }

    public Coordinate Move(
        Direction direction)
    {
        return direction switch
        {
            Direction.Up => this with{ Row = Row - 1 },
            Direction.Right => this with{ Col = Col + 1 },
            Direction.Down => this with{ Row = Row + 1 },
            Direction.Left => this with{ Col = Col - 1 },
            _ => throw new InvalidOperationException()
        };
    }
};
