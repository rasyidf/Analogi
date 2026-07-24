---
layout: default
title: Winnowing (MOSS)
parent: Algorithms
nav_order: 2
---

# Winnowing — Document Fingerprinting

{: .label .label-blue }
Weight: 0.9

## What It Does

Winnowing creates a set of **fingerprints** from a document — a compressed representation that survives local edits, insertion, and deletion. It's the core algorithm behind [MOSS](https://theory.stanford.edu/~aiken/moss/) (Measure of Software Similarity), used by thousands of universities worldwide.

## How It Works

### Step 1: Create k-grams

Slide a window of size **k** over the token sequence:

```
Tokens: [int, main, return, 0, end, print, hello, world]
3-grams: [int main return], [main return 0], [return 0 end], ...
```

### Step 2: Hash each k-gram

Compute a hash for each k-gram using a polynomial hash function (Analogi uses FNV-1a):

```
h₁ = hash("int main return") = 7234801...
h₂ = hash("main return 0")   = 1829412...
h₃ = hash("return 0 end")    = 9012341...
...
```

### Step 3: Winnow — select fingerprints

Slide a window of size **w** over the hash sequence. In each window, select the **minimum hash value** (rightmost on tie):

```
Window size = 4
Hashes: [72, 18, 90, 45, 23, 67, 11, 89, ...]

Window [72, 18, 90, 45] → select 18 ✓
Window [18, 90, 45, 23] → select 18 (same position, skip)
Window [90, 45, 23, 67] → select 23 ✓
Window [45, 23, 67, 11] → select 11 ✓
```

The selected hashes are the document's **fingerprints**.

### Step 4: Compare fingerprint sets

Compare two documents using **Jaccard similarity** on their fingerprint sets:

```
J(A, B) = |A ∩ B| / |A ∪ B|
```

## Why Winnowing?

| Property | Explanation |
|----------|-------------|
| **Noise threshold** | Matches shorter than k tokens are not detected (filters language boilerplate) |
| **Guaranteed detection** | Any match of length ≥ k + w - 1 tokens is *guaranteed* to be detected |
| **Local edit resistance** | Inserting or deleting a few tokens only affects nearby fingerprints |
| **Position independence** | Reordering code blocks is still detected (shared fingerprints persist) |

## Parameters in Analogi

| Parameter | Value | Effect |
|-----------|-------|--------|
| k (kgram size) | 5 | Matches < 5 tokens are noise |
| w (window size) | 4 | Guarantees detection of matches ≥ 8 tokens |
| Threshold | 0.3 | Reports pairs with > 30% fingerprint overlap |

## Example: Why Renaming Variables Doesn't Help

After preprocessing normalizes identifiers:
```
Original:  int fibonacci(int n) { ... }
Renamed:   int calculate(int x) { ... }
Normalized: int v0(int v1) { ... }  ← same for both!
```

Both produce identical k-grams → identical fingerprints → similarity = 1.0.

## Example: Why Reordering Partially Helps

If a student moves a function from the top to the bottom of the file:
- The k-grams *within* the function are unchanged → those fingerprints still match
- Only k-grams that *span* the boundary are different
- Result: still high similarity, but not 100%

## Limitations

- Very short files produce few fingerprints (unreliable)
- Adding many new lines of code (padding) dilutes the overlap percentage
- That's why Analogi pairs winnowing with cosine similarity (which handles the "vocabulary" dimension)

## Further Reading

- Schleimer, S., Wilkerson, D.S., & Aiken, A. (2003). "Winnowing: Local Algorithms for Document Fingerprinting." ACM SIGMOD.
- [MOSS homepage](https://theory.stanford.edu/~aiken/moss/)
