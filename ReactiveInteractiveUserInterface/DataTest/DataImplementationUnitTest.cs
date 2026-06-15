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
    public class DataImplementationUnitTest
        {
        [TestMethod]
        public void ConstructorTestMethod()
        {
            using (DataImplementation newInstance = new DataImplementation())
            {
                IEnumerable<IBall>? ballsList = null;
                newInstance.CheckBallsList(x => ballsList = x);
                Assert.IsNotNull(ballsList);
                int numberOfBalls = 0;
                newInstance.CheckNumberOfBalls(x => numberOfBalls = x);
                Assert.AreEqual<int>(0, numberOfBalls);
            }
        }

        [TestMethod]
        public void DisposeTestMethod()
        {
            DataImplementation newInstance = new DataImplementation();
            bool newInstanceDisposed = false;
            newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
            Assert.IsFalse(newInstanceDisposed);
            newInstance.Dispose();
            newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
            Assert.IsTrue(newInstanceDisposed);
            IEnumerable<IBall>? ballsList = null;
            newInstance.CheckBallsList(x => ballsList = x);
            Assert.IsNotNull(ballsList);
            newInstance.CheckNumberOfBalls(x => Assert.AreEqual<int>(0, x));
            Assert.ThrowsException<ObjectDisposedException>(() => newInstance.Dispose());
            Assert.ThrowsException<ObjectDisposedException>(() => newInstance.Start(0, (position, ball) => { }));
        }

        [TestMethod]
        public void StartTestMethod()
        {
            using (DataImplementation newInstance = new DataImplementation())
            {
                int numberOfCallbackInvoked = 0;
                int numberOfBalls2Create = 10;
                newInstance.Start(
                    numberOfBalls2Create,
                    (startingPosition, ball) =>
                    {
                        numberOfCallbackInvoked++;
                        Assert.IsTrue(startingPosition.x >= 0);
                        Assert.IsTrue(startingPosition.y >= 0);
                        Assert.IsNotNull(ball);
                    }
                );
                Assert.AreEqual<int>(numberOfBalls2Create, numberOfCallbackInvoked);
                newInstance.CheckNumberOfBalls(x => Assert.AreEqual<int>(10, x));
            }
        }

        [TestMethod]
        public void MoveTestMethod()
        {
            using (DataImplementation newInstance = new DataImplementation())
            {
                int numberOfCallbackInvoked = 0;
                IBall? movingBall = null;
                newInstance.Start(
                    1,
                    (startingPosition, ball) =>
                    {
                        movingBall = ball;
                        ball.NewPositionNotification += (sender, position) =>
                        {
                            Assert.IsNotNull(sender);
                            Assert.IsNotNull(position);
                            numberOfCallbackInvoked++;
                        };
                    }
                );

                Assert.IsNotNull(movingBall);
                newInstance.Move(movingBall, 1.0);

                Assert.AreEqual<int>(1, numberOfCallbackInvoked);
            }
        }

        private class FakeDiagnosticLogger : IDiagnosticLogger
        {
            public int LogCallCount { get; private set; }
            public bool Disposed { get; private set; }

            public void Log(BallDiagnosticData data)
            {
                LogCallCount++;
            }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        [TestMethod]
        public void Move_Should_Use_Injected_DiagnosticLogger()
        {
            FakeDiagnosticLogger logger = new();

            using (DataImplementation newInstance = new DataImplementation(logger))
            {
                IBall? movingBall = null;
                newInstance.Start(1, (position, ball) => { movingBall = ball; });

                Assert.IsNotNull(movingBall);
                newInstance.Move(movingBall, 1.0);

                Assert.IsTrue(logger.LogCallCount > 0);
            }

            Assert.IsTrue(logger.Disposed);
        }
    }
}
