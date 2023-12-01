string filename = Environment.GetCommandLineArgs()[1];
if (string.IsNullOrEmpty(filename))
{
    Console.WriteLine("Please provide a filename");
    return;
}

var inputs = File.ReadAllLines(filename);
var values = new List<int>();

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
    values.Add(output);
}

Console.WriteLine($"Sum: {values.Sum()}");