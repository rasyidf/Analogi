using System.Collections.Generic;

namespace rasyidf.Analogi.Core
{
                    
        public interface IScanner
        {
            IEnumerable<string> Scan();
            string Path { get; set; }
        }       
}