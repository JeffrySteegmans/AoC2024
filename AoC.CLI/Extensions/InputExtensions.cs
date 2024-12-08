namespace AoC.CLI.Extensions;

internal static class InputExtensions
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
}
