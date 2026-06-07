using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.DataTest
{
    [TestClass]
    public class DiagnosticLoggerUnitTest
    {
        [TestMethod]
        public void DiagnosticLogger_Should_Write_Data_To_File()
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");

            using (DiagnosticLogger logger = new DiagnosticLogger(filePath))
            {
                logger.Log(new BallDiagnosticData(
                    DateTime.UtcNow,
                    1,
                    10.0,
                    20.0,
                    1.5,
                    -2.5,
                    1.0,
                    20.0));
            }

            Assert.IsTrue(File.Exists(filePath));

            string content = File.ReadAllText(filePath);

            Assert.IsTrue(content.Contains("1;10"));
            Assert.IsTrue(content.Contains("20"));
            Assert.IsTrue(content.Contains("1.5") || content.Contains("1,5") == false);

            File.Delete(filePath);
        }
    }
}
