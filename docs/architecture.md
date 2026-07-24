---
layout: default
title: Architecture
nav_order: 5
---

# Architecture

## Project Structure

```
Analogi/
├── Analogi.Core/           Pure .NET 10 library (no UI dependencies)
│   ├── Interfaces/         ILanguageProfile, IPipelineStep, IScanner
│   ├── Models/             CodeFile, PipelineContext, ScanResult, Submission
│   ├── Algorithm/          CosineSimilarity (static), Winnowing (MOSS-style)
│   ├── Languages/          Cpp, Python, Java, CSharp, JavaScript profiles
│   ├── Extractors/         Code, Comment, Structure
│   ├── PreProcessors/      CaseFold, WhitespaceNormalize, StringLiteral, Identifier
│   ├── Analyzers/          Cosine, Winnowing, Structure, Import, Comment, Metrics
│   └── Pipeline/           AnalysisEngine (orchestrator)
│
├── Analogi.App/            AvaloniaUI desktop app (FluentAvalonia theme)
│   ├── Views/              MainWindow, Pages (Scan, Results, Compare, About)
│   └── ViewModels/         MVVM with CommunityToolkit.Mvvm
│
├── Analogi.CLI/            Console app for batch processing
├── Analogi.Tests/          xUnit test suite
└── docs/                   This documentation (Jekyll + just-the-docs)
```

## Design Principles

1. **Core is pure** — `Analogi.Core` has zero UI or platform dependencies. It targets `net10.0` and can be used by any .NET application (CLI, web API, desktop app).

2. **Pipeline is pluggable** — Every step (extractor, preprocessor, analyzer) implements `IPipelineStep`. You can add, remove, or reorder steps without changing the engine.

3. **Languages are data** — Adding a language means providing regex patterns, not modifying the analysis logic.

4. **Comparison is O(n²/2)** — We only compare pairs (i, j) where i < j, halving the work since similarity is symmetric.

5. **Async-first** — All scanning is async with `IProgress<T>` reporting and `CancellationToken` support.

## Data Flow

```
Folder Path
    │
    ▼
┌─────────────────┐
│  AnalysisEngine  │
│                  │
│  1. ScanFiles()  │─── Discovers files, groups by language
│  2. For each     │
│     pair (i,j):  │
│                  │
│  ┌────────────┐  │
│  │ Extractors │  │─── CodeExtractor, CommentExtractor, StructureExtractor
│  └─────┬──────┘  │
│        ▼         │
│  ┌────────────┐  │
│  │ PreProcess │  │─── CaseFold → Whitespace → StringLiteral → Identifier
│  └─────┬──────┘  │
│        ▼         │
│  ┌────────────┐  │
│  │ Analyzers  │  │─── Each adds SimilarityReason to context if score > threshold
│  └─────┬──────┘  │
│        ▼         │
│  FilePairResult   │─── Weighted average of all reasons
│                  │
└────────┬─────────┘
         ▼
    ScanResult
    (sorted pairs)
```

## Technology Stack

| Component | Technology |
|-----------|-----------|
| Core library | .NET 10, C# 14 |
| Desktop UI | AvaloniaUI 12, FluentAvalonia 2.2 |
| Code viewer | AvaloniaEdit 12 |
| MVVM | CommunityToolkit.Mvvm 8.2 |
| Testing | xUnit |
| Documentation | Jekyll + just-the-docs |
