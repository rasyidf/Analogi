
using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System.Collections.Generic;

namespace Analogi.Core;

public class MOSSEngine
{
    public IScanner Scanner { get; set; }

    public MOSSEngine(IScanner scanner)
    {
        Scanner = scanner; DetectionResults = [];
    }

    public MOSSEngine()
    {
        DetectionResults = [];

    }

    public List<IPipeline> Pipelines { get; set; }
    public List<IExtractor> Extractors { get; set; }
    public List<DetectionResultViewModel> DetectionResults { get; private set; }

    public void Start()
    {
        DetectionResults = [];
        List<CodeFile> files = new(Scanner.Scan());

        DetectionResultViewModel tmpDR;
        for (int i = 0; i < files.Count; i++)
        {
            tmpDR = new DetectionResultViewModel(files[i]);
            for (int j = 0; j < files.Count; j++)
            {

                if (i != j)
                {
                    IEnumerable<IReason> reasons = StartPipeline(files[i], files[j]);
                    tmpDR.Model.Reasons.AddRange(reasons ?? []);
                }
            }
            DetectionResults.Add(tmpDR);
        }

    }

    private List<IReason> StartPipeline(CodeFile v1, CodeFile v2)
    {

        PipelineData pd = new();
        pd.AddMetadata("path", "file", [v1.Path, v2.Path]);

        foreach (IExtractor ext in Extractors)
        {
            ext.Run(ref pd, "file.1", v1);
            ext.Run(ref pd, "file.2", v2);
        }

        for (int i = 0; i < Pipelines.Count; i++)
        {
            IPipeline pipeline = Pipelines[i];
            pd = pipeline.Run(pd);
        }

        return pd.Reasons;
    }
}