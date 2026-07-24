using Analogi.Core.Interfaces;

namespace Analogi.Core.Models;

/// <summary>
/// Holds all data flowing through the pipeline for a single file-pair comparison.
/// </summary>
public sealed class PipelineContext
{
    public CodeFile FileA { get; }
    public CodeFile FileB { get; }
    public ILanguageProfile Language { get; }

    /// <summary>Extracted/transformed text data keyed by step name (e.g., "code.a", "comment.b").</summary>
    public Dictionary<string, List<string>> Metadata { get; } = new();

    /// <summary>Similarity reasons found by analyzers.</summary>
    public List<SimilarityReason> Reasons { get; } = [];

    public PipelineContext(CodeFile fileA, CodeFile fileB, ILanguageProfile language)
    {
        FileA = fileA;
        FileB = fileB;
        Language = language;
    }

    public void SetMetadata(string key, List<string> value) => Metadata[key] = value;

    public List<string> GetMetadata(string key) =>
        Metadata.TryGetValue(key, out var value) ? value : [];
}
