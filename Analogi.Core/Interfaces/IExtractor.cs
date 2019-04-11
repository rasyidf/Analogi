using System.Collections.Generic;

namespace rasyidf.Analogi.Core
{
    public interface IExtractor
    {
        string Name { get; }  

        List<string> Run(CodeFile data);
    }
}