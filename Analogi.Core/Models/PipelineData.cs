using Analogi.Core.Interfaces;
using System.Collections.Generic;

namespace Analogi.Core.Models
{
    public class PipelineData
    {
        public List<IReason> Reasons { get; set; }

        public Dictionary<string, List<string>> FileMetadataMappings { get; set; }
        internal void AddMetadata(string name, string file, List<string> Data)
        {
            FileMetadataMappings ??= [];
            FileMetadataMappings.Add($"{file}.{name}", Data);
        }

        internal void AddReason(IReason reason, string targetFileName)
        {
            Reasons ??= [];
            reason.SetTargetFile(targetFileName);
            Reasons.Add(reason);
        }
    }
}