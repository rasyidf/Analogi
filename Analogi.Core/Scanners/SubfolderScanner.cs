using Analogi.Core.Interfaces;
using Analogi.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Analogi.Core
{

    namespace Scanner
    {
        public class SubfolderScanner(string path) : IScanner
        {
            public string Path { get; set; } = path;

            public IEnumerable<CodeFile> Scan()
            {
                return (from item in Directory.EnumerateDirectories(Path)
                        select new CodeFile(item)).ToList();
            }
        }
    }
}