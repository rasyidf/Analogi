/*
 * MIT
 */

namespace Analogi.Core.Interfaces
{
    public interface IStringSimilarity
    {
        /// <summary>
        /// Compute and return a measure of similarity between 2 strings.
        /// </summary>
        /// <param name="s1">The first string</param>
        /// <param name="s2">The second string</param>
        /// <returns>Similarity (0 means both strings are completely different)</returns>
        double Similarity(string s1, string s2);
    }
}