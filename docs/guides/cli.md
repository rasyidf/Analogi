---
layout: default
title: CLI Reference
parent: User Guide
nav_order: 2
---

# CLI Reference

The `Analogi.CLI` project provides a command-line interface for batch processing.

## Usage

```bash
dotnet run --project Analogi.CLI -- <folder-path> [options]
```

## Options

| Flag | Short | Description |
|------|-------|-------------|
| `--submissions` | `-s` | Treat subfolders as student submissions |
| `--top N` | `-n N` | Show top N results (default: 10) |

## Examples

### File Mode

Compare all source files in a flat directory:

```bash
dotnet run --project Analogi.CLI -- ./assignments/ --top 5
```

Output:
```
Scanning files: ./assignments/

  Done. 10 files, 45 pairs in 60ms.

Top 5 results:
----------------------------------------------------------------------
  student1.cpp <-> student2.cpp: 95% (Extreme)
    [CosineSimilarity] Code cosine similarity: 98%
    [Winnowing] Winnowing fingerprint overlap: 92%
    [StructureSimilarity] Function name overlap (Jaccard): 100%
```

### Submission Mode

Compare student submission folders (each subfolder = one student):

```bash
dotnet run --project Analogi.CLI -- ./submissions/ --submissions --top 5
```

```
submissions/
├── alice/
│   ├── main.cpp
│   └── util.cpp
├── bob/
│   ├── solution.cpp
│   └── helper.cpp
└── charlie/
    └── program.cpp
```

Output includes which specific files matched across submissions:
```
  alice <-> bob: 87% (VeryHigh)
    Files: 2 vs 2 | Matching file pairs: 3
    [CosineSimilarity] Code cosine similarity: 89%
    [Winnowing] Winnowing fingerprint overlap: 78%
```

## Exit Codes

| Code | Meaning |
|------|---------|
| 0 | Success |
| 1 | Error (invalid path, no arguments) |

## Integration with CI

You can use the CLI in automated grading pipelines:

```bash
# Run and fail if any pair exceeds 80% similarity
dotnet run --project Analogi.CLI -- ./submissions/ -s -n 100 | grep -q "Extreme\|VeryHigh" && echo "ALERT: High similarity detected"
```
