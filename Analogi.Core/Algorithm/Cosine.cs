/*
 * MIT
 */

using Analogi.Core.Interfaces;
using System;
using System.Collections.Generic;

// ReSharper disable LoopCanBeConvertedToQuery

namespace Analogi.Core.Algorithm
{
    public class Cosine : TFBased, IStringSimilarity, IStringDistance
    {
        #region Constructors

        public Cosine()
        {
        }

        #endregion Constructors

        #region Methods

        public double Distance(string s1, string s2)
        {
            return 1.0 - Similarity(s1, s2);
        }

        public double Similarity(string s1, string s2)
        {
            if (s1 == null || s2 == null)
            {
                throw new ArgumentNullException(nameof(s2));
            }

            if (s1.Equals(s2))
            {
                return 1;
            }

            IDictionary<string, int> profile1 = Tokenize(s1);
            IDictionary<string, int> profile2 = Tokenize(s2);

            return Similarity(profile1, profile2);
        }

        /// <summary></summary>
        /// <param name="profile1"></param>
        /// <param name="profile2"></param>
        /// <returns></returns>
        public static double Similarity(IDictionary<string, int> profile1, IDictionary<string, int> profile2)
        {
            return DotProduct(profile1, profile2) /
                  (Norm(profile1) * Norm(profile2));
        }

        private static double DotProduct(IDictionary<string, int> profile1,
                                         IDictionary<string, int> profile2)
        {
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

        /// <summary>Compute the norm L2 : sqrt(Sum_i( v_i²)).</summary>
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