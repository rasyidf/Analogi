using System.Collections.Generic;

namespace Analogi.Core
{
                    
        public interface IScanner
        {
            IEnumerable<CodeFile> Scan();
            string Path { get; set; }
        }       
}