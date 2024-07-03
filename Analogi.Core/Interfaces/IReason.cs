namespace Analogi.Core.Interfaces
{
    public interface IReason
    {
        string ReasonString { get; }

        double Index { get; set; }

        int IndexPercentage { get; }

        double Weight { get; set; }

        double Treshold { get; }

        string TargetFile { get; }

        void SetTargetFile(string value);

        double Check(string source, string target);
    }
}
