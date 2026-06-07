//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data.Test
{
    [TestClass]
    public class BallUnitTest
        {
        [TestMethod]
        public void ConstructorTestMethod()
        {
            Vector testinVector = new Vector(0.0, 0.0);
            Ball newInstance = new(testinVector, testinVector, mass: 1.0, diameter: 20.0, ballId: 0, diagnosticLogger: null);
        }

        [TestMethod]
        public void MoveTestMethod()
        {
            Vector initialPosition = new(10.0, 10.0);
            Ball newInstance = new(initialPosition, new Vector(0.0, 0.0), mass: 1.0, diameter: 20.0, ballId: 0, diagnosticLogger: null);
            IVector curentPosition = new Vector(0.0, 0.0);
            int numberOfCallBackCalled = 0;
            newInstance.NewPositionNotification += (sender, position) => { Assert.IsNotNull(sender); curentPosition = position; numberOfCallBackCalled++; };
            newInstance.Move(400.0, 420.0, 20.0, 1.0);
            Assert.AreEqual<int>(1, numberOfCallBackCalled);
            Assert.AreEqual<IVector>(initialPosition, curentPosition);
        }

        [TestMethod]
        public void MoveWithDeltaTimeTestMethod()
        {
            Vector initialPosition = new(10.0, 10.0);
            Vector initialVelocity = new(2.0, 3.0);

            Ball newInstance = new(initialPosition, initialVelocity, mass: 1.0, diameter: 20.0, ballId: 0, diagnosticLogger: null);

            newInstance.Move(400.0, 420.0, 20.0, 0.5);

            IVector position = newInstance.CurrentPosition;

            Assert.AreEqual(11.0, position.x, 0.001);
            Assert.AreEqual(11.5, position.y, 0.001);
        }

        [TestMethod]
        public void MassPropertyTestMethod()
        {
            Vector testinVector = new Vector(0.0, 0.0);
            double expectedMass = 1.0;
            Ball newInstance = new(testinVector, testinVector, mass: expectedMass, diameter: 20.0, ballId: 0, diagnosticLogger: null);
            Assert.AreEqual<double>(expectedMass, newInstance.Mass);
        }

        [TestMethod]
        public void DiameterPropertyTestMethod()
        {
            Vector testinVector = new Vector(0.0, 0.0);
            double expectedDiameter = 20.0;
            Ball newInstance = new(testinVector, testinVector, mass: 1.0, diameter: expectedDiameter, ballId: 0, diagnosticLogger: null);
            Assert.AreEqual<double>(expectedDiameter, newInstance.Diameter);
        }

        [TestMethod]
        public void CurrentPositionTestMethod()
        {
            Vector initialPosition = new(10.0, 15.0);
            Ball newInstance = new(initialPosition, new Vector(0.0, 0.0), mass: 1.0, diameter: 20.0, ballId: 0, diagnosticLogger: null);
            IVector position = newInstance.CurrentPosition;
            Assert.AreEqual<double>(10.0, position.x);
            Assert.AreEqual<double>(15.0, position.y);
        }

        [TestMethod]
        public void ThreadSafetyVelocityTestMethod()
        {
            Vector initialPosition = new(50.0, 50.0);
            Vector initialVelocity = new(5.0, 5.0);
            Ball newInstance = new(initialPosition, initialVelocity, mass: 1.0, diameter: 20.0, ballId: 0, diagnosticLogger: null);
      
            int readCount = 0;
            int writeCount = 0;

            Task readTask = Task.Run(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        _ = newInstance.Velocity;
                        readCount++;
                    }
                }
            );

            Task writeTask = Task.Run(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        newInstance.Velocity = new Vector(i * 0.1, i * 0.1);
                        writeCount++;
                    }
                }
            );

            Task.WaitAll(readTask, writeTask);
            Assert.AreEqual<int>(100, readCount);
            Assert.AreEqual<int>(100, writeCount);
        }
    }
}