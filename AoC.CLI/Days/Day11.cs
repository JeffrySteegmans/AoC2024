namespace AoC.CLI.Days;

public class Day11
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var stones = new Stones(input);

        for (var i = 0; i < 25; i++)
        {
            stones.Blink();
        }
        
        var answer = stones
            .Count
            .ToString();
        
        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("Not Implemented");
    }
}

public class Stones
{
    public int Count => _stones.Count;
    
    private List<Stone> _stones = [];
    
    public Stones(
        IEnumerable<string> input)
    {
        _stones = input
            .First()
            .Split(' ')
            .Select(x => new Stone(int.Parse(x)))
            .ToList();
    }

    public void Blink()
    {
        var numberOfStones = _stones.Count;
        
        for (var i = 0; i < numberOfStones; i++)
        {
            if (_stones[i].Value == 0)
            {
                _stones[i].Value++;
                continue;
            }
            
            var stoneDigits = GetDigits(_stones[i].Value).ToArray();
            if (stoneDigits.Length % 2 == 0)
            {
                var chunks = stoneDigits
                    .Chunk(stoneDigits.Length / 2)
                    .ToArray();

                _stones[i].Value = chunks[0]
                    .Reverse()
                    .Aggregate((seed, digit) => seed * 10 + digit);
                
                _stones.Add(new Stone(chunks[1].Reverse().Aggregate((seed, digit) => seed * 10 + digit)));
                continue;
            }

            _stones[i].Value *= 2024;
        }
    }
    
    private static IEnumerable<long> GetDigits(
        long source)
    {
        while (source > 0)
        {
            var digit = source % 10;
            source /= 10;
            yield return digit;
        }
    }
}

public class Stone(long value)
{
    public long Value { get; set; } = value;
};