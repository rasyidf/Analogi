---
layout: default
title: Preprocessing
parent: Algorithms
nav_order: 4
---

# Preprocessing Pipeline

Before analysis, code passes through several normalization steps that strip away superficial differences. This is key to catching "disguised" plagiarism.

## Pipeline Order

```
Raw Code → CaseFold → WhitespaceNormalize → StringLiteralNormalize → IdentifierNormalize → Analyzers
```

Each step makes the code more "canonical", so that cosmetic differences don't fool the analyzers.

---

## Case Fold

Converts all text to lowercase.

```
Before: int MyVariable = GetValue();
After:  int myvariable = getvalue();
```

**Why:** Changing capitalization is the easiest "disguise" and should never fool a detector.

---

## Whitespace Normalize

Collapses all whitespace (tabs, multiple spaces, blank lines) into single spaces, and trims lines.

```
Before:     int    x   =   5  ;
After:  int x = 5 ;
```

**Why:** Adding/removing whitespace is trivial and meaningless.

---

## String Literal Normalize

Replaces all string constants with a placeholder `__STR__`.

```
Before: cout << "Hello, World!" << endl;
After:  cout << __STR__ << endl;

Before: print("Goodbye, Earth!")
After:  print(__STR__)
```

**Why:** Students often change only string messages to "personalize" copied code. After this step, both versions are identical.

---

## Identifier Normalize

Replaces user-defined variable and function names with sequential generic tokens (`v0`, `v1`, `v2`, ...), while preserving language keywords.

```
Before: int counter = 0; counter = counter + 1; return counter;
After:  int v0 = 0; v0 = v0 + 1; return v0;

Before: int total = 0; total = total + 1; return total;
After:  int v0 = 0; v0 = v0 + 1; return v0;
```

Both are now **identical** after normalization.

**How it works:**
1. Scan tokens left-to-right
2. If a token is a language keyword (`int`, `return`, `if`, `for`, etc.) → keep it
3. Otherwise → assign it the next generic name (`v0`, `v1`, ...)
4. Subsequent occurrences of the same original name get the same generic name

**Why:** Variable renaming (alpha-renaming) is the most common plagiarism technique. This step completely defeats it.

{: .warning }
> Identifier normalization is aggressive. It can produce false positives for very short programs where different algorithms happen to use the same number of variables. That's why Analogi uses multiple analyzers and requires convergence from several signals.

---

## What Preprocessing Does NOT Do

- It does not reorder code (that's what winnowing handles)
- It does not change control flow (no AST transformation)
- It does not add or remove lines

This means that genuinely **restructured** code (different algorithm, different decomposition) will still look different after preprocessing. The system only flags code that's structurally the same but cosmetically different.
