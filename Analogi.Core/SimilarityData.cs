/*
 * MIT
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Yaudah.Core
{
    public class CodeFile
    {
        public CodeFile(string url)
        {
            this.Path = url;
            var a = new FileInfo(url);
            this.Name = a.Name;
            this.CreationTime = a.CreationTime;
            this.Length = a.Length;
        }

        public string Name { get; private set; }
        public DateTime CreationTime { get; set; }
        public long Length { get; private set; }
        public string Path { get; set; }
        public int Index { get; set; }

        internal string ReadAll()
        {
            return File.ReadAllText(Path);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class DetectionResult
    {
        private readonly CodeFile CodeFile; 
        private readonly double index;

        public string Name { get => this.CodeFile.Name; }

        public List<IReason> Reasons { get; set; }

        public double Index
        {
            get
            { 
                if (Reasons.Count == 0)
                {
                    return 0;
                }

                double r = 1;
                foreach (var item in Reasons)
                {
                    r *= item.Index;
                }
                return r;
            }
        }

        public int IndexPercentage => (int) Index * 100;



        public string Reason
        {

            get
            {
                if (Reasons.Count > 1)
                {
                    return "There are some reasons, double click to see details";
                } else if (Reasons.Count == 1)
                {
                    return Reasons[0].ReasonString;
                }
                return "File is Fine";
            }
        }
        public DetectionResult(string path)
        {
            this.CodeFile = new CodeFile(path); Reasons = new List<IReason>();
        }
        public DetectionResult(CodeFile code)
        {
            this.CodeFile = code; Reasons = new List<IReason>();
        }
    }
}