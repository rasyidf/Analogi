using Analogi.Core.Pipeline;

if (args.Length == 0)
{
    Console.WriteLine("Analogi CLI - Code Clone Detector");
    Console.WriteLine("Usage: analogi <folder-path> [--top N]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --top N    Show top N results (default: 10)");
    return 1;
}

string path = args[0];
int top = 10;

for (int i = 1; i < args.Length; i++)
{
    if (args[i] == "--top" && i + 1 < args.Length && int.TryParse(args[i + 1], out int n))
    {
        top = n;
        i++;
    }
}

if (!Directory.Exists(path))
{
    Console.Error.WriteLine($"Error: '{path}' is not a valid directory.");
    return 1;
}

Console.WriteLine($"Scanning: {path}");
Console.WriteLine();

var engine = new AnalysisEngine();
var progress = new Progress<(int current, int total)>(p =>
{
    Console.Write($"\r  Comparing pair {p.current}/{p.total}...");
});

var result = await engine.ScanAsync(path, progress);
Console.WriteLine($"\r  Done. {result.TotalFiles} files, {result.Pairs.Count} pairs in {result.Duration.TotalMilliseconds:F0}ms.");
Console.WriteLine();

if (result.Pairs.Count == 0)
{
    Console.WriteLine("No similar pairs found.");
    return 0;
}

Console.WriteLine($"Top {Math.Min(top, result.Pairs.Count)} results:");
Console.WriteLine(new string('-', 70));

foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex).Take(top))
{
    Console.WriteLine($"  {pair.FileA.Name} <-> {pair.FileB.Name}: {pair.SimilarityPercent}% ({pair.Level})");
    foreach (var r in pair.Reasons)
        Console.WriteLine($"    [{r.AnalyzerName}] {r.Description}");
    Console.WriteLine();
}

return 0;
