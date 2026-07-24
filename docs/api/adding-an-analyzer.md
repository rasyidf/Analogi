---
layout: default
title: Adding an Analyzer
parent: API Reference
nav_order: 2
---

# Adding a New Analyzer

All pipeline steps (extractors, preprocessors, analyzers) implement the same interface:

```csharp
public interface IPipelineStep
{
    string Name { get; }
    PipelineContext Run(PipelineContext context);
}
```

## The PipelineContext

```csharp
public sealed class PipelineContext
{
    public CodeFile FileA { get; }
    public CodeFile FileB { get; }
    public ILanguageProfile Language { get; }

    // Extracted data (populated by extractors, transformed by preprocessors)
    public Dictionary<string, List<string>> Metadata { get; }

    // Results (populated by analyzers)
    public List<SimilarityReason> Reasons { get; }

    // Safe accessor
    public List<string> GetMetadata(string key);
    public void SetMetadata(string key, List<string> value);
}
```

## Available Metadata Keys

After the default extractors run, these keys are available:

| Key | Content |
|-----|---------|
| `code.a`, `code.b` | Clean code lines (comments stripped) |
| `comment.a`, `comment.b` | Extracted comments |
| `functions.a`, `functions.b` | Function/method names |
| `imports.a`, `imports.b` | Import/include statements |

## Example: Variable Count Analyzer

```csharp
public sealed class VariableCountAnalyzer : IPipelineStep
{
    public string Name => "VariableCount";

    public PipelineContext Run(PipelineContext ctx)
    {
        var codeA = ctx.GetMetadata("code.a");
        var codeB = ctx.GetMetadata("code.b");

        // Count unique tokens (rough proxy for variable count)
        var tokensA = codeA.SelectMany(line => line.Split(' ')).Distinct().Count();
        var tokensB = codeB.SelectMany(line => line.Split(' ')).Distinct().Count();

        if (tokensA == 0 && tokensB == 0) return ctx;

        double ratio = 1.0 - (double)Math.Abs(tokensA - tokensB) / Math.Max(tokensA, tokensB);

        if (ratio > 0.9)
        {
            ctx.Reasons.Add(new SimilarityReason(
                Name,
                $"Vocabulary size ratio: {ratio:P0} ({tokensA} vs {tokensB} unique tokens)",
                ratio,
                Weight: 0.2));
        }

        return ctx;
    }
}
```

## Adding to the Pipeline

```csharp
var steps = AnalysisEngine.DefaultPipeline();
steps.Add(new VariableCountAnalyzer());
var engine = new AnalysisEngine(steps: steps);
```

Or insert at a specific position:
```csharp
steps.Insert(steps.Count - 1, new VariableCountAnalyzer());
```

## Weight Guidelines

| Weight | When to use |
|--------|-------------|
| 1.0 | Primary signal (cosine similarity) |
| 0.8–0.9 | Strong complementary signal (winnowing) |
| 0.4–0.5 | Structural/semantic signal |
| 0.2–0.3 | Weak/supporting signal (file metrics) |

Higher weight means the analyzer has more influence on the final score. A weak signal with high weight causes false positives; a strong signal with low weight gets drowned out.
