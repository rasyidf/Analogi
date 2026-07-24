---
layout: default
title: Home
nav_order: 1
permalink: /
---

# Analogi Documentation

**Analogi** is a code clone and plagiarism detection tool designed for university courses. It helps professors identify similarities across student programming assignments using state-of-the-art algorithms.

{: .fs-6 .fw-300 }

[Get Started]({% link guides/getting-started.md %}){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 .mr-2 }
[View on GitHub](https://github.com/rasyidf/Analogi){: .btn .fs-5 .mb-4 .mb-md-0 }

---

## Why Analogi?

| Feature | Description |
|---------|-------------|
| **Multi-language** | Supports C++, Python, Java, C#, JavaScript/TypeScript out of the box |
| **Extensible** | Add new languages or analyzers by implementing one interface |
| **Two scan modes** | Compare individual files or entire student submission folders |
| **Robust detection** | Survives variable renaming, reordering, and string changes |
| **Cross-platform** | Runs on Windows, Linux, and macOS |
| **Educational** | Built to teach about algorithms, not just catch cheaters |

## How It Works

Analogi uses a **pipeline architecture** where each file pair passes through:

1. **Extractors** — pull out code, comments, and structural elements
2. **Preprocessors** — normalize the code (case fold, remove string literals, rename variables)
3. **Analyzers** — compute similarity scores using different algorithms

The final score is a weighted average of all analyzer results.

## For Students

Understanding how plagiarism detection works helps you write *better*, more original code. Read the [Algorithm Documentation]({% link algorithms/index.md %}) to learn about cosine similarity, winnowing, and structural analysis.

## For Professors

See the [User Guide]({% link guides/getting-started.md %}) to set up Analogi for your course, or the [CLI Reference]({% link guides/cli.md %}) for batch processing.
