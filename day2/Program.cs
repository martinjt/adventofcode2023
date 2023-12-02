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

var games = new List<Game>();
var lines = File.ReadAllLines(filename);
foreach (var line in lines)
{
    games.Add(Game.LoadGame(line));
}

var test = new List<(string colour, int colourCount)>
{
    ("red", 12),
    ("blue", 14),
    ("green", 13)
};

foreach (var game in games)
{
    Console.WriteLine($"Game {game.GameNumber} {game}");
}

var sumOfGames = games
    .Where(game => game.CanHaveGame(test))
    .Sum(game => game.GameNumber);

var powerOfGames = games
    .Select(g => g.MaxColourRequirements.Values.Aggregate(1, (acc, val) => acc * val))
    .Sum();


Console.WriteLine($"Sum of games: {sumOfGames}");
Console.WriteLine($"Power of games: {powerOfGames}");


class Game
{
    static Regex _gameRegex = new(@"Game (?<gameNumber>\d+): (?<turns>.*)", RegexOptions.Compiled);
    static Regex _colourRegex = new(@"(?<count>\d+) (?<colour>.*)", RegexOptions.Compiled);
    public int GameNumber { get; set; }
    public int TotalTurns { get; set; }
    public Dictionary<string, int> MaxColourRequirements { get; set; } = [];

    public static Game LoadGame(string gameLine)
    {
        var gameInfo = _gameRegex.Match(gameLine);
        var gameNumber = int.Parse(gameInfo.Groups["gameNumber"].Value);
        var turns = gameInfo.Groups["turns"].Value.Split(";");
        var game = new Game
        {
            GameNumber = gameNumber,
            TotalTurns = turns.Length
        };

        foreach (var turn in turns)
        {
            var colours = turn.Split(",");
            foreach (var colourCount in colours)
            {
                var colourInfo = _colourRegex.Match(colourCount);
                var colour = colourInfo.Groups["colour"].Value;
                int.TryParse(colourInfo.Groups["count"].Value, out var countOfColourInTurn);
                if (!game.MaxColourRequirements.TryGetValue(colour, out var currentCount) ||
                    currentCount < countOfColourInTurn)
                {
                    game.MaxColourRequirements[colour] = countOfColourInTurn;
                }
            }

        }
        return game;
    }

    public bool CanHaveGame(List<(string colour, int colourCount)> colours)
    {
        foreach (var colour in colours)
        {
            if (!MaxColourRequirements.TryGetValue(colour.colour, out var maxColourRequirement) ||
                maxColourRequirement > colour.colourCount)
            {
                return false;
            }
        }
        return true;
    }

    public override string ToString()
    {
        return $"Game {GameNumber}: {TotalTurns} turns, {string.Join(", ", MaxColourRequirements.Select(kvp => $"{kvp.Value} {kvp.Key}"))}";
    }
}
