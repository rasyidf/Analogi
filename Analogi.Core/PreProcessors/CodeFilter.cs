﻿

using System;
using System.Linq;
using System.Collections.Generic;

namespace rasyidf.Analogi.Core.PreProcessors
{
    public class CodeFilter : IPipeline
    {
        public PipelineData Run(PipelineData data)
        {
            var p = data;
            var m = data.Metadatas.ToDictionary(kvp => kvp.Key, kvp => Filter(kvp));

            p.Metadatas = m;
            return p;
        }

        private static List<string> Filter(KeyValuePair<string, List<string>> kvp)
        {
            if (kvp.Key == "file.path")
                return kvp.Value;
            return kvp.Value.ConvertAll(x => x.Trim().Replace('\t',' '));
        }
    }
}