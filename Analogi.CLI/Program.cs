using Analogi.Core.Pipeline;

if (args.Length == 0)
{
    Console.WriteLine("Analogi CLI - Code Clone Detector");
    Console.WriteLine();
    Console.WriteLine("Usage:");
    Console.WriteLine("  analogi <folder-path> [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --submissions  Treat subfolders as student submissions (cross-file comparison)");
    Console.WriteLine("  --top N        Show top N results (default: 10)");
    Console.WriteLine();
    Console.WriteLine("Modes:");
    Console.WriteLine("  File mode (default): Compares individual files in the folder");
    Console.WriteLine("  Submission mode:     Compares entire subfolders as units");
    Console.WriteLine();
    Console.WriteLine("Supported languages: C/C++, Python, Java, C#, JavaScript/TypeScript");
    return 1;
}

string path = args[0];
int top = 10;
bool submissionMode = false;

for (int i = 1; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--submissions" or "-s":
            submissionMode = true;
            break;
        case "--top" or "-n" when i + 1 < args.Length && int.TryParse(args[i + 1], out int n):
            top = n;
            i++;
            break;
    }
}

if (!Directory.Exists(path))
{
    Console.Error.WriteLine($"Error: '{path}' is not a valid directory.");
    return 1;
}

var engine = new AnalysisEngine();
var progress = new Progress<(int current, int total)>(p =>
{
    Console.Write($"\r  Comparing pair {p.current}/{p.total}...");
});

if (submissionMode)
{
    Console.WriteLine($"Scanning submissions: {path}");
    Console.WriteLine();

    var result = await engine.ScanSubmissionsAsync(path, progress);
    Console.WriteLine($"\r  Done. {result.TotalSubmissions} submissions, {result.Pairs.Count} pairs in {result.Duration.TotalMilliseconds:F0}ms.");
    Console.WriteLine();

    if (result.Pairs.Count == 0)
    {
        Console.WriteLine("No similar submission pairs found.");
        return 0;
    }

    Console.WriteLine($"Top {Math.Min(top, result.Pairs.Count)} results:");
    Console.WriteLine(new string('-', 80));

    foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex).Take(top))
    {
        Console.WriteLine($"  {pair.SubmissionA.Name} <-> {pair.SubmissionB.Name}: {pair.SimilarityPercent}% ({pair.Level})");
        Console.WriteLine($"    Files: {pair.SubmissionA.Files.Count} vs {pair.SubmissionB.Files.Count} | Matching file pairs: {pair.FilePairDetails.Count}");
        foreach (var r in pair.Reasons)
            Console.WriteLine($"    [{r.AnalyzerName}] {r.Description}");
        Console.WriteLine();
    }
}
else
{
    Console.WriteLine($"Scanning files: {path}");
    Console.WriteLine();

    var result = await engine.ScanAsync(path, progress);
    Console.WriteLine($"\r  Done. {result.TotalFiles} files, {result.Pairs.Count} pairs in {result.Duration.TotalMilliseconds:F0}ms.");
    Console.WriteLine();

    if (result.Pairs.Count == 0)
    {
        Console.WriteLine("No similar pairs found.");
        return 0;
    }

    Console.WriteLine($"Top {Math.Min(top, result.Pairs.Count)} results:");
    Console.WriteLine(new string('-', 80));

    foreach (var pair in result.Pairs.OrderByDescending(p => p.SimilarityIndex).Take(top))
    {
        Console.WriteLine($"  {pair.FileA.Name} <-> {pair.FileB.Name}: {pair.SimilarityPercent}% ({pair.Level})");
        foreach (var r in pair.Reasons)
            Console.WriteLine($"    [{r.AnalyzerName}] {r.Description}");
        Console.WriteLine();
    }
}

return 0;
