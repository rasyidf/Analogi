/*
 * MIT
 */

using rasyidf.Analogi.Interfaces;
using System;
using System.Collections.Generic;

namespace rasyidf.Analogi
{
    /// Q-gram distance, as defined by Ukkonen in "Approximate string-matching with
    /// q-grams and maximal matches". The distance between two strings is defined as
    /// the L1 norm of the difference of their profiles (the number of occurences of
    /// each n-gram): SUM( |V1_i - V2_i| ). Q-gram distance is a lower bound on
    /// Levenshtein distance, but can be computed in O(m + n), where Levenshtein
    /// requires O(m.n).
    public class QGram : ShingleBased, IStringDistance
    {
        /// <summary>
        /// Q-gram similarity and distance. Defined by Ukkonen in "Approximate
        /// string-matching with q-grams and maximal matches",
        /// http://www.sciencedirect.com/science/article/pii/0304397592901434 The
        /// distance between two strings is defined as the L1 norm of the difference
        /// of their profiles (the number of occurences of each k-shingle). Q-gram
        /// distance is a lower bound on Levenshtein distance, but can be computed in
        /// O(|A| + |B|), where Levenshtein requires O(|A|.|B|)
        /// </summary>
        /// <param name="k"></param>
        public QGram(int k) : base(k) { }

        /// <summary>
        /// Q-gram similarity and distance. Defined by Ukkonen in "Approximate
        /// string-matching with q-grams and maximal matches",
        /// http://www.sciencedirect.com/science/article/pii/0304397592901434 The
        /// distance between two strings is defined as the L1 norm of the difference
        /// of their profiles (the number of occurence of each k-shingle). Q-gram
        /// distance is a lower bound on Levenshtein distance, but can be computed in
        /// O(|A| + |B|), where Levenshtein requires O(|A|.|B|)
        /// Default k is 3.
        /// </summary>
        public QGram() { }

        /// <summary>
        /// The distance between two strings is defined as the L1 norm of the
        /// difference of their profiles (the number of occurence of each k-shingle).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The computed Q-gram distance.</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
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
                return 0;
            }

            IDictionary<string, int> profile1 = GetProfile(s1);
            IDictionary<string, int> profile2 = GetProfile(s2);

            return Distance(profile1, profile2);
        }

        /// <summary>
        /// Compute QGram distance using precomputed profiles.
        /// </summary>
        /// <param name="profile1"></param>
        /// <param name="profile2"></param>
        /// <returns></returns>
        public double Distance(IDictionary<string, int> profile1, IDictionary<string, int> profile2)
        {
            HashSet<string> union = new HashSet<string>();
            union.UnionWith(profile1.Keys);
            union.UnionWith(profile2.Keys);

            int agg = 0;
            foreach (string key in union)
            {
                int v1 = 0;
                int v2 = 0;

                if (profile1.TryGetValue(key, out int iv1))
                {
                    v1 = iv1;
                }

                if (profile2.TryGetValue(key, out int iv2))
                {
                    v2 = iv2;
                }

                agg += Math.Abs(v1 - v2);
            }

            return agg;
        }
    }
}