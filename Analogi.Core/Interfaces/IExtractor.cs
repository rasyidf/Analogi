using System.Collections.Generic;

namespace Analogi.Core
{
    public interface IExtractor
    {
        string Name { get; }  

        List<string> Run(CodeFile data);
    }
}