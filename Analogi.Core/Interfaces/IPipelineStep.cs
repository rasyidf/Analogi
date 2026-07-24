using Analogi.Core.Models;

namespace Analogi.Core.Interfaces;

/// <summary>
/// A single step in the analysis pipeline.
/// Steps are composable: extractors, preprocessors, analyzers, and postprocessors all implement this.
/// </summary>
public interface IPipelineStep
{
    string Name { get; }

    /// <summary>
    /// Whether this step is active. Disabled steps are skipped during pipeline execution.
    /// </summary>
    bool IsEnabled { get; set; }

    PipelineContext Run(PipelineContext context);
}
