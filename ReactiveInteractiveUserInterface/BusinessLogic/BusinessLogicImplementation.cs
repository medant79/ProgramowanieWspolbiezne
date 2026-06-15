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

            MoveTimer = TimeProvider.CreateTimer(
                Move,
                null,
                Timeout.InfiniteTimeSpan,
                Timeout.InfiniteTimeSpan);
        }

        #endregion ctor

        #region BusinessLogicAbstractAPI

        public override void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(BusinessLogicImplementation));

            MoveTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            MoveTimer.Dispose();

            layerBellow.Dispose();
            Disposed = true;

        }

        public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(BusinessLogicImplementation));

            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));

            MoveTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
      
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

            LastMoveTimestamp = TimeProvider.GetTimestamp();

            MoveTimer.Change(TimeSpan.Zero, FrameInterval);
        }

        #endregion BusinessLogicAbstractAPI

        #region private

        private bool Disposed = false;
        private int MoveInProgress = 0;

        private readonly TimeProvider TimeProvider;
        private readonly ITimer MoveTimer;
        private long LastMoveTimestamp = 0;
        private static readonly TimeSpan FrameInterval = TimeSpan.FromMilliseconds(16);

        private readonly UnderneathLayerAPI layerBellow;

        private readonly List<Data.IBall> DataBalls = new();
        private readonly object DataBallsLock = new();
    
        private void Move(object? state)
        {
            if (Disposed)
                return;

            if (Interlocked.Exchange(ref MoveInProgress, 1) == 1)
                return;

            try
            {
                long nowTimestamp = TimeProvider.GetTimestamp();
                TimeSpan elapsedTime = TimeProvider.GetElapsedTime(LastMoveTimestamp, nowTimestamp);
                LastMoveTimestamp = nowTimestamp;

                double deltaTime = elapsedTime.TotalSeconds;

                layerBellow.Move(deltaTime);

                lock (DataBallsLock)
                {
                    ResolveCollisions();
                }
            }
            finally
            {
                Interlocked.Exchange(ref MoveInProgress, 0);
            }

        }

        private void ResolveCollisions()
        {
            for (int i = 0; i < DataBalls.Count; i++)
            {
                for (int j = i + 1; j < DataBalls.Count; j++)
                {
                    Data.IBall FirstBall = DataBalls[i];
                    Data.IBall SecondBall = DataBalls[j];

                    if(CollisionService.AreColliding(FirstBall, SecondBall))
                    {
                        CollisionService.ResolveElasticCollision(FirstBall, SecondBall);
                    }
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
