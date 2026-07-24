---
layout: default
title: Getting Started
parent: User Guide
nav_order: 1
---

# Getting Started

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later

## Installation

```bash
git clone https://github.com/rasyidf/Analogi.git
cd Analogi
dotnet build
```

## Running the Desktop App

```bash
dotnet run --project Analogi.App
```

The app opens with a **navigation sidebar**:
- **Scan** — select a folder and run analysis
- **Results** — view similarity scores, filter by severity
- **Compare** — side-by-side code view with highlighted differences
- **About** — version and technology info

## Quick Scan (Desktop)

1. Click **Browse** and select the folder containing student files
2. Toggle **Submission mode** if each student has their own subfolder
3. Click **Start Scan**
4. Review results sorted by similarity percentage
5. Click any pair to see the side-by-side comparison

## Quick Scan (CLI)

```bash
# File mode — compare individual files
dotnet run --project Analogi.CLI -- path/to/files --top 10

# Submission mode — compare student folders
dotnet run --project Analogi.CLI -- path/to/submissions --submissions --top 10
```

## Understanding Results

Each pair receives a **Similarity Index** (0–100%) based on a weighted average of multiple analyzer scores:

| Level | Threshold | Interpretation |
|-------|-----------|----------------|
| Extreme | ≥ 90% | Almost certainly copied |
| Very High | ≥ 80% | Very likely copied with minor changes |
| High | ≥ 70% | Significant structural similarity |
| Moderate | ≥ 60% | Suspicious, worth investigating |
| Low | ≥ 40% | Some shared patterns |
| Minor | ≥ 20% | Minimal similarity (likely coincidence) |
| Original | < 20% | Independent work |

{: .note }
> A high similarity score doesn't automatically mean plagiarism. Common patterns (boilerplate, standard algorithms) can produce false positives. Always review the **specific reasons** listed for each pair.
