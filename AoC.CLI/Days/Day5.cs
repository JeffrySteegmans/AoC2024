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
        var rules = input.GetRules();

        var answer = input
            .GetPageNumbers()
            .GetInvalid(rules)
            .ApplyRules(rules)
            .GetMiddlePageNumbers()
            .Sum();

        return Task.FromResult(answer.ToString());
    }
}

internal static class Day5Extensions
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

    public static List<List<int>> GetInvalid(
        this List<List<int>> pageNumbers,
        List<Rule> rules)
    {
        var validPageNumbers = new List<List<int>>();

        foreach (var pages in pageNumbers)
        {
            var isValid = rules
                .Where(x => pages.Contains(x.firstPageNumber) && pages.Contains(x.secondPageNumber))
                .All(x => pages.IndexOf(x.firstPageNumber) < pages.IndexOf(x.secondPageNumber));

            if (!isValid)
            {
                validPageNumbers.Add(pages);
            }
        }

        return validPageNumbers;
    }

    public static List<List<int>> ApplyRules(
        this List<List<int>> pageNumbers,
        List<Rule> rules)
    {
        var validPageNumbers = new List<List<int>>();

        foreach (var page in pageNumbers)
        {
            var aplicableRules = rules
                .Where(x => page.Contains(x.firstPageNumber) && page.Contains(x.secondPageNumber));

            List<int> orderedPage = [.. page];

            do
            {
                foreach (var rule in aplicableRules)
                {
                    orderedPage = orderedPage.ApplyRule(rule);
                }
            } while (orderedPage.IsInvalid(aplicableRules));

            validPageNumbers.Add(orderedPage);
        }

        return validPageNumbers;
    }

    private static bool IsInvalid(
        this List<int> page,
        IEnumerable<Rule> rules)
    {
        return rules
            .Any(x => page.Contains(x.firstPageNumber) && page.Contains(x.secondPageNumber) && page.IndexOf(x.firstPageNumber) > page.IndexOf(x.secondPageNumber));
    }

    private static List<int> ApplyRule(
    this List<int> page,
    Rule rule)
    {
        var indexA = page.IndexOf(rule.firstPageNumber);
        var indexB = page.IndexOf(rule.secondPageNumber);

        if (indexA > indexB)
        {
            (page[indexA], page[indexB]) = (page[indexB], page[indexA]);
        }

        return page;
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
