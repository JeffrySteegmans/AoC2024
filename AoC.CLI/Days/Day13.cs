using System.Drawing;
using AoC.CLI.Models;

namespace AoC.CLI.Days;

public class Day13
    : IDay
{
    public Task<string> ExecutePart1(
        IEnumerable<string> input)
    {
        var clawMachines = input
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Chunk(3)
            .Select(x => new ClawMachine(x));

        var answer = clawMachines
            .Sum(clawMachine => clawMachine.CalculateLeastTokens())
            .ToString();

        return Task.FromResult(answer);
    }

    public Task<string> ExecutePart2(
        IEnumerable<string> input)
    {
        return Task.FromResult("No implemented");
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

    public int CalculateLeastTokens()
    {
        for (var i = 100; i > 0; i--)
        {
            var x = i * _buttonB.XAction;
            var y = i * _buttonB.YAction;

            if (x > _prizeLocation.X ||
                y > _prizeLocation.Y)
            {
                continue;
            }

            if ((_prizeLocation.X - x) % _buttonA.XAction > 0 ||
                (_prizeLocation.Y - y) % _buttonA.YAction > 0)
            {
                continue;
            }

            var buttonAClicksX = (_prizeLocation.X - x) / _buttonA.XAction;
            var buttonAClicksY = (_prizeLocation.Y - y) / _buttonA.YAction;

            if (buttonAClicksX != buttonAClicksY)
            {
                continue;
            }

            if (buttonAClicksX > 100 ||
                buttonAClicksY > 100)
            {
                return 0;
            }

            var buttonBClicks = i;

            return buttonAClicksX * 3 + buttonBClicks;
        }

        return 0;
    }
}

internal record Button(
    int XAction,
    int YAction);