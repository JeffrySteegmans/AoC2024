using AoC.CLI.Models;

namespace AoC.CLI.Extensions;

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