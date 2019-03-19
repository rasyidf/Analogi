namespace Yaudah.Core
{
    public class IdenticalStructureReason : IReason
    {
        public string ReasonString => $"The Code has same program structure with :{TargetFile}";

        public string TargetFile { get; private set; }
        public double Index { get; set; }

        public void SetTargetFile(string value)
        {
            TargetFile = value;
        }
         
        public double Check(string source, string target)
        {
            Index = 0;
            return 0;
        }
    }
}
