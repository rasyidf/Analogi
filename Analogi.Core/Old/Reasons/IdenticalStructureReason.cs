namespace rasyidf.Analogi.Core
{
    public class IdenticalStructureReason : IReason
    {
        #region Properties

        public double Bias { get; set; }
        public double Index { get; set; }
        public string ReasonString => $"The Code has same program structure with :{TargetFile}";

        public string TargetFile { get; private set; }
        public double Treshold => 0.5;

        #endregion Properties

        #region Methods
        

        public void SetTargetFile(string value)
        {
            TargetFile = value;
        }
                             
        public double Check(string source, string target)
        {                                                 // Not Implemented
            Index = 0;
            return 0;
        }

        #endregion Methods
    }
}