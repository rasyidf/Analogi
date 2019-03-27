using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
 
using System.IO;

namespace rasyidf.Analogi.Core
{
    public class PlagiarismDetect
    {
        public List<DetectionResult> DetectionResults { get; private set; }

        private const string SupportedExtension = "cpp|c|cs|py|rb|pas|vb|r|js";
        public List<IReason> ReasonsPipeline = new List<IReason>() {
            new CosineSimilarityReason(),
            new IdenticalStructureReason()
        };

        public string Path { get; }

        public PlagiarismDetect(string path)
        {
            Path = path;
        }
        public void Start()
        {
            DetectionResults = new List<DetectionResult>();
            // List All Souce Code

            var files = Directory
                       .EnumerateFiles(Path, "*.*")
                       .Where( f => SupportedExtension.Split('|').Contains(f.ToLower().Split('.').Last())).ToArray();



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

            if (n.Check(ref path1,ref path2) == 1)
            {

                tmpReasons.Add(n);
                return tmpReasons;
            }


            var a =  File.ReadAllText(path1);
            var b =  File.ReadAllText(path2);

            foreach (var reason in ReasonsPipeline)
            {
                if (reason.Check(ref a,ref b) > reason.Treshold)
                {
                    reason.SetTargetFile(path2);
                    tmpReasons.Add(reason);
                }
            }

            return tmpReasons;
        }
    }
}
