//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
    [TestClass]
    public class BusinessLogicImplementationUnitTest
    {
        [TestMethod]
        public void ConstructorTestMethod()
        {
            using (BusinessLogicImplementation newInstance = new(new DataLayerConstructorFixcure()))
            {
                bool newInstanceDisposed = true;
                newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
                Assert.IsFalse(newInstanceDisposed);
            }
        }

        [TestMethod]
        public void DisposeTestMethod()
        {
            DataLayerDisposeFixcure dataLayerFixcure = new DataLayerDisposeFixcure();
            BusinessLogicImplementation newInstance = new(dataLayerFixcure);
            Assert.IsFalse(dataLayerFixcure.Disposed);
            bool newInstanceDisposed = true;
            newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
            Assert.IsFalse(newInstanceDisposed);
            newInstance.Dispose();
            newInstance.CheckObjectDisposed(x => newInstanceDisposed = x);
            Assert.IsTrue(newInstanceDisposed);
            Assert.ThrowsException<ObjectDisposedException>(() => newInstance.Dispose());
            Assert.ThrowsException<ObjectDisposedException>(() => newInstance.Start(0, (position, ball) => { }));
            Assert.IsTrue(dataLayerFixcure.Disposed);
        }

        [TestMethod]
        public void StartTestMethod()
        {
            DataLayerStartFixcure dataLayerFixcure = new();
            using (BusinessLogicImplementation newInstance = new(dataLayerFixcure))
            {
                int called = 0;
                int numberOfBalls2Create = 10;
                newInstance.Start(
                    numberOfBalls2Create,
                    (startingPosition, ball) => { called++; Assert.IsNotNull(startingPosition); Assert.IsNotNull(ball); });
                Assert.AreEqual<int>(numberOfBalls2Create, called);
                Assert.IsTrue(dataLayerFixcure.StartCalled);
                Assert.IsTrue(SpinWait.SpinUntil(() => Volatile.Read(ref dataLayerFixcure.MoveCallCount) > 0, TimeSpan.FromSeconds(1)));
                Assert.IsTrue(SpinWait.SpinUntil(() => dataLayerFixcure.MovedBallsCount == numberOfBalls2Create, TimeSpan.FromSeconds(1)));
                Assert.IsTrue(dataLayerFixcure.LastDeltaTime >= 0.0);
                Assert.AreEqual<int>(numberOfBalls2Create, dataLayerFixcure.NumberOfBallseCreated);
            }
        }

        #region testing instrumentation

        private class DataLayerConstructorFixcure : Data.DataAbstractAPI
        {
            public override void Dispose()
            { }

            public override void Start(int numberOfBalls, Action<IVector, Data.IBall> upperLayerHandler)
            {
                throw new NotImplementedException();
            }

            public override void Move(Data.IBall ball, double deltaTime)
            {
                throw new NotImplementedException();
            }
        }

        private class DataLayerDisposeFixcure : Data.DataAbstractAPI
        {
            internal bool Disposed = false;

            public override void Dispose()
            {
                Disposed = true;
            }

            public override void Start(int numberOfBalls, Action<IVector, Data.IBall> upperLayerHandler)
            {
                throw new NotImplementedException();
            }

            public override void Move(Data.IBall ball, double deltaTime)
            {
                throw new NotImplementedException();
            }
        }

        private class DataLayerStartFixcure : Data.DataAbstractAPI
        {
            internal bool StartCalled = false;
            internal int MoveCallCount = 0;
            internal int NumberOfBallseCreated = -1;
            private readonly object MovedBallsLock = new();
            private readonly HashSet<Data.IBall> MovedBalls = [];

            internal int MovedBallsCount
            {
                get
                {
                    lock (MovedBallsLock)
                    {
                        return MovedBalls.Count;
                    }
                }
            }

            public override void Dispose()
            { }

            public override void Start(int numberOfBalls, Action<IVector, Data.IBall> upperLayerHandler)
            {
                StartCalled = true;
                NumberOfBallseCreated = numberOfBalls;
                for (int i = 0; i < numberOfBalls; i++)
                {
                    DataVectorFixture position = new(i * 100.0, 0.0);
                    upperLayerHandler(position, new DataBallFixture(position));
                }
            }

            internal double LastDeltaTime = 0.0;

            public override void Move(Data.IBall ball, double deltaTime)
            {
                LastDeltaTime = deltaTime;
                lock (MovedBallsLock)
                {
                    MovedBalls.Add(ball);
                }
                Interlocked.Increment(ref MoveCallCount);
            }

            private record DataVectorFixture(double x, double y) : Data.IVector;

            private class DataBallFixture : Data.IBall
            {
                internal DataBallFixture(IVector currentPosition)
                {
                    CurrentPosition = currentPosition;
                }

                public IVector Velocity { get; set; } = new DataVectorFixture(0.0, 0.0);

                public double Mass => 1.0;

                public double Diameter => 20.0;

                public IVector CurrentPosition { get; }

                public event EventHandler<IVector>? NewPositionNotification = null;
            }
        }

    #endregion testing instrumentation
    }
}
