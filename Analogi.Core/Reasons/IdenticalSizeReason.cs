using System.IO;

namespace Yaudah.Core
{
    public class IdenticalSizeReason : IReason
    {
        public string ReasonString => $"The File Size is Identical with {TargetFile}";

        private string targetFile;

        public string TargetFile => targetFile;

        public double Index { get; set; }

        public void SetTargetFile(string value)
        {
            targetFile = value;
        }

        public double Check(string source, string target)
        {

            Index = new FileInfo(source).Length == new FileInfo(target).Length ? 1 : 0;
            return Index;

        }
    }
}
