using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.DataTest
{
    [TestClass]
    public class DataDiagnosticsIntegrationTest
    {
        [TestMethod]
        public void DataImplementation_Should_Write_Diagnostics_During_Move()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");

            DiagnosticLogger logger = new(filePath);

            using (DataImplementation data = new(logger))
            {
                IBall? movingBall = null;
                data.Start(1, (position, ball) => { movingBall = ball; });

                Assert.IsNotNull(movingBall);
                data.Move(movingBall, 1.0);
            }

            Assert.IsTrue(File.Exists(filePath));

            string content = File.ReadAllText(filePath);

            Assert.IsFalse(string.IsNullOrWhiteSpace(content));
            Assert.IsTrue(content.Contains(";"));

            File.Delete(filePath);
        }
    }
}
