using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaudah.Core
{
    public interface IReason
    {
        string ReasonString { get; }

        double Index { get; set; }
        string TargetFile { get; }

        void SetTargetFile(string value);
        double Check(string source, string target);
    }
}
