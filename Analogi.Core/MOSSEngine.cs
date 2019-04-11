
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace rasyidf.Analogi.Core
{
    public class MOSSEngine
    {
        private IScanner Scanner;

        public MOSSEngine(IScanner scanner)
        {
            this.Scanner = scanner; DetectionResults = new List<DetectionResult>();

        }

        public List<IPipeline> Pipelines { get; set; }
        public List<IExtractor> Extractors { get; set; }
        public List<DetectionResult> DetectionResults { get; private set; }

        public void Start()
        {
            List<string> files = new List<string>(Scanner.Scan());

            DetectionResult tmpDR;
            for (int i = 0; i < files.Count; i++)
            {
                tmpDR = new DetectionResult(files[i]);
                for (int j = 0; j < files.Count; j++)
                {
                    if (i != j)
                    {
                        tmpDR.Reasons.AddRange(StartPipeline(files[i], files[j])?? new List<IReason>());
                    }
                }
                DetectionResults.Add(tmpDR);
            }
                   
        }

        private IEnumerable<IReason> StartPipeline(string v1, string v2)
        {
            var pd = new PipelineData();
            pd.AddMetadata("path", "file", new List<string>() { v1, v2 });

            for (int i = 0; i < Extractors.Count; i++)
            {
                var ext = Extractors[i];
                pd.AddMetadata(ext.Name, "file.1", ext.Run(v1));
                pd.AddMetadata(ext.Name, "file.2", ext.Run(v2));
                if (ext.Name=="comment" && pd.Metadatas["file.1.comment"].Count> 0)
                {
                    Debug.Print(pd.Metadatas["file.2.comment"].ToString());
                }
            }

            for (int i = 0; i < Pipelines.Count; i++)
            {
                IPipeline pipeline = Pipelines[i];
                pd = pipeline.Run(pd);
            }

            return pd.Reasons;
        }
    }
}