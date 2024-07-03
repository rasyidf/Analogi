using Analogi.Core.Models;
using System.Collections.Generic;

namespace Analogi.Core.Interfaces
{

    public interface IScanner
    {
        IEnumerable<CodeFile> Scan();
        string Path { get; set; }
    }
}