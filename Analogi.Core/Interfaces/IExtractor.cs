using Analogi.Core.Models;

namespace Analogi.Core.Interfaces
{
    public interface IExtractor
    {
        string Name { get; }

        void Run(ref PipelineData pd, string id, CodeFile data);
    }
}