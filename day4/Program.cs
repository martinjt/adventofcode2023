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
var numberRegex = new Regex(@"\d+");

var runningTotal = 0;

foreach (var line in lines)
{
    var game = line.Split(":");
    var splitNumbers = game[1].Split("|");
    var winningNumbers = numberRegex.Matches(splitNumbers[0]).Select(m => int.Parse(m.Value));
    var myNumbers = numberRegex.Matches(splitNumbers[1]).Select(m => int.Parse(m.Value));
    var matches = myNumbers.Where(my => winningNumbers.Contains(my));
    if (matches.Any())
    {
        var total = 1;
        for (var i = 0; i < matches.Count() - 1; i++)
        {
            total *= 2;
        }
        runningTotal += total;
    }
}

Console.WriteLine($"Total: {runningTotal}");
