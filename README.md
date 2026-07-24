# ANALOGI

Code clone / plagiarism detector for student programming assignments.  
Supports **C++, Python, Java, C#, and JavaScript/TypeScript** — extensible to any language.

Built with **.NET 10**, **AvaloniaUI** (Fluent theme), and **CommunityToolkit.Mvvm**.

## Features

- **Multi-language** — pluggable `ILanguageProfile` system (C++, Python, Java, C#, JS/TS built-in)
- **Extensible pipeline** — extractors, preprocessors, and analyzers are all `IPipelineStep` implementations
- **Two scan modes:**
  - **File mode** — compares individual files in a folder
  - **Submission mode** — treats each subfolder as a student submission, compares all files cross-folder
- **MOSS-style winnowing** — robust document fingerprinting that survives local edits
- **Cosine similarity** on tokenized code
- **Structure analysis** — function name overlap (Jaccard index)
- **Comment similarity** — catches forgotten identical comments
- **Import overlap** — flags shared unusual dependencies
- **Anti-rename preprocessing** — normalizes identifiers and string literals
- **File size and line count** ratio comparison
- **Async scanning** with progress reporting and cancellation
- **Cross-platform** (Windows, Linux, macOS via Avalonia)
- **CLI** for quick batch testing

## Architecture

```
Analogi.Core/           Pure library (no UI dependencies, targets net10.0)
├── Interfaces/         ILanguageProfile, IPipelineStep, IScanner
├── Models/             CodeFile, PipelineContext, ScanResult, FilePairResult
│                       Submission, SubmissionPairResult, SubmissionScanResult
├── Algorithm/          CosineSimilarity (static), Winnowing (MOSS-style)
├── Languages/          CppProfile, PythonProfile, JavaProfile, CSharpProfile,
│                       JavaScriptProfile, LanguageRegistry
├── Extractors/         Code, Comment, Structure
├── PreProcessors/      CaseFold, WhitespaceNormalize, StringLiteralNormalize,
│                       IdentifierNormalize
├── Analyzers/          CosineSimilarity, Winnowing, Structure, ImportOverlap,
│                       CommentSimilarity, FileSize, LineCount
└── Pipeline/           AnalysisEngine (orchestrator)

Analogi.App/            AvaloniaUI desktop app (Fluent theme)
├── Views/              MainWindow
└── ViewModels/         MainWindowViewModel (MVVM with CommunityToolkit)

Analogi.CLI/            Console app for batch usage
Analogi.Tests/          xUnit test suite (26 tests)
```

## Quick Start

```bash
# Build
dotnet build

# Run the desktop app
dotnet run --project Analogi.App

# Run the CLI (file mode)
dotnet run --project Analogi.CLI -- path/to/files --top 10

# Run the CLI (submission mode - compare student folders)
dotnet run --project Analogi.CLI -- path/to/submissions --submissions --top 10

# Run tests
dotnet test
```

## Scan Modes

### File Mode (default)

Compares all source files in a directory pairwise:

```
assignments/
├── student1.cpp
├── student2.cpp
└── student3.cpp
```

```bash
dotnet run --project Analogi.CLI -- assignments/
```

### Submission Mode (`--submissions`)

Treats each subdirectory as one student submission. Compares all files in submission A against all files in submission B:

```
submissions/
├── alice/
│   ├── main.cpp
│   └── util.cpp
├── bob/
│   ├── app.cpp
│   └── helper.cpp
└── charlie/
    └── solution.cpp
```

```bash
dotnet run --project Analogi.CLI -- submissions/ --submissions
```

## Analysis Pipeline

The default pipeline runs these steps in order:

| Step | Type | Description |
|------|------|-------------|
| CodeExtractor | Extractor | Strips comments, extracts clean code lines |
| CommentExtractor | Extractor | Extracts comment text |
| StructureExtractor | Extractor | Extracts function names and imports |
| CaseFold | PreProcessor | Lowercases all tokens |
| WhitespaceNormalize | PreProcessor | Collapses whitespace |
| StringLiteralNormalize | PreProcessor | Replaces string constants with `__STR__` |
| IdentifierNormalize | PreProcessor | Replaces variable names with generic tokens |
| CosineSimilarity | Analyzer | Token frequency vector comparison (weight: 1.0) |
| Winnowing | Analyzer | MOSS-style fingerprint overlap (weight: 0.9) |
| StructureSimilarity | Analyzer | Function name Jaccard index (weight: 0.5) |
| ImportOverlap | Analyzer | Import list Jaccard index (weight: 0.3) |
| CommentSimilarity | Analyzer | Cosine on comment text (weight: 0.4) |
| FileSizeRatio | Analyzer | File size ratio (weight: 0.3) |
| LineCountMatch | Analyzer | Code line count ratio (weight: 0.2) |

## Adding a New Language

Implement `ILanguageProfile`:

```csharp
public sealed partial class GoProfile : ILanguageProfile
{
    public string Name => "Go";
    public string[] FileExtensions => [".go"];
    public Regex SingleLineComment => ...;
    public Regex MultiLineComment => ...;
    public Regex FunctionDeclaration => ...;
    public Regex ImportStatement => ...;
}
```

Then register it in `LanguageRegistry`:

```csharp
registry.Register(new GoProfile());
```

## Adding a New Analyzer

Implement `IPipelineStep`:

```csharp
public sealed class MyAnalyzer : IPipelineStep
{
    public string Name => "MyAnalyzer";

    public PipelineContext Run(PipelineContext ctx)
    {
        var codeA = ctx.GetMetadata("code.a");
        var codeB = ctx.GetMetadata("code.b");

        double score = /* your comparison logic */;

        if (score > threshold)
            ctx.Reasons.Add(new SimilarityReason(Name, "description", score, Weight: 0.5));

        return ctx;
    }
}
```

Then add it to the pipeline list in `AnalysisEngine.DefaultPipeline()` or pass a custom step list to the constructor.

## How Scoring Works

Each analyzer produces a `SimilarityReason` with a score (0..1) and a weight. The final similarity index for a file pair is the **weighted average** of all triggered reasons:

```
SimilarityIndex = Σ(score × weight) / Σ(weight)
```

The plagiarism level is derived from the index:

| Level | Threshold |
|-------|-----------|
| Extreme | ≥ 90% |
| Very High | ≥ 80% |
| High | ≥ 70% |
| Moderate | ≥ 60% |
| Low | ≥ 40% |
| Minor | ≥ 20% |
| Original | < 20% |

## Requirements

- .NET 10 SDK

## License

MIT
