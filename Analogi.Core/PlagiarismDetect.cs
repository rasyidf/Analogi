using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
 
using System.IO;

namespace Yaudah.Core
{
    public class PlagiarismDetect
    {
        public List<DetectionResult> DetectionResult { get; private set; }
        public string Path { get; }

        public PlagiarismDetect(string path)
        {
            Path = path;
        }
        public void Start()
        {
            DetectionResult = new List<DetectionResult>();
            // List All Souce Code

            var files = System.IO.Directory.EnumerateFiles(Path, "*.cpp|*.c|*.cs|*.py").ToArray();

             
            DetectionResult dr;
            for (int i = 0; i < files.Count(); i++)
            {
                dr = new DetectionResult(files[i]);
                for (int j = 0; j < files.Count(); j++)
                {
                    if (i != j)
                    {
                        var sd = CheckSimilarity(files[i], files[j]);
                        dr.Reasons.AddRange(sd);
                    }
                }
                DetectionResult.Add(dr);
            }
             
        }
         
        private List<IReason> CheckSimilarity(string path1, string path2)
        {
            List<IReason> tmpReasons = new List<IReason>();
            List<IReason> CheckReasons = new List<IReason>() { new CosineSimilarityReason() };

            var n = new IdenticalSizeReason();
            n.SetTargetFile(path2);
            if (n.Check(path1, path2) == 1)
            {

                tmpReasons.Add(n);
                return tmpReasons;
            }


            var a =  File.ReadAllText(path1);
            var b =  File.ReadAllText(path2);

            foreach (var item in CheckReasons)
            {
                if (item.Check(a,b) > 0.6f)
                {
                    item.SetTargetFile(path2);
                    tmpReasons.Add(item);
                }
            }

            return tmpReasons;
        }
    }
}
