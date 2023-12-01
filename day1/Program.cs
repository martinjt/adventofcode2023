string filename = Environment.GetCommandLineArgs()[1];
if (string.IsNullOrEmpty(filename))
{
    Console.WriteLine("Please provide a filename");
    return;
}

var inputs = File.ReadAllLines(filename);
var firstValues = new List<int>();

foreach (var input in inputs) {
    double? first = null;
    double? last = null;

    foreach (var character in input.AsSpan()) {
        if (char.IsDigit(character) ) {
            var number = char.GetNumericValue(character);
            if (first == null) {
                first = number;
                last = number;
            } else {
                last = number;
            }
        }
    }
    var output =  int.Parse($"{first}{last}");
    firstValues.Add(output);
}

var numberStrings = new [] {
    "one", "two", "three", "four",
    "five", "six", "seven", "eight", "nine"
};

var secondValues = new List<int>();

foreach (var input in inputs) {
    double? first = null;
    double? last = null;

    for (int i = 0; i < input.Length; i++) {
        if (char.IsDigit(input[i]) ) {
            var number = char.GetNumericValue(input[i]);
            if (first == null) {
                first = number;
                last = number;
            } else {
                last = number;
            }
            continue;
        }
        for (int j = 0; j < numberStrings.Length; j++) {
            var numberString = numberStrings[j];
            var numberValue = j + 1;
            if (input[i..].StartsWith(numberString)) {
                if (first == null) {
                    first = numberValue;
                    last = numberValue;
                } else {
                    last = numberValue;
                }
                break;
            }
        }
    }

    var output =  int.Parse($"{first}{last}");
    secondValues.Add(output);
}

Console.WriteLine($"Puzzle 1 Sum: {firstValues.Sum()}");
Console.WriteLine($"Puzzle 2 Sum: {secondValues.Sum()}");