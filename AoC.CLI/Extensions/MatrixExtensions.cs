using AoC.CLI.Models;

namespace AoC.CLI.Extensions;

internal static class MatrixExtensions
{
    public static bool IsInside(
        this char[,] garden,
        Coordinate coordinate)
    {
        return coordinate.Row >= 0 && coordinate.Row < garden.GetLength(0) &&
               coordinate.Col >= 0 && coordinate.Col < garden.GetLength(1);
    }
}