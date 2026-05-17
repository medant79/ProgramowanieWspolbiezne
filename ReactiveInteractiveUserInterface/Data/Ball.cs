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

    internal Ball(Vector initialPosition, Vector initialVelocity, double mass, double diameter)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
      _mass = mass;
      _diameter = diameter;
    }

    #endregion ctor

    #region IBall

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }

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
    private double _mass;
    private double _diameter;
    private readonly object _lock = new();

    private void RaiseNewPositionChangeNotification()
    {
      lock (_lock)
        NewPositionNotification?.Invoke(this, Position);
    }

    internal void Move(double boardWidth, double boardHeight, double ballDiameter)
    {
      lock (_lock)
      {
        double nextX = Position.x + Velocity.x;
        double nextY = Position.y + Velocity.y;

        if (nextX < 0)
        {
          nextX = 0;
          Velocity = new Vector(-Velocity.x, Velocity.y);
        }
        else if (nextX > boardWidth - ballDiameter)
        {
          nextX = boardWidth - ballDiameter;
          Velocity = new Vector(-Velocity.x, Velocity.y);
        }

        if (nextY < 0)
        {
          nextY = 0;
          Velocity = new Vector(Velocity.x, -Velocity.y);
        }
        else if (nextY > boardHeight - ballDiameter)
        {
          nextY = boardHeight - ballDiameter;
          Velocity = new Vector(Velocity.x, -Velocity.y);
        }

        Position = new Vector(nextX, nextY);
        RaiseNewPositionChangeNotification();
      }
    }

    #endregion private
  }
}