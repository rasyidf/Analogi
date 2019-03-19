/*
 * MIT
 */

using rasyidf.Analogi.Interfaces;
using System;
using System.Collections.Generic;

// ReSharper disable LoopCanBeConvertedToQuery

namespace rasyidf.Analogi
{
    public class Cosine : TFIDFBased, INormalizedStringSimilarity, INormalizedStringDistance
    {
        #region Constructors

        /// <summary>
        /// Implements Cosine Similarity between strings.The strings are first
        /// transformed in vectors of occurrences of k-shingles(sequences of k
        /// characters). In this n-dimensional space, the similarity between the two
        /// strings is the cosine of their respective vectors.
        /// </summary>
        /// <param name="k"></param>
        public Cosine(int k) : base(k) { }

        /// <summary>
        /// Implements Cosine Similarity between strings.The strings are first
        /// transformed in vectors of occurrences of k-shingles(sequences of k
        /// characters). In this n-dimensional space, the similarity between the two
        /// strings is the cosine of their respective vectors.
        ///
        /// Default k is 3.
        /// </summary>
        public Cosine() { }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Returns 1.0 - similarity.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>1.0 - the cosine similarity in the range [0, 1]</returns>
        /// <exception cref="ArgumentNullException">If s1 or s2 is null.</exception>
        public double Distance(string s1, string s2)
        {
            return 1.0 - Similarity(s1, s2);
        }

        /// <summary>
        /// Compute the cosine similarity between strings.
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>The cosine similarity in the range [0, 1]</returns>
        /// <exception cref="T:System.ArgumentNullException">If s1 or s2 is null.</exception>
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

            if (s1.Length < k || s2.Length < k)
            {
                return 0;
            }

            IDictionary<string, int> profile1 = GetProfile(s1);
            IDictionary<string, int> profile2 = GetProfile(s2);

            return DotProduct(profile1, profile2) / (Norm(profile1) * Norm(profile2));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="profile1"></param> 
        /// <param name="profile2"></param>
        /// <returns></returns>
        public double Similarity(IDictionary<string, int> profile1, IDictionary<string, int> profile2)
        {
            return DotProduct(profile1, profile2)
                       / (Norm(profile1) * Norm(profile2));
        }

        private static double DotProduct(IDictionary<string, int> profile1,
                    IDictionary<string, int> profile2)
        {
            // Loop over the smallest map
            IDictionary<string, int> small_profile = profile2;
            IDictionary<string, int> large_profile = profile1;

            if (profile1.Count < profile2.Count)
            {
                small_profile = profile1;
                large_profile = profile2;
            }

            double agg = 0;
            foreach (KeyValuePair<string, int> entry in small_profile)
            {
                if (!large_profile.TryGetValue(entry.Key, out int i))
                {
                    continue;
                }

                agg += 1.0 * entry.Value * i;
            }

            return agg;
        }

        /// <summary>
        /// Compute the norm L2 : sqrt(Sum_i( v_i²)).
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private static double Norm(IDictionary<string, int> profile)
        {
            double agg = 0;

            foreach (KeyValuePair<string, int> entry in profile)
            {
                agg += 1.0 * entry.Value * entry.Value;
            }

            return Math.Sqrt(agg);
        }

        #endregion Methods
    }
}