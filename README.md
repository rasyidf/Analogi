# ANALOGI

Code clone / plagiarism detector for student programming assignments.  
Supports **C++, Python, Java, and C#** out of the box — and is extensible to any language.

Built with **.NET 10**, **AvaloniaUI** (Fluent theme), and **CommunityToolkit.Mvvm**.

## Features

- Multi-language support via pluggable `ILanguageProfile` — add a new language by implementing one interface
- Extensible analysis pipeline — extractors, preprocessors, and analyzers are all pluggable `IPipelineStep` implementations
- Cosine similarity on tokenized code
- Structural analysis (function name overlap via Jaccard index)
- File size and line count ratio comparison
- Async scanning with progress reporting
- Filter results by plagiarism severity level
- Cross-platform (Windows, Linux, macOS via Avalonia)

## Architecture

```
Analogi.Core/        Pure library (no UI dependencies)
├── Interfaces/      ILanguageProfile, IPipelineStep, IScanner
├── Models/          CodeFile, PipelineContext, ScanResult, FilePairResult
├── Algorithm/       Static CosineSimilarity
├── Languages/       CppProfile, PythonProfile, JavaProfile, CSharpProfile, LanguageRegistry
├── Extractors/      Code, Comment, Structure
├── PreProcessors/   CaseFold, WhitespaceNormalize
├── Analyzers/       CosineSimilarity, Structure, FileSize, LineCount
└── Pipeline/        AnalysisEngine (orchestrator)

Analogi.App/         AvaloniaUI desktop app
├── Views/           MainWindow (AXAML + code-behind for platform APIs)
└── ViewModels/      MainWindowViewModel (MVVM with CommunityToolkit)
```

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

Then register it: `registry.Register(new GoProfile());`

## Adding a New Analyzer

Implement `IPipelineStep`:

```csharp
public sealed class MyAnalyzer : IPipelineStep
{
    public string Name => "MyAnalyzer";
    public PipelineContext Run(PipelineContext ctx)
    {
        // Read from ctx.GetMetadata(...), compute, add to ctx.Reasons
        return ctx;
    }
}
```

Then add it to the pipeline list.

## Build & Run

```bash
dotnet build
dotnet run --project Analogi.App
```

## Requirements

- .NET 10 SDK
