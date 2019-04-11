using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace rasyidf.Analogi.Core
{
    public class DetectionEngine
    {
        #region Fields

        public List<IReason> ReasonsPipeline = new List<IReason>() {
            new CosineSimilarityReason(),
            new IdenticalStructureReason()
        };

        private const string SupportedExtension = "cpp|c|cs|py|rb|pas|vb|r|js";

        #endregion Fields

        #region Constructors

        public DetectionEngine(string path)
        {
            Path = path;
        }

        #endregion Constructors

        #region Properties

        public List<DetectionResult> DetectionResults { get; private set; }
        public string Path { get; }

        #endregion Properties

        #region Methods

        public void Start()
        {

            DetectionResults = new List<DetectionResult>();
            // List All Souce Code

            string[] files = Directory
                       .EnumerateFiles(Path, "*.*")
                       .Where(f => SupportedExtension.Split('|').Contains(f.ToLower().Split('.').Last())).ToArray();

            DetectionResult tmpDR;
            for (int i = 0; i < files.Count(); i++)
            {
                tmpDR = new DetectionResult(files[i]);
                for (int j = 0; j < files.Count(); j++)
                {
                    if (i != j)
                    {
                        tmpDR.Reasons.AddRange(CheckPlagiarism(files[i], files[j]));
                    }
                }
                DetectionResults.Add(tmpDR);
            }
        }

        private List<IReason> CheckPlagiarism(string path1, string path2)
        {
            var tmpReasons = new List<IReason>();

            var n = new IdenticalSizeReason();
            n.SetTargetFile(path2);

            if (n.Check(path1, path2) == 1)
            {
                tmpReasons.Add(n);
                return tmpReasons;
            }

            string a = File.ReadAllText(path1);
            string b = File.ReadAllText(path2);

            foreach (IReason reason in ReasonsPipeline)
            {
                if (reason.Check(a, b) > reason.Treshold)
                {
                    reason.SetTargetFile(path2);
                    tmpReasons.Add(reason);
                }
            }

            return tmpReasons;
        }

        #endregion Methods
    }
}