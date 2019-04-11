using System.IO;

namespace rasyidf.Analogi.Core
{
    public class IdenticalSizeReason : IReason
    {
        #region Properties

        public double Bias { get; set; } = 1;
        public double Index { get; set; }
        public string ReasonString => $"The File Size is Identical with {TargetFile}";

        public string TargetFile { get; private set; }
        public double Treshold => 0.5;

        #endregion Properties

        #region Methods

        

        public void SetTargetFile(string value)
        {
            TargetFile = value;
        }
                                               
        public double Check(string source, string target)
        {
            long len1 = new FileInfo(source).Length;
            long len2 = new FileInfo(target).Length;
            Index = (len1 == len2) ? 1 : 0;

            return Index;
        }

        #endregion Methods
    }
}