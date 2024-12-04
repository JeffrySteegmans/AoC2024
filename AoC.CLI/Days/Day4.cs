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

    public async Task<string> ExecutePart2(
      IEnumerable<string> input)
    {
      var matrix = CreateMatrix(input);
      
      var answer = await SearchAllXmas(
        matrix);

      return answer
        .ToString();
    }

    private async Task<int> SearchAllXmas(
      char[,] matrix) 
    {
      var total = 0;

      for (int i = 0; i < matrix.GetLength(0); i++) 
      {
        for (int j = 0; j < matrix.GetLength(1); j++) 
        {
          var character = matrix[i, j];
          
          if (character != 'A') {
            continue;
          }

          var xmasFound = await SearchXmas(
            matrix,
            i,
            j
          );
          if (xmasFound) {
            total++;
          }
        }
      }

      return total;
    }

    private Task<bool> SearchXmas(
      char[,] matrix,
      int rowIndex,
      int colIndex)
    {
      var rowLength = matrix.GetLength(0);
      var colLength = matrix.GetLength(1);

      if (OutOfBound(rowLength, colLength, rowIndex, colIndex)) {
        return Task.FromResult(false);
      }

      var leftUpCharacter = matrix[rowIndex-1, colIndex-1];
      var rightUpCharacter = matrix[rowIndex-1, colIndex+1];
      var leftDownCharacter = matrix[rowIndex+1, colIndex-1];
      var rightDownCharacter = matrix[rowIndex+1, colIndex+1];

      return Task.FromResult(IsMas(leftUpCharacter, rightDownCharacter) && IsMas(rightUpCharacter, leftDownCharacter));

      static bool IsMas(char firstCharacter, char secondCharacter)
      {
        if (firstCharacter == secondCharacter){
          return false;
        }

        return (firstCharacter == 'M' && secondCharacter == 'S') || (firstCharacter == 'S' && secondCharacter == 'M');
      }
      static bool OutOfBound(int rowLength, int colLength, int rowIndex, int colIndex) 
      {
        return rowIndex == 0 || colIndex == 0 || rowIndex >= rowLength - 1 || colIndex >= colLength - 1;
      }
    }
}