using Analogi.Core.Analyzers;
using Analogi.Core.Extractors;
using Analogi.Core.Interfaces;
using Analogi.Core.Languages;
using Analogi.Core.Models;
using Analogi.Core.PreProcessors;

namespace Analogi.Core.Pipeline;

public sealed class AnalysisEngine
{
    private readonly LanguageRegistry _languages;
    private readonly List<IPipelineStep> _steps;

    public AnalysisEngine(LanguageRegistry? languages = null, List<IPipelineStep>? steps = null)
    {
        _languages = languages ?? new LanguageRegistry();
        _steps = steps ?? DefaultPipeline();
    }

    /// <summary>
    /// Run a full scan on the given path. Compares all files pairwise (i &lt; j only).
    /// </summary>
    public async Task<ScanResult> ScanAsync(
        string path,
        IProgress<(int current, int total)>? progress = null,
        CancellationToken ct = default)
    {
        return await Task.Run(() => ScanCore(path, progress, ct), ct);
    }

    private ScanResult ScanCore(
        string path,
        IProgress<(int current, int total)>? progress,
        CancellationToken ct)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var files = ScanFiles(path);
        int totalPairs = files.Count * (files.Count - 1) / 2;
        int done = 0;

        var results = new List<FilePairResult>();

        for (int i = 0; i < files.Count; i++)
        {
            for (int j = i + 1; j < files.Count; j++)
            {
                ct.ThrowIfCancellationRequested();

                var pair = ComparePair(files[i], files[j]);
                if (pair != null)
                    results.Add(pair);

                done++;
                progress?.Report((done, totalPairs));
            }
        }

        sw.Stop();
        return new ScanResult(results, files.Count, sw.Elapsed);
    }

    private FilePairResult? ComparePair(CodeFile a, CodeFile b)
    {
        // Both files must share a language profile
        var extA = Path.GetExtension(a.Path);
        var extB = Path.GetExtension(b.Path);
        var lang = _languages.GetByExtension(extA);
        if (lang == null || _languages.GetByExtension(extB) != lang) return null;

        var ctx = new PipelineContext(a, b, lang);
        foreach (var step in _steps)
        {
            ctx = step.Run(ctx);
        }

        return ctx.Reasons.Count > 0
            ? new FilePairResult(a, b, ctx.Reasons)
            : null;
    }

    private List<CodeFile> ScanFiles(string path)
    {
        var extensions = new HashSet<string>(_languages.AllExtensions, StringComparer.OrdinalIgnoreCase);
        var files = new List<CodeFile>();

        if (File.Exists(path)) return [new CodeFile(path)];

        // Check if this is a folder of subfolders (student submissions) or flat files
        var directFiles = Directory.EnumerateFiles(path)
            .Where(f => extensions.Contains(Path.GetExtension(f)))
            .ToList();

        if (directFiles.Count > 0)
        {
            files.AddRange(directFiles.Select(f => new CodeFile(f)));
        }
        else
        {
            // Subfolder mode: each subfolder is one "submission", take first matching file
            foreach (var dir in Directory.EnumerateDirectories(path))
            {
                var first = Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories)
                    .FirstOrDefault(f => extensions.Contains(Path.GetExtension(f)));
                if (first != null)
                    files.Add(new CodeFile(first));
            }
        }

        return files;
    }

    public static List<IPipelineStep> DefaultPipeline() =>
    [
        // Extraction
        new CodeExtractor(),
        new CommentExtractor(),
        new StructureExtractor(),
        // Preprocessing
        new CaseFold(),
        new WhitespaceNormalize(),
        // Analysis
        new CosineSimilarityAnalyzer(),
        new StructureAnalyzer(),
        new FileSizeAnalyzer(),
        new LineCountAnalyzer(),
    ];
}
