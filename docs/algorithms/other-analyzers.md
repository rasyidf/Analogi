---
layout: default
title: Other Analyzers
parent: Algorithms
nav_order: 3
---

# Other Analyzers

Beyond cosine similarity and winnowing, Analogi uses several supplementary analyzers that catch different plagiarism patterns.

---

## Structure Similarity (Jaccard on Function Names)

{: .label .label-green }
Weight: 0.5

Extracts function/method names from both files using language-specific regex, then computes **Jaccard similarity**:

```
J(A, B) = |A ∩ B| / |A ∪ B|
```

**What it catches:** Students who copy the overall program structure (same function names, same decomposition) even if the implementation details differ.

**Threshold:** 0.5 (50% of function names shared)

---

## Import/Include Overlap

{: .label .label-yellow }
Weight: 0.3

Jaccard similarity on the list of imported modules/headers.

**What it catches:** Unusual shared dependencies. If two students both import `<algorithm>`, `<unordered_map>`, and `<functional>` — that's a signal. Common imports like `<iostream>` are less suspicious individually, but the *combination* matters.

**Threshold:** 0.7 (70% import overlap)

---

## Comment Similarity

{: .label .label-red }
Weight: 0.4

Cosine similarity computed on comment text only (extracted separately from code).

**What it catches:** Students who copy code and forget to change the comments. Even "personalized" code often has identical comment blocks.

**Threshold:** 0.7 (70% comment similarity)

---

## File Size Ratio

{: .label }
Weight: 0.3

```
ratio = 1 - |sizeA - sizeB| / max(sizeA, sizeB)
```

**What it catches:** A weak signal on its own, but combined with other analyzers, near-identical file sizes (>95% ratio) add confidence that files share a common origin.

**Threshold:** 0.95 (file sizes within 5% of each other)

---

## Line Count Match

{: .label }
Weight: 0.2

Same formula as file size ratio, but applied to the number of code lines (after comment stripping).

**Threshold:** 0.95 (line counts within 5%)

---

## Why Multiple Analyzers?

No single metric is perfect:

| Attack | Cosine | Winnowing | Structure | Comments | Size |
|--------|:------:|:---------:|:---------:|:--------:|:----:|
| Copy-paste (no changes) | ✓ | ✓ | ✓ | ✓ | ✓ |
| Rename variables | ✗* | ✗* | ✓ | ✓ | ✓ |
| Reorder functions | ✓ | ~✓ | ✓ | ✓ | ✓ |
| Add whitespace/comments | ✓ | ✓ | ✓ | ~✗ | ~✗ |
| Rewrite logic, keep structure | ✗ | ✗ | ✓ | ✗ | ~✓ |
| Complete rewrite | ✗ | ✗ | ✗ | ✗ | ✗ |

*\* = After IdentifierNormalize preprocessing, these become ✓*

By combining all signals with weights, Analogi produces a score that's hard to game without genuinely rewriting the code.
