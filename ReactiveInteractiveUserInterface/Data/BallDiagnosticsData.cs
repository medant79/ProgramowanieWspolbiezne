using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.Data
{
    public record BallDiagnosticData(
        DateTime Timestamp,
        int BallId,
        double X,
        double Y,
        double VelocityX,
        double VelocityY,
        double Mass,
        double Diameter
    );
}
