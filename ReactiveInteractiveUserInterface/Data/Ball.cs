//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity, double mass, double diameter, int ballId, IDiagnosticLogger? diagnosticLogger)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
            _mass = mass;
            _diameter = diameter;
            _ballId = ballId;
            _diagnosticLogger = diagnosticLogger;
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity
        {
            get
            {
                lock (_lock)
                    return _velocity;
            }
            set
            {
                lock (_lock)
                    _velocity = value;
            }
        }

        public double Mass 
        { 
            get 
            { 
            lock (_lock) 
                return _mass; 
            } 
        }

        public double Diameter 
        { 
            get 
            { 
            lock (_lock) 
                return _diameter; 
            } 
        }

        public IVector CurrentPosition
        {
            get
            {
            lock (_lock)
                return Position;
            }
        }

        #endregion IBall

        #region private

        private Vector Position;
        private IVector _velocity = new Vector(0.0, 0.0);
        private readonly double _mass;
        private readonly double _diameter;
        private readonly object _lock = new();
        private readonly int _ballId;
        private readonly IDiagnosticLogger? _diagnosticLogger;

        internal void Move(double boardWidth, double boardHeight, double ballDiameter, double deltaTime)
        {
            IVector newPosition;

            lock (_lock)
            {
                double nextX = Position.x + _velocity.x * deltaTime;
                double nextY = Position.y + _velocity.y * deltaTime;

                if (nextX < 0)
                {
                    nextX = 0;
                    _velocity = new Vector(-_velocity.x, _velocity.y);
                }
                else if (nextX > boardWidth - ballDiameter)
                {
                    nextX = boardWidth - ballDiameter;
                    _velocity = new Vector(-_velocity.x, _velocity.y);
                }

                if (nextY < 0)
                {
                    nextY = 0;
                    _velocity = new Vector(_velocity.x, -_velocity.y);
                }
                else if (nextY > boardHeight - ballDiameter)
                {
                    nextY = boardHeight - ballDiameter;
                    _velocity = new Vector(_velocity.x, -_velocity.y);
                }

                Position = new Vector(nextX, nextY);
                newPosition = Position;
            }
            _diagnosticLogger?.Log(new BallDiagnosticData(
                DateTime.UtcNow,
                _ballId,
                newPosition.x,
                newPosition.y,
                Velocity.x,
                Velocity.y,
                Mass,
                Diameter)
            );

            NewPositionNotification?.Invoke(this, newPosition);
        }
        #endregion private
    }
}