/*
 * MIT
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace rasyidf.Analogi
{
    public abstract class ShingleBased
    {
        private const int DEFAULT_K = 3;

        /// <summary>
        /// Return k, the length of k-shingles (aka n-grams).
        /// </summary>
        protected int k { get; }

        /// <summary>
        /// Pattern for finding multiple following spaces
        /// </summary>
        private static readonly Regex SPACE_REG = new Regex("\\s+");

        /// <summary>
        /// </summary>
        /// <param name="k"></param>
        /// <exception cref="ArgumentOutOfRangeException">If k is less than or equal to 0.</exception>
        protected ShingleBased(int k)
        {
            if (k <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "k should be positive!");
            }

            this.k = k;
        }

        protected ShingleBased() : this(DEFAULT_K)
        {
        }

        protected IDictionary<string, int> GetProfile(string s)
        {
            Dictionary<string, int> shingles = new Dictionary<string, int>();

            string string_no_space = SPACE_REG.Replace(s, " ");

            for (int i = 0; i < (string_no_space.Length - k + 1); i++)
            {
                string shingle = string_no_space.Substring(i, k); 
                shingles[shingle] = shingles.TryGetValue(shingle, out int old) ? old + 1 : 1;
            }

            return new ReadOnlyDictionary<string, int>(shingles);
        }
    }
}

namespace rasyidf.Analogi
{
    public abstract class TFIDFBased
    {
        private const int DEFAULT_K = 3;

        /// <summary>
        /// Return k, the length of k-shingles (aka n-grams).
        /// </summary>
        protected int k { get; }

        /// <summary>
        /// Pattern for finding multiple following spaces
        /// </summary>
        private static readonly Regex SPACE_REG = new Regex("\\s+");

        /// <summary>
        /// </summary>
        /// <param name="k"></param>
        /// <exception cref="ArgumentOutOfRangeException">If k is less than or equal to 0.</exception>
        protected TFIDFBased(int k)
        {
            if (k <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "k should be positive!");
            }

            this.k = k;
        }

        protected TFIDFBased() : this(DEFAULT_K)
        {
        }

        protected IDictionary<string, int> GetProfile(string s)
        {
            Dictionary<string, int> shingles = new Dictionary<string, int>();

            string[] string_no_space = SPACE_REG.Replace(s, " ").Split(' ');


            foreach (string token in string_no_space) { 
                if (shingles.ContainsKey(token))
                {
                    shingles[token] = shingles.TryGetValue(token, out int old) ? old + 1:1;
                }
                else
                {
                    shingles.Add(token, 1);
                }
               
            }
            
            return new ReadOnlyDictionary<string, int>(shingles);
        }
    }
}