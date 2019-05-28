using System.Collections.Generic;

namespace Analogi.Core
{
    public interface IExtractor
    {
        string Name { get; }  

        void Run(ref PipelineData pd, string id, CodeFile data);
    }
}