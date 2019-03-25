/*
 * MIT
 */

using System.Collections.Generic;

namespace rasyidf.Analogi.Core
{
    public class DetectionResult
    {
        private readonly CodeFile CodeFile; 

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
                    if (item.Index < 0.3f)
                    {
                        continue;
                    }
                    r *= item.Index * item.Bias;
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
                }
                else if (Reasons.Count == 1)
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