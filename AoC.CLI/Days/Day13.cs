using System.Drawing;
using AoC.CLI.Models;

namespace AoC.CLI.Days;

public class Day13
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var answer = input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Chunk(3)
            .Select(x => new ClawMachine(x))
            .Sum(clawMachine => clawMachine.CalculateLeastTokens())
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        var offset = 10_000_000_000_000;

        var answer = input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Chunk(3)
            .Select(x => new ClawMachine(x))
            .Sum(clawMachine => clawMachine.CalculateLeastTokens(offset))
            .ToString();

        return Task.FromResult(answer);
    }
}

internal class ClawMachine
{
    private Button _buttonA;
    private Button _buttonB;
    private Point _prizeLocation;

    public ClawMachine(
        IEnumerable<string> input)
    {
        var inputArray = input.ToArray();

        _buttonA = GetButton(inputArray[0]);
        _buttonB = GetButton(inputArray[1]);
        _prizeLocation = GetCoordinate(inputArray[2]);
    }

    private static Button GetButton(
        string input)
    {
        var actions = input
            .Split(":")[1]
            .Split(",")
            .Select(x => int.Parse(x.Trim().Split("+")[1]))
            .ToArray();

        return new Button(
            actions[0],
            actions[1]);
    }

    private static Point GetCoordinate(
        string input)
    {
        var coordinate = input
            .Split(":")[1]
            .Split(",")
            .Select(x => int.Parse(x.Trim().Split("=")[1]))
            .ToArray();

        return new Point(
            coordinate[0],
            coordinate[1]);
    }

    public long CalculateLeastTokens(
        long offset = 0)
    {
        var prizeY = _prizeLocation.Y + offset;
        var prizeX = _prizeLocation.X + offset;

        var buttonBClicks = ((prizeY * _buttonA.XAction) - (prizeX * _buttonA.YAction))/
                            ((_buttonA.XAction * _buttonB.YAction) - (_buttonA.YAction * _buttonB.XAction));
        var buttonAClicks = (prizeX - _buttonB.XAction * buttonBClicks)/_buttonA.XAction;

        if (offset == 0 && (buttonAClicks > 100 || buttonBClicks > 100))
        {
            return 0;
        }

        var xPrize = (buttonAClicks * _buttonA.XAction) + (buttonBClicks * _buttonB.XAction);
        var yPrize = (buttonAClicks * _buttonA.YAction) + (buttonBClicks * _buttonB.YAction);

        if (xPrize != prizeX || yPrize != prizeY)
        {
            return 0;
        }

        return buttonAClicks * 3 + buttonBClicks;
    }
}

internal record Button(
    int XAction,
    int YAction);