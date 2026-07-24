---
layout: default
title: Cosine Similarity
parent: Algorithms
nav_order: 1
---

# Cosine Similarity

{: .label .label-purple }
Weight: 1.0

## What It Does

Compares two code files by treating them as **bags of tokens** (words) and measuring the angle between their frequency vectors. Identical documents have cosine = 1; completely different documents have cosine = 0.

## The Math

Given two documents represented as term-frequency vectors **A** and **B**:

```
similarity(A, B) = (A · B) / (‖A‖ × ‖B‖)

where:
  A · B    = Σ(aᵢ × bᵢ)       (dot product)
  ‖A‖     = √(Σ(aᵢ²))         (L2 norm)
```

## Example

Given two code snippets:

```
File A: "int x = 5; int y = 10; return x + y;"
File B: "int a = 5; int b = 10; return a + b;"
```

After tokenization:
- A: {int: 2, x: 2, 5: 1, y: 2, 10: 1, return: 1}
- B: {int: 2, a: 2, 5: 1, b: 2, 10: 1, return: 1}

Shared tokens: `int`, `5`, `10`, `return` → significant overlap.

After **identifier normalization** (a preprocessing step), both become:
- {int: 2, v0: 2, 5: 1, v1: 2, 10: 1, return: 1}

Now cosine similarity = **1.0** (identical).

## Why It Works for Plagiarism Detection

- Captures the overall "vocabulary" of a program
- Insensitive to token ordering (reordering lines doesn't help)
- Combined with preprocessing (case fold, identifier normalize), it catches renamed-variable plagiarism

## Limitations

- Doesn't capture **structure** — two programs with the same tokens in completely different order score identically
- Short programs may score high by coincidence (few unique tokens)
- That's why Analogi uses **multiple** analyzers, not just cosine

## Threshold

Only triggers when similarity > **0.6** (60%). Below this, most code is coincidentally similar due to language keywords.

## Further Reading

- Singhal, A. (2001). "Modern Information Retrieval: A Brief Overview." IEEE Data Engineering Bulletin.
- Manning, C.D., Raghavan, P., & Schütze, H. (2008). *Introduction to Information Retrieval*, Chapter 6.
