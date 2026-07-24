using System.Text.RegularExpressions;

namespace Analogi.Core.Algorithm;

public static partial class CosineSimilarity
{
    /// <summary>
    /// Compute cosine similarity between two strings using token frequency vectors.
    /// Returns 0..1 where 1 = identical.
    /// </summary>
    public static double Compute(string s1, string s2)
    {
        if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2)) return 0;
        if (s1 == s2) return 1;

        var profile1 = Tokenize(s1);
        var profile2 = Tokenize(s2);

        double dot = DotProduct(profile1, profile2);
        double norm = Norm(profile1) * Norm(profile2);
        return norm == 0 ? 0 : dot / norm;
    }

    private static Dictionary<string, int> Tokenize(string text)
    {
        var tokens = new Dictionary<string, int>();
        foreach (var match in TokenRegex().EnumerateMatches(text.AsSpan()))
        {
            var token = text.Substring(match.Index, match.Length);
            if (tokens.TryGetValue(token, out int count))
                tokens[token] = count + 1;
            else
                tokens[token] = 1;
        }
        return tokens;
    }

    private static double DotProduct(Dictionary<string, int> a, Dictionary<string, int> b)
    {
        // Iterate over the smaller set
        if (a.Count > b.Count) (a, b) = (b, a);

        double sum = 0;
        foreach (var (key, val) in a)
        {
            if (b.TryGetValue(key, out int bVal))
                sum += (double)val * bVal;
        }
        return sum;
    }

    private static double Norm(Dictionary<string, int> profile)
    {
        double sum = 0;
        foreach (var (_, val) in profile)
            sum += (double)val * val;
        return Math.Sqrt(sum);
    }

    [GeneratedRegex(@"\w+", RegexOptions.Compiled)]
    private static partial Regex TokenRegex();
}
