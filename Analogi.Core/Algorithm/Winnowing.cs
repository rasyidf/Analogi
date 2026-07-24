namespace Analogi.Core.Algorithm;

/// <summary>
/// Winnowing algorithm for document fingerprinting (as used by MOSS).
/// Produces a set of selected fingerprints from a document by:
/// 1. Computing rolling hash values for all k-grams
/// 2. Selecting minimum hash in each window of size w
/// This gives a robust subset of fingerprints that survives local edits.
/// 
/// Reference: Schleimer, Wilkerson, Aiken - "Winnowing: Local Algorithms for Document Fingerprinting" (2003)
/// </summary>
public static class Winnowing
{
    /// <summary>
    /// Compute winnowed fingerprints from token sequence.
    /// </summary>
    /// <param name="tokens">List of normalized tokens (e.g., code lines or words).</param>
    /// <param name="kgramSize">Size of k-grams (default 5). Matches shorter than this are noise.</param>
    /// <param name="windowSize">Window size for winnowing (default 4). Guarantees detection of matches >= k+w-1.</param>
    /// <returns>Set of selected hash fingerprints.</returns>
    public static HashSet<long> Fingerprint(IReadOnlyList<string> tokens, int kgramSize = 5, int windowSize = 4)
    {
        if (tokens.Count < kgramSize) return [];

        // Step 1: Compute hashes for all k-grams
        var hashes = ComputeKgramHashes(tokens, kgramSize);
        if (hashes.Length < windowSize) return new HashSet<long>(hashes);

        // Step 2: Winnow — select minimum hash in each window
        var fingerprints = new HashSet<long>();
        int prevMin = -1;

        for (int i = 0; i <= hashes.Length - windowSize; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < i + windowSize; j++)
            {
                if (hashes[j] <= hashes[minIdx]) // rightmost minimum on tie
                    minIdx = j;
            }

            // Only add if this is a new minimum position (avoid duplicates)
            if (minIdx != prevMin)
            {
                fingerprints.Add(hashes[minIdx]);
                prevMin = minIdx;
            }
        }

        return fingerprints;
    }

    /// <summary>
    /// Compute Jaccard similarity between two fingerprint sets.
    /// </summary>
    public static double Similarity(HashSet<long> fpA, HashSet<long> fpB)
    {
        if (fpA.Count == 0 && fpB.Count == 0) return 0;

        int intersection = fpA.Count < fpB.Count
            ? fpA.Count(h => fpB.Contains(h))
            : fpB.Count(h => fpA.Contains(h));

        int union = fpA.Count + fpB.Count - intersection;
        return union == 0 ? 0 : (double)intersection / union;
    }

    private static long[] ComputeKgramHashes(IReadOnlyList<string> tokens, int k)
    {
        int count = tokens.Count - k + 1;
        var hashes = new long[count];

        for (int i = 0; i < count; i++)
        {
            hashes[i] = KgramHash(tokens, i, k);
        }

        return hashes;
    }

    /// <summary>
    /// Hash a k-gram using a rolling polynomial hash.
    /// </summary>
    private static long KgramHash(IReadOnlyList<string> tokens, int start, int k)
    {
        // FNV-1a inspired hash over concatenated tokens
        const long FnvPrime = 1099511628211L;
        const long FnvOffset = unchecked((long)14695981039346656037UL);

        long hash = FnvOffset;
        for (int i = start; i < start + k; i++)
        {
            foreach (char c in tokens[i])
            {
                hash ^= c;
                hash *= FnvPrime;
            }
            hash ^= '\n'; // separator between tokens
            hash *= FnvPrime;
        }
        return hash;
    }
}
