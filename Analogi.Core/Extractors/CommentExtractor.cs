﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace rasyidf.Analogi.Core.Extractors
{
    public class CommentExtractor : IExtractor
    {
        readonly Regex CommentRegex = new Regex("//[^(\n|\r)]*\n?");

        readonly Regex CommentBlockRegex = new Regex(@"/\*[^*]*\*+(?:[^/*][^*]*\*+)*/");

        public List<string> Run(string data)
        {
            var file = System.IO.File.ReadAllText(data);
            var v = CommentBlockRegex.Matches(file);
            var w = CommentRegex.Matches(file);
            return Run(v,w);
        }

        private List<string> Run(MatchCollection v, MatchCollection w)
        {
            var str = new List<string>();
            foreach (Match item in v)
            {
                str.Add(item.Value);
            }
            foreach (Match item in w)
            {
                str.Add(item.Value);
            }
            return str;
        }

        public string Name { get => "comment"; } 
    }
}