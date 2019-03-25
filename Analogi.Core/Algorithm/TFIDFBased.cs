/*
 * MIT
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace rasyidf.Analogi
{
    public abstract class TFIDFBased
    { 
        /// <summary>
        /// </summary>  
        protected TFIDFBased( )
        {  
        }

        /// <summary>
        /// Pattern for finding multiple following spaces
        /// </summary>
        private static readonly Regex SPACE_REG = new Regex("\\s+");

        protected IDictionary<string, int> GetProfile(string s)
        {
            var tokens = new Dictionary<string, int>();

            string[] string_no_space = SPACE_REG.Replace(s, " ").Split(' ');


            foreach (string token in string_no_space) { 
                if (tokens.ContainsKey(token))
                {
                    tokens[token] = tokens.TryGetValue(token, out int old) ? old + 1:1;
                }
                else
                {
                    tokens.Add(token, 1);
                }
               
            }
            
            return new ReadOnlyDictionary<string, int>(tokens);
        }
    }
}