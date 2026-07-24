using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Analyzers;

/// <summary>
/// Compares file sizes as a ratio. Near-identical size is suspicious.
/// </summary>
public sealed class FileSizeAnalyzer : IPipelineStep
{
    public string Name => "FileSizeRatio";
    public bool IsEnabled { get; set; } = true;
    private const double Threshold = 0.95;

    public PipelineContext Run(PipelineContext ctx)
    {
        long a = ctx.FileA.Length;
        long b = ctx.FileB.Length;
        if (a == 0 && b == 0) return ctx;

        double ratio = 1.0 - (double)Math.Abs(a - b) / Math.Max(a, b);
        if (ratio >= Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"File size ratio: {ratio:P0} ({a} vs {b} bytes)",
                ratio,
                Weight: 0.3));
        }
        return ctx;
    }
}

/// <summary>
/// Compares code line counts. Identical line count is a weak signal.
/// </summary>
public sealed class LineCountAnalyzer : IPipelineStep
{
    public string Name => "LineCountMatch";
    public bool IsEnabled { get; set; } = true;
    private const double Threshold = 0.95;

    public PipelineContext Run(PipelineContext ctx)
    {
        int a = ctx.GetMetadata("code.a").Count;
        int b = ctx.GetMetadata("code.b").Count;
        if (a == 0 && b == 0) return ctx;

        double ratio = 1.0 - (double)Math.Abs(a - b) / Math.Max(a, b);
        if (ratio >= Threshold)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Code line count ratio: {ratio:P0} ({a} vs {b} lines)",
                ratio,
                Weight: 0.2));
        }
        return ctx;
    }
}
