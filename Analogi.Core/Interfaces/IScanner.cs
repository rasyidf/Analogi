using System.Collections.Generic;

namespace rasyidf.Analogi.Core
{
                    
        public interface IScanner
        {
            IEnumerable<CodeFile> Scan();
            string Path { get; set; }
        }       
}