/*
 * MIT
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Analogi
{
    /// <summary>
    /// K-means (shingle) based tokenizer
    /// </summary>
    public abstract class ShingleBased
    {
        #region Fields

        private const int DEFAULT_K = 3;

        /// <summary>Pattern for finding multiple following spaces</summary>
        private static readonly Regex SPACE_REG = new Regex("\\s+");

        #endregion Fields

        #region Constructors

        /// <summary></summary>
        /// <param name="k"></param>
        /// <exception cref="ArgumentOutOfRangeException">If k is less than or equal to 0.</exception>
        protected ShingleBased(int k)
        {
            if (k <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "k should be positive!");
            }

            this.K = k;
        }

        protected ShingleBased() : this(DEFAULT_K)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>Return k, the length of k-shingles (aka n-grams).</summary>
        protected int K { get; }

        #endregion Properties

        #region Methods

        protected IDictionary<string, int> GetProfile(string s)
        {
            var shingles = new Dictionary<string, int>();

            string string_no_space = SPACE_REG.Replace(s, " ");

            for (int i = 0; i < (string_no_space.Length - K + 1); i++)
            {
                string shingle = string_no_space.Substring(i, K);
                shingles[shingle] = shingles.TryGetValue(shingle, out int old) ? old + 1 : 1;
            }

            return new ReadOnlyDictionary<string, int>(shingles);
        }

        #endregion Methods
    }
}