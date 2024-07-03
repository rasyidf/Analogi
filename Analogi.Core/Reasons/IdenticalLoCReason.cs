using Analogi.Core.Interfaces;
using System;
using System.IO;

namespace Analogi.Core.Reasons
{
    public class IdenticalLoCReason : IReason
    {
        public double Weight { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"The File {CommentIs} Line is Identical with {TargetFile}";

        private string CommentIs => IsComment ? "Comment" : "Code";
        public int IndexPercentage => Convert.ToInt32(Index * 100);
        public string TargetFile { get; private set; }
        public double Treshold => 0.5;

        public bool IsComment { get; set; }

        public void SetTargetFile(string value)
        {
            TargetFile = new FileInfo(value).Name;
        }

        public double Check(string source, string target)
        {
            return !(string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) && source != target ? 0 : 1;
        }
    }
}