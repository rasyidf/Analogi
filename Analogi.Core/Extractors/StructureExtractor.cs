using Analogi.Core.Interfaces;
using Analogi.Core.Models;

namespace Analogi.Core.Extractors;

/// <summary>
/// Extracts structural elements: function names and import statements.
/// </summary>
public sealed class StructureExtractor : IPipelineStep
{
    public string Name => "StructureExtractor";

    public PipelineContext Run(PipelineContext ctx)
    {
        ctx.SetMetadata("functions.a", ExtractFunctions(ctx.FileA.Content, ctx.Language));
        ctx.SetMetadata("functions.b", ExtractFunctions(ctx.FileB.Content, ctx.Language));
        ctx.SetMetadata("imports.a", ExtractImports(ctx.FileA.Content, ctx.Language));
        ctx.SetMetadata("imports.b", ExtractImports(ctx.FileB.Content, ctx.Language));
        return ctx;
    }

    private static List<string> ExtractFunctions(string content, ILanguageProfile lang) =>
        lang.FunctionDeclaration.Matches(content)
            .Select(m => m.Groups["name"].Value)
            .Where(n => n.Length > 0)
            .ToList();

    private static List<string> ExtractImports(string content, ILanguageProfile lang) =>
        lang.ImportStatement.Matches(content)
            .Select(m => m.Groups["file"].Value)
            .Where(n => n.Length > 0)
            .ToList();
}
