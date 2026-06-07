using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.Data
{
    public interface IDiagnosticLogger: IDisposable
    {
        void Log(BallDiagnosticData data);
    }
}
