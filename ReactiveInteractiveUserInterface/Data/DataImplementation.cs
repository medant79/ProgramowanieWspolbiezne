//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor

        public DataImplementation() : this(null)
        {
        }

        internal DataImplementation(IDiagnosticLogger? diagnosticLogger)
        {
            DiagnosticLogger = diagnosticLogger ?? new DiagnosticLogger();
        }

        #endregion ctor

        #region DataAbstractAPI

        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));

            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            lock (BallsListLock)
            {
                BallsList.Clear();

                for (int i = 0; i < numberOfBalls; i++)
                {
                    Vector startingPosition = new(
                        RandomGenerator.Next((int)_ballDiameter, (int)(_boardWidth - _ballDiameter)),
                        RandomGenerator.Next((int)_ballDiameter, (int)(_boardHeight - _ballDiameter))
                    );

                    Vector initialVelocity = new(
                        (RandomGenerator.NextDouble() - 0.5) * _maxInitialSpeed,
                        (RandomGenerator.NextDouble() - 0.5) * _maxInitialSpeed
                    );

                    Ball newBall = new(startingPosition, initialVelocity, _ballMass, _ballDiameter, i, DiagnosticLogger);

                    BallsList.Add(newBall);
                    upperLayerHandler(startingPosition, newBall);
                }
            }
        }

        public override void Move(double deltaTime)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));

            lock (BallsListLock)
            {
                foreach (Ball item in BallsList)
                    item.Move(_boardWidth, _boardHeight, _ballDiameter, deltaTime);
            }
        }

        #endregion DataAbstractAPI

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    lock (BallsListLock)
                    {
                        BallsList.Clear();
                    }

                    DiagnosticLogger.Dispose();
                }
                Disposed = true;
            }
            else
                throw new ObjectDisposedException(nameof(DataImplementation));
        }

        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region private

        private bool Disposed = false;

        private readonly object BallsListLock = new();

        private Random RandomGenerator = new();
        private List<Ball> BallsList = [];

        private readonly double _boardWidth = 400.0;
        private readonly double _boardHeight = 420.0;
        private readonly double _ballDiameter = 20.0;
        private readonly double _ballMass = 1.0;

        private readonly double _maxInitialSpeed = 300.0;

        private readonly IDiagnosticLogger DiagnosticLogger;

        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
        {
            lock (BallsListLock)
            {
                returnBallsList(BallsList);
            }
        }

        [Conditional("DEBUG")]
        internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
        {
            lock (BallsListLock)
            {
                returnNumberOfBalls(BallsList.Count);
            }
        }

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

    #endregion TestingInfrastructure
    }
}
