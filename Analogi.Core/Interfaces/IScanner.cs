using Analogi.Core.Models;

namespace Analogi.Core.Interfaces;

public interface IScanner
{
    IReadOnlyList<CodeFile> Scan();
}
