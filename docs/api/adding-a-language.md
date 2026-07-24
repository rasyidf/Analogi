---
layout: default
title: Adding a Language
parent: API Reference
nav_order: 1
---

# Adding a New Language

Implement the `ILanguageProfile` interface to add support for any programming language.

## Interface

```csharp
public interface ILanguageProfile
{
    string Name { get; }
    string[] FileExtensions { get; }
    Regex SingleLineComment { get; }
    Regex MultiLineComment { get; }
    Regex FunctionDeclaration { get; }
    Regex ImportStatement { get; }
}
```

## Example: Adding Go Support

```csharp
public sealed partial class GoProfile : ILanguageProfile
{
    public string Name => "Go";
    public string[] FileExtensions => [".go"];

    public Regex SingleLineComment => SingleLine();
    public Regex MultiLineComment => MultiLine();
    public Regex FunctionDeclaration => FuncDecl();
    public Regex ImportStatement => Import();

    [GeneratedRegex(@"//[^\n]*", RegexOptions.Compiled)]
    private static partial Regex SingleLine();

    [GeneratedRegex(@"/\*[\s\S]*?\*/", RegexOptions.Compiled)]
    private static partial Regex MultiLine();

    [GeneratedRegex(@"func\s+(?<name>\w+)\s*\(", RegexOptions.Compiled)]
    private static partial Regex FuncDecl();

    [GeneratedRegex(@"import\s+(?:""(?<file>[^""]+)""|[\s\S]*?""(?<file>[^""]+)"")", RegexOptions.Compiled)]
    private static partial Regex Import();
}
```

## Registering

```csharp
var registry = new LanguageRegistry();
registry.Register(new GoProfile());
var engine = new AnalysisEngine(registry);
```

## What Each Regex Does

| Property | Used By | Purpose |
|----------|---------|---------|
| `SingleLineComment` | CodeExtractor, CommentExtractor | Strip/extract single-line comments |
| `MultiLineComment` | CodeExtractor, CommentExtractor | Strip/extract block comments |
| `FunctionDeclaration` | StructureExtractor | Extract function names for structural comparison |
| `ImportStatement` | StructureExtractor | Extract imports for overlap analysis |

## Tips

- The `FunctionDeclaration` regex should have a named capture group `name` that captures the function identifier
- The `ImportStatement` regex should have a named capture group `file` that captures the module/file path
- Test your regex against real student code before deploying
