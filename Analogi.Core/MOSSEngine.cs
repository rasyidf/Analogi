
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Analogi.Core
{
    public class MOSSEngine
    {
        public IScanner Scanner { get; set; }

        public MOSSEngine(IScanner scanner)
        {
            this.Scanner = scanner; DetectionResults = new List<DetectionResult>();

        }

        public MOSSEngine()
        {
            DetectionResults = new List<DetectionResult>();

        }

        public List<IPipeline> Pipelines { get; set; }
        public List<IExtractor> Extractors { get; set; }
        public List<DetectionResult> DetectionResults { get; private set; }

        public void Start()
        {
            List<CodeFile> files = new List<CodeFile>(Scanner.Scan());

            DetectionResult tmpDR;
            for (int i = 0; i < files.Count; i++)
            {
                // Buat DetectionResult untuk File [i]
                tmpDR = new DetectionResult(files[i]);
                for (int j = 0; j < files.Count; j++)
                {

                    if (i != j)
                    {
                        // Bandingkan File[i] dengan File[j]
                        // Jalankan Pipeline
                        var reasons = StartPipeline(files[i], files[j]);
                        // tambahkan reasons ke DetectionResult
                        tmpDR.Reasons.AddRange(reasons ?? new List<IReason>());
                    }
                }
                // Jika Semua file selesai dibandingkan 
                // tambahkan DetectionResult ke DetectionResults (Kumpulan DR)
                DetectionResults.Add(tmpDR);
            }

        }

        private IEnumerable<IReason> StartPipeline(CodeFile v1, CodeFile v2)
        {

            // Persiapkan Pipeline Data
            var pd = new PipelineData();
            // Tambahkan Lokasi File
            pd.AddMetadata("path", "file", new List<string>() { v1.Path, v2.Path });

            // Jalankan Semua Ekstraktor
            for (int i = 0; i < Extractors.Count; i++)
            {
                    var ext = Extractors[i];
                    pd.AddMetadata(ext.Name, "file.1", ext.Run(v1));
                    pd.AddMetadata(ext.Name, "file.2", ext.Run(v2)); 
            }

            // Jalankan Semua Pipeline dengan Pipeline data yang sudah ada
            for (int i = 0; i < Pipelines.Count; i++)
            {
                IPipeline pipeline = Pipelines[i];
                pd = pipeline.Run(pd);
            }
            // Keluarkan Kumpulan Reason untuk ditambahkan ke DetectionResult
            return pd.Reasons;
        }
    }
}