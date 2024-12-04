using AoC.CLI;

internal class Day4
    : IDay
{
    public async Task<string> ExecutePart1(
      IEnumerable<string> input)
    {
      var target = "XMAS";
      var matrix = CreateMatrix(input);
      
      var answer = await SearchAll(
        matrix,
        target);

      return answer
        .ToString();
    }

    private async Task<int> SearchAll(
      char[,] matrix,
      string target)
    {
      var total = 0;

      for (int i = 0; i < matrix.GetLength(0); i++) 
      {
        for (int j = 0; j < matrix.GetLength(1); j++) 
        {
          var character = matrix[i, j];

          if (character != target[0]) {
            continue;
          }

          var results = await Task.WhenAll(
            Search(matrix, target, i, j, 0, +1),
            Search(matrix, target, i, j, +1, +1),
            Search(matrix, target, i, j, +1, 0),
            Search(matrix, target, i, j, +1, -1),
            Search(matrix, target, i, j, 0, -1),
            Search(matrix, target, i, j, -1, -1),
            Search(matrix, target, i, j, -1, 0),
            Search(matrix, target, i, j, -1, +1)
          );

          total += results.Count(x => x == true);
        }
      }

      return total;
    }

    private Task<bool> Search(
      char[,] matrix,
      string target,
      int startRow,
      int startCol,
      int rowOperation,
      int colOperation) 
    {
      var rowLength = matrix.GetLength(0);
      var colLength = matrix.GetLength(1);

      var currentRow = startRow;
      var currentCol = startCol;

      foreach (var character in target)
      {
        if (OutOfBound(rowLength, colLength, currentRow, currentCol))
        {
          return Task.FromResult(false);
        }

        if (matrix[currentRow, currentCol] != character)
        {
          return Task.FromResult(false);
        }

        currentRow += rowOperation;
        currentCol += colOperation;
      }

      return Task.FromResult(true);

      static bool OutOfBound(int rowLength, int colLength, int currentRow, int currentCol)
      {
        return currentRow < 0 || rowLength <= currentRow || currentCol < 0 || colLength <= currentCol;
      }
    }

    private char[,] CreateMatrix(
      IEnumerable<string> input)
    {
      var inputArray = input.ToArray();
      var matrix = new char[inputArray[0].Count(), inputArray.Count()];

      for (var i = 0; i < inputArray.Count(); i++) {
        var line = inputArray[i].ToCharArray();
        for (var j = 0; j < line.Count(); j++) {
          matrix[i, j] = line[j];
        }
      }

      return matrix;  
    }

    public Task<string> ExecutePart2(
      IEnumerable<string> input)
    {
        return Task.FromResult("Not implemented");
    }
}