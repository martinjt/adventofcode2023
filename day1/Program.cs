var filename = Environment.GetCommandLineArgs().Count() != 1 ?
    Environment.GetCommandLineArgs()[1] :
    "";
Console.WriteLine($"Reading from file: {filename}");
if (string.IsNullOrEmpty(filename))
{
    Console.WriteLine("Please provide a filename");
    return;
}

var inputs = File.ReadAllLines(filename);

if (!inputs.Any())
{
    Console.WriteLine("No inputs found");
    return;
}
var numberStrings = new[] {
    "one", "two", "three", "four",
    "five", "six", "seven", "eight", "nine"
};

var firstValues = GetValues(inputs, false);
var secondValues = GetValues(inputs, true);

Console.WriteLine($"Puzzle 1 Sum: {firstValues.Sum()}");
Console.WriteLine($"Puzzle 2 Sum: {secondValues.Sum()}");

List<int> GetValues(string[] inputs, bool supportNumberStrings)
{
    var values = new List<int>();
    foreach (var input in inputs)
    {
        double? first = null;
        double? last = null;

        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(input[i]))
            {
                var number = char.GetNumericValue(input[i]);
                if (first == null)
                {
                    first = number;
                    last = number;
                }
                else
                {
                    last = number;
                }
                continue;
            }

            if (!supportNumberStrings)
                continue;

            for (int j = 0; j < numberStrings.Length; j++)
            {
                var numberString = numberStrings[j];
                var numberValue = j + 1;
                if (input[i..].StartsWith(numberString))
                {
                    if (first == null)
                    {
                        first = numberValue;
                        last = numberValue;
                    }
                    else
                    {
                        last = numberValue;
                    }
                    break;
                }
            }
        }

        var output = int.Parse($"{first}{last}");
        values.Add(output);
    }
    return values;

}