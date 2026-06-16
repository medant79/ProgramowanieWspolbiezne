//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System.Diagnostics;
using UnderneathLayerAPI = TP.ConcurrentProgramming.Data.DataAbstractAPI;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    internal class BusinessLogicImplementation : BusinessLogicAbstractAPI
    {
        #region ctor

        public BusinessLogicImplementation() : this(null, null)
        { }

        internal BusinessLogicImplementation(UnderneathLayerAPI? underneathLayer) : this(underneathLayer, null)
        { }

        internal BusinessLogicImplementation(UnderneathLayerAPI? underneathLayer, TimeProvider? timeProvider)
        {
            layerBellow = underneathLayer == null ? UnderneathLayerAPI.GetDataLayer() : underneathLayer;
            TimeProvider = timeProvider ?? TimeProvider.System;
        }

        #endregion ctor

        #region BusinessLogicAbstractAPI

        public override void Dispose()
        {
            lock (LifecycleLock)
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(BusinessLogicImplementation));

                StopBallWorkers();
                layerBellow.Dispose();
                Disposed = true;
            }

        }

        public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
        {
            lock (LifecycleLock)
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(BusinessLogicImplementation));

                if (upperLayerHandler == null)
                    throw new ArgumentNullException(nameof(upperLayerHandler));

                StopBallWorkers();
      
                lock(DataBallsLock)
                {
                    DataBalls.Clear();
                }

                layerBellow.Start(numberOfBalls, (startingPosition, databall) =>
                {
                    lock (DataBallsLock)
                    {
                        DataBalls.Add(databall);
                    }

                    upperLayerHandler(
                        new Position(startingPosition.x, startingPosition.y),
                        new Ball(databall)
                    );
                });

                StartBallWorkers();
            }
        }

        #endregion BusinessLogicAbstractAPI

        #region private

        private bool Disposed = false;

        private readonly TimeProvider TimeProvider;
        private static readonly TimeSpan FrameInterval = TimeSpan.FromMilliseconds(1);

        private readonly UnderneathLayerAPI layerBellow;

        private readonly List<Data.IBall> DataBalls = new();
        private readonly object DataBallsLock = new();
        private readonly object LifecycleLock = new();
        private readonly object BallWorkersLock = new();
        private readonly List<Thread> BallWorkers = new();
        private CancellationTokenSource? SimulationCancellationTokenSource = null;
    
        private void StartBallWorkers()
        {
            Data.IBall[] ballsSnapshot;

            lock (DataBallsLock)
            {
                ballsSnapshot = DataBalls.ToArray();
            }

            CancellationTokenSource cancellationTokenSource = new();
            List<Thread> workers = [];

            foreach (Data.IBall ball in ballsSnapshot)
            {
                Thread worker = new(() => MoveBall(ball, cancellationTokenSource.Token))
                {
                    IsBackground = true,
                    Name = "Ball worker"
                };
                worker.Start();
                workers.Add(worker);
            }

            lock (BallWorkersLock)
            {
                SimulationCancellationTokenSource = cancellationTokenSource;
                BallWorkers.Clear();
                BallWorkers.AddRange(workers);
            }
        }

        private void StopBallWorkers()
        {
            CancellationTokenSource? cancellationTokenSource;
            Thread[] workers;

            lock (BallWorkersLock)
            {
                cancellationTokenSource = SimulationCancellationTokenSource;
                workers = BallWorkers.ToArray();
                SimulationCancellationTokenSource = null;
                BallWorkers.Clear();
            }

            if (cancellationTokenSource == null)
                return;

            cancellationTokenSource.Cancel();

            foreach (Thread worker in workers)
                if (worker.IsAlive)
                    worker.Join(TimeSpan.FromSeconds(2));

            cancellationTokenSource.Dispose();
        }

        private void MoveBall(Data.IBall ball, CancellationToken cancellationToken)
        {
            long lastMoveTimestamp = TimeProvider.GetTimestamp();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (cancellationToken.WaitHandle.WaitOne(FrameInterval))
                    break;

                long nowTimestamp = TimeProvider.GetTimestamp();
                TimeSpan elapsedTime = TimeProvider.GetElapsedTime(lastMoveTimestamp, nowTimestamp);
                lastMoveTimestamp = nowTimestamp;

                try
                {
                    layerBellow.Move(ball, elapsedTime.TotalSeconds);

                    lock (DataBallsLock)
                    {
                        ResolveCollisionsFor(ball);
                    }
                }
                catch (ObjectDisposedException) when (Disposed || cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private void ResolveCollisionsFor(Data.IBall movingBall)
        {
            foreach (Data.IBall otherBall in DataBalls)
            {
                if (ReferenceEquals(movingBall, otherBall))
                    continue;

                if(CollisionService.AreColliding(movingBall, otherBall))
                {
                    CollisionService.ResolveElasticCollision(movingBall, otherBall);
                }
            }
        }

        #endregion private

        #region TestingInfrastructure

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }

        #endregion TestingInfrastructure
    }
}
