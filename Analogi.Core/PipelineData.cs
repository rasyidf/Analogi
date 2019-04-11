using System;
using System.Collections.Generic;
using rasyidf.Analogi.Core.Analyzers;

namespace rasyidf.Analogi.Core
{
    public class PipelineData
    {
        public List<IReason> Reasons { get; set; }

        public Dictionary<string, List<string>> Metadatas { get; set; }
        internal void AddMetadata(string name, string file, List<string> Data)
        {
            if (Metadatas == null) Metadatas = new Dictionary<string, List<string>>();
            Metadatas.Add($"{file}.{name}", Data);
        }

        internal void AddReason(IReason reason, string targetfile)
        {                                                      
            if (Reasons == null) Reasons = new List<IReason>();
            reason.SetTargetFile(targetfile);            
            Reasons.Add(reason);
        }
    }
}