/*
 * MIT
 */

using rasyidf.Analogi.Interfaces;
using System;

namespace rasyidf.Analogi
{
    /// <summary>
    /// Distance metric based on Longest Common Subsequence, from the notes "An
    /// LCS-based string metric" by Daniel Bakkelund.
    /// </summary>
    public class MetricLCS : IMetricStringDistance, INormalizedStringDistance
    {
        private readonly LongestCommonSubsequence lcs = new LongestCommonSubsequence();

        /// <summary>
        /// Distance metric based on Longest Common Subsequence, computed as
        /// 1 - |LCS(s1, s2)| / max(|s1|, |s2|).
        /// </summary>
        /// <param name="s1">The first string to compare.</param>
        /// <param name="s2">The second string to compare.</param>
        /// <returns>LCS distance metric</returns>
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

            int m_len = Math.Max(s1.Length, s2.Length);

            if (m_len == 0)
            {
                return 0.0;
            }

            return 1.0
                    - (1.0 * lcs.Length(s1, s2))
                    / m_len;
        }
    }
}