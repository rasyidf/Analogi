using Analogi.Core.Models;

namespace Analogi.Core.Interfaces;

/// <summary>
/// A single step in the analysis pipeline.
/// Steps are composable: extractors, preprocessors, analyzers, and postprocessors all implement this.
/// </summary>
public interface IPipelineStep
{
    string Name { get; }
    PipelineContext Run(PipelineContext context);
}
