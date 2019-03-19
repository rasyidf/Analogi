/*
 * MIT
 */

using rasyidf.Analogi.Interfaces;
using System;
using System.Collections.Generic;

// ReSharper disable LoopCanBeConvertedToQuery

namespace rasyidf.Analogi
{
    /// <summary>
    /// Each input string is converted into a set of n-grams, the Jaccard index is
    /// then computed as |V1 inter V2| / |V1 union V2|.
    /// Like Q-Gram distance, the input strings are first converted into sets of
    /// n-grams (sequences of n characters, also called k-shingles), but this time
    /// the cardinality of each n-gram is not taken into account.
    /// Distance is computed as 1 - cosine similarity.
    /// Jaccard index is a metric distance.
    /// </summary>
    public class Jaccard : ShingleBased, IMetricStringDistance, INormalizedStringDistance, INormalizedStringSimilarity
    {
        /// <summary>
        /// The strings are first transformed into sets of k-shingles (sequences of k
        /// characters), then Jaccard index is computed as |A inter B| / |A union B|.
        /// The default value of k is 3.
        /// </summary>
        /// <param name="k"></param>
        public Jaccard(int k) : base(k) { }

        /// <summary>
        /// The strings are first transformed into sets of k-shingles (sequences of k
        /// characters), then Jaccard index is computed as |A inter B| / |A union B|.
        /// The default value of k is 3.
        /// </summary>
        public Jaccard() { }

        /// <summary>
        /// Compute jaccard index: |A inter B| / |A union B|.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The Jaccard index in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Similarity(string s1, string s2)
        {
            if (s1 == null)
            {
                throw new ArgumentNullException(nameof(s1));
            }

            if (s2 == null)
            {
                throw new ArgumentNullException(nameof(s2));
            }

            if (s1.Equals(s2))
            {
                return 1;
            }

            IDictionary<string, int> profile1 = GetProfile(s1);
            IDictionary<string, int> profile2 = GetProfile(s2);

            HashSet<string> union = new HashSet<string>();
            union.UnionWith(profile1.Keys);
            union.UnionWith(profile2.Keys);

            int inter = profile1.Keys.Count + profile2.Keys.Count
                        - union.Count;

            return 1.0 * inter / union.Count;
        }

        /// <summary>
        /// Distance is computed as 1 - similarity.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1 - the Jaccard similarity.</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
        {
            return 1.0 - Similarity(s1, s2);
        }
    }
}