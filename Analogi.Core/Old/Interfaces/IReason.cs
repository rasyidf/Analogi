using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rasyidf.Analogi.Core
{
    public interface IReason
    {
        string ReasonString { get; }

        double Index { get; set; }
        
        double Bias { get; set; }

        double Treshold { get;   }

        string TargetFile { get; }

        void SetTargetFile(string value);

        double Check(string source, string target);
    }
}
