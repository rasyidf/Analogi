using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Analogi.Core.PreProcessors
{
    public class CodeFilter : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            PipelineData p = data;
            Dictionary<string, List<string>> m = data.FileMetadataMappings.ToDictionary(kvp => kvp.Key, Filter);

            p.FileMetadataMappings = m;
            return p;
        }

        private static List<string> Filter(KeyValuePair<string, List<string>> kvp)
        {
            return kvp.Key == "file.path" ? kvp.Value : kvp.Value.ConvertAll(x => x.Replace('\t', ' ').Trim());
        }
    }
}