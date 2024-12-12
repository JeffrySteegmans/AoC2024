using System.Xml.Xsl;

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
        var stones = new Stones(input);

        for (var i = 0; i < 75; i++)
        {
            stones.Blink();
        }
        
        var answer = stones
            .Count
            .ToString();
        
        return Task.FromResult(answer);
    }
}

public class Stones
{
    public long Count => _stones.Sum(x => x.Value);
    
    private Dictionary<long, long> _stones = new();
    
    public Stones(
        IEnumerable<string> input)
    {
        input
            .First()
            .Split(' ')
            .Select(long.Parse)
            .ToList()
            .ForEach(x => _stones.TryAdd(x, 1));
    }

    // public void Blink()
    // {
    //     var numberOfStones = _stones.Count;
    //     
    //     for (var i = 0; i < numberOfStones; i++)
    //     {
    //         if (_stones[i].Value == 0)
    //         {
    //             _stones[i].Value++;
    //             continue;
    //         }
    //         
    //         var stoneDigits = GetDigits(_stones[i].Value).ToArray();
    //         if (stoneDigits.Length % 2 == 0)
    //         {
    //             var chunks = stoneDigits
    //                 .Chunk(stoneDigits.Length / 2)
    //                 .ToArray();
    //
    //             _stones[i].Value = chunks[0]
    //                 .Reverse()
    //                 .Aggregate((seed, digit) => seed * 10 + digit);
    //             
    //             _stones.Add(new Stone(chunks[1].Reverse().Aggregate((seed, digit) => seed * 10 + digit)));
    //             continue;
    //         }
    //
    //         _stones[i].Value *= 2024;
    //     }
    // }

    public void Blink()
    {
        var newStones = new Dictionary<long, long>();

        foreach (var stone in _stones)
        {
            if (stone.Key == 0)
            {
                newStones.TryGetValue(1, out var value1);
                newStones[1] = stone.Value + value1;
                continue;
            }

            var (left, right) = Split(stone.Key);
            if (left != long.MinValue && right != long.MinValue)
            {   
                newStones.TryGetValue(left, out var value2);
                newStones[left] = stone.Value + value2;
                
                newStones.TryGetValue(right, out var value3);
                newStones[right] = stone.Value + value3;
                continue;
            }

            var newStoneValue = stone.Key * 2024;
            newStones.TryGetValue(newStoneValue, out var value4);
            newStones[newStoneValue] = stone.Value + value4; 
        }
        
        _stones = newStones;
    }

    private static (long left, long right) Split(
        long number)
    {
        var digits = GetDigits(number).Reverse().ToArray();

        if (digits.Length % 2 != 0)
        {
            return (long.MinValue, long.MinValue);
        }

        var chunks = digits
            .Chunk(digits.Length / 2)
            .ToArray();
                
        var left = chunks[0]
            .Aggregate((seed, digit) => seed * 10 + digit);
        
        var right = chunks[1]
            .Aggregate((seed, digit) => seed * 10 + digit);
        
        return (left, right);
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