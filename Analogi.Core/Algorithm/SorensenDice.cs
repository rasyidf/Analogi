/*
 * MIT
 */

using rasyidf.Analogi.Interfaces;
using System;
using System.Collections.Generic;

// ReSharper disable LoopCanBeConvertedToQuery

namespace rasyidf.Analogi
{
    /// Similar to Jaccard index, but this time the similarity is computed as 2 * |V1
    /// inter V2| / (|V1| + |V2|). Distance is computed as 1 - cosine similarity.
    public class SorensenDice : ShingleBased, INormalizedStringDistance, INormalizedStringSimilarity
    {
        /// <summary>
        /// Sorensen-Dice coefficient, aka Sørensen index, Dice's coefficient or
        /// Czekanowski's binary (non-quantitative) index.
        ///
        /// The strings are first converted to boolean sets of k-shingles (sequences
        /// of k characters), then the similarity is computed as 2 * |A inter B| /
        /// (|A| + |B|). Attention: Sorensen-Dice distance (and similarity) does not
        /// satisfy triangle inequality.
        /// </summary>
        /// <param name="k"></param>
        public SorensenDice(int k) : base(k) { }

        /// <summary>
        /// Sorensen-Dice coefficient, aka Sørensen index, Dice's coefficient or
        /// Czekanowski's binary (non-quantitative) index.
        ///
        /// The strings are first converted to boolean sets of k-shingles (sequences
        /// of k characters), then the similarity is computed as 2 * |A inter B| /
        /// (|A| + |B|). Attention: Sorensen-Dice distance (and similarity) does not
        /// satisfy triangle inequality.
        /// Default k is 3.
        /// </summary>
        public SorensenDice() { }

        /// <summary>
        /// Similarity is computed as 2 * |A inter B| / (|A| + |B|).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The computed Sorensen-Dice similarity.</returns>
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

            int inter = 0;

            foreach (string key in union)
            {
                if (profile1.ContainsKey(key) && profile2.ContainsKey(key))
                {
                    inter++;
                }
            }

            return 2.0 * inter / (profile1.Count + profile2.Count);
        }

        /// <summary>
        /// Returns 1 - similarity.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1.0 - the computed similarity</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
        {
            return 1 - Similarity(s1, s2);
        }
    }
}