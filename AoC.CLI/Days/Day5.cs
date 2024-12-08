using AoC.CLI;

internal class Day5
  : IDay
{
    public Task<string> ExecutePart1(
      IEnumerable<string> input)
    {
        var answer = input
            .GetPageNumbers()
            .Validate(input.GetRules())
            .GetMiddlePageNumbers()
            .Sum();

        return Task.FromResult(answer.ToString());
    }

    public Task<string> ExecutePart2(
      IEnumerable<string> input)
    {
        return Task.FromResult("Not implemented");
    }
}

internal static class Extensions
{
    public static List<Rule> GetRules(
        this IEnumerable<string> input)
    {
        var rules = new List<Rule>();

        foreach (var line in input.Where(x => x.Contains('|')))
        {
            var rule = line.Split("|");
            var firstPageNumber = int.Parse(rule[0]);
            var secondPageNumber = int.Parse(rule[1]);

            rules.Add(
                new Rule(firstPageNumber, secondPageNumber));
        }

        return rules;
    }

    public static List<List<int>> GetPageNumbers(
        this IEnumerable<string> input)
    {
        var pageNumbers = new List<List<int>>();

        foreach (var item in input.Where(x => x.Contains(',')))
        {
            var pages = item
                .Split(",")
                .Select(int.Parse)
                .ToList();

            pageNumbers.Add(
                pages);
        }

        return pageNumbers;
    }

    public static List<List<int>> Validate(
        this List<List<int>> pageNumbers,
        List<Rule> rules)
    {
        var validPageNumbers = new List<List<int>>();

        foreach (var pages in pageNumbers)
        {
            var isValid = rules
                .Where(x => pages.Contains(x.firstPageNumber) && pages.Contains(x.secondPageNumber))
                .All(x => pages.IndexOf(x.firstPageNumber) < pages.IndexOf(x.secondPageNumber));

            if (isValid)
            {
                validPageNumbers.Add(pages);
            }
        }

        return validPageNumbers;
    }

    public static List<int> GetMiddlePageNumbers(
        this List<List<int>> pageNumbers)
    {
        var middlePageNumbers = new List<int>();

        foreach (var pages in pageNumbers)
        {
            var middlePageNumber = pages[pages.Count / 2];
            middlePageNumbers.Add(middlePageNumber);
        }

        return middlePageNumbers;
    }
}

internal record Rule(
    int firstPageNumber,
    int secondPageNumber);
