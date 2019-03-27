namespace rasyidf.Analogi.Core
{
    public class IdenticalStructureReason : IReason
    {
        public string ReasonString => $"The Code has same program structure with :{TargetFile}";

        public string TargetFile { get; private set; }
        public double Index { get; set; }
        public double Bias { get; set; }

        public double Treshold { get => 0.5; }
        public void SetTargetFile(string value)
        {
            TargetFile = value;
        }
         
        public double Check(ref string source,ref string target)
        {
            // Not Implemented
            Index = 0;
            return 0;
        }
    }
}
