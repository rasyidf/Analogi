---
layout: default
title: Algorithms
nav_order: 3
has_children: true
---

# Algorithm Documentation

This section explains the detection algorithms used by Analogi. Understanding these helps you appreciate *why* certain code modifications are (or aren't) detected, and teaches fundamental concepts in information retrieval and text analysis.

## Pipeline Overview

Every file pair passes through this pipeline:

```
┌─────────────┐    ┌─────────────────┐    ┌───────────┐
│  Extractors │ →  │  Preprocessors  │ →  │ Analyzers │
└─────────────┘    └─────────────────┘    └───────────┘
      │                    │                     │
 Code, Comments,     Normalize text:        Score each
 Function names,     case fold, strip       dimension:
 Imports             strings, rename vars   cosine, winnow...
```

## Scoring Formula

Each analyzer produces a score (0–1) and a weight. The final similarity index is:

```
SimilarityIndex = Σ(scoreᵢ × weightᵢ) / Σ(weightᵢ)
```

Only analyzers that trigger (score > threshold) contribute to the final average. This means files with many matching signals score higher than files that match on only one dimension.
