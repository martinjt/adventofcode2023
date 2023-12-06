using System.Text.RegularExpressions;

var filename = Environment.GetCommandLineArgs().Length != 1 ?
    Environment.GetCommandLineArgs()[1] :
    "";
Console.WriteLine($"Reading from file: {filename}");
if (string.IsNullOrEmpty(filename))
{
    Console.WriteLine("Please provide a filename");
    return;
}

var lines = File.ReadAllLines(filename);

var total = 0;

for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
{
    var line = lines[lineNumber];
    var matches = Checks.NumberRegex().Matches(line);
    var isFirstLine = lineNumber == 0;
    var isLastLine = lineNumber == lines.Length - 1;
    var previousLine = lineNumber - 1;
    var nextLine = lineNumber + 1;
    foreach (Match match in matches)
    {
        Console.WriteLine($"Match: {match.Value}");
        if (match.NextCharacterIsSymbol(line) ||
            match.PreviousCharacterIsSymbol(line))
        {
            total += int.Parse(match.Value);
            Console.WriteLine("---- Match");
            continue;
        }

        var startIndex = match.Index == 0 ? 0 : match.Index - 1;
        var endIndex = match.Index + match.Length >= line.Length - 1 ?
            line.Length - 1 :
            match.Index + match.Length;

        var length = endIndex - startIndex + 1;

        if (!isLastLine)
        {
            var belowString = lines[nextLine].Substring(startIndex, length);
            Console.WriteLine($"Below: {belowString}");
            if (Checks.SymbolRegex().IsMatch(belowString))
            {
                total += int.Parse(match.Value);
                Console.WriteLine("---- Match");
                continue;
            }
        }

        if (!isFirstLine)
        {
            var aboveString = lines[previousLine].Substring(startIndex, length);
            Console.WriteLine($"Above: {aboveString}");
            if (Checks.SymbolRegex().IsMatch(aboveString))
            {
                total += int.Parse(match.Value);
                Console.WriteLine("---- Match");
                continue;
            }
        }
        Console.WriteLine("---- No match");
    }
}

Console.WriteLine($"Total: {total}");

Console.WriteLine("#########################");

var totalPartNumber = 0;
var matchedNumbers = new List<(int lineNumber, int start, int length)>();

for (var lineNumber = 0; lineNumber < lines.Length; lineNumber++)
{
    var line = lines[lineNumber];
    var lineMatches = Checks.NumberRegex().Matches(line);
    var isFirstLine = lineNumber == 0;
    var isLastLine = lineNumber == lines.Length - 1;
    var previousLine = lineNumber - 1;
    var nextLine = lineNumber + 1;

    var allAsterisks = Checks.AsteriskRegex().Matches(line);
    if (!allAsterisks.Any())
        continue;

    foreach (var asteriskIndex in allAsterisks.Select(x => x.Index))
    {

        Console.WriteLine($"Asterix Position: {asteriskIndex},{lineNumber}");


        var matchedPartNumbers = new List<int>();
        var matchedLocations = new List<(int lineNumber, int start, int length)>();
        if (lineMatches.Count != 0)
        {
            foreach (Match match in lineMatches)
            {
                if (match.Index < asteriskIndex && match.Index + match.Length == asteriskIndex)
                {
                    matchedPartNumbers.Add(int.Parse(match.Value));
                    matchedLocations.Add((lineNumber, match.Index, match.Length));
                    continue;
                }
                if (match.Index == asteriskIndex + 1)
                {
                    matchedPartNumbers.Add(int.Parse(match.Value));
                    matchedLocations.Add((lineNumber, match.Index, match.Length));
                    continue;
                }
            }
        }

        if (!isLastLine)
        {
            var nextLineMatches = Checks.NumberRegex().Matches(lines[nextLine]);
            foreach (Match nextLineMatch in nextLineMatches)
            {
                Console.WriteLine($"Next Line Match: {nextLineMatch.Value}, position: {nextLineMatch.Index},{nextLineMatch.Length}");
                if (nextLineMatch.Index > asteriskIndex + 1)
                {
                    continue;
                }
                if (nextLineMatch.Index + (nextLineMatch.Length - 1) < asteriskIndex - 1)
                {
                    continue;
                }
                Console.WriteLine($"{nextLineMatch.Value} is match");
                matchedPartNumbers.Add(int.Parse(nextLineMatch.Value));
                matchedLocations.Add((nextLine, nextLineMatch.Index, nextLineMatch.Length));

            }
        }
        if (!isFirstLine)
        {
            var previousLineMatches = Checks.NumberRegex().Matches(lines[previousLine]);
            foreach (Match previousLineMatch in previousLineMatches)
            {
                Console.WriteLine($"Previous Line Match: {previousLineMatch.Value}");

                if (previousLineMatch.Index > asteriskIndex + 1)
                {
                    continue;
                }
                if (previousLineMatch.Index + (previousLineMatch.Length - 1) < asteriskIndex - 1)
                {
                    continue;
                }
                Console.WriteLine($"{previousLineMatch.Value} is match");
                matchedPartNumbers.Add(int.Parse(previousLineMatch.Value));
                matchedLocations.Add((previousLine, previousLineMatch.Index, previousLineMatch.Length));
            }
        }
        if (matchedPartNumbers.Count() >= 2)
        {
            Console.WriteLine($"Gear Ratio {matchedPartNumbers.Aggregate((a, b) => a * b)}");
            totalPartNumber += matchedPartNumbers.Aggregate((a, b) => a * b);
            matchedNumbers.AddRange(matchedLocations);
        }

        Console.WriteLine("------------------------");
    }

}

Console.WriteLine($"Total Part Number: {totalPartNumber}");

internal static partial class Checks
{
    public static bool NextCharacterIsSymbol(this Match match, string line)
    {
        var nextChar = match.Index + match.Length;

        return nextChar < line.Length && Checks.SymbolRegex().IsMatch(line[nextChar].ToString());
    }
    public static bool PreviousCharacterIsSymbol(this Match match, string line)
    {
        if (match.Index == 0)
            return false;

        return Checks.SymbolRegex().IsMatch(line[match.Index - 1].ToString());
    }

    //generated regex for asterisk
    [GeneratedRegex(@"\*", RegexOptions.Compiled)]
    public static partial Regex AsteriskRegex();

    [GeneratedRegex(@"[-!$%^&*()_+|~=`{}\[\]:"";'<>?,\/#@]", RegexOptions.Compiled)]
    public static partial Regex SymbolRegex();
    [GeneratedRegex(@"\d+", RegexOptions.Compiled)]
    public static partial Regex NumberRegex();
}
