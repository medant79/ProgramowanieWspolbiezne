using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
    [TestClass]
    public class CollisionServiceUnitTest
    {
        [TestMethod]
        public void AreColliding_Should_Return_True_When_Balls_Touch()
        {
            FakeBall firstBall = new(
              position: new VectorFixture(0.0, 0.0),
              velocity: new VectorFixture(1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            FakeBall secondBall = new(
              position: new VectorFixture(20.0, 0.0),
              velocity: new VectorFixture(-1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            bool result = CollisionService.AreColliding(firstBall, secondBall);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AreColliding_Should_Return_False_When_Balls_Are_Far_Away()
        {
            FakeBall firstBall = new(
              position: new VectorFixture(0.0, 0.0),
              velocity: new VectorFixture(1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            FakeBall secondBall = new(
              position: new VectorFixture(100.0, 100.0),
              velocity: new VectorFixture(-1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            bool result = CollisionService.AreColliding(firstBall, secondBall);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ResolveElasticCollision_Should_Change_Velocities_When_Balls_Collide()
        {
            FakeBall firstBall = new(
              position: new VectorFixture(0.0, 0.0),
              velocity: new VectorFixture(1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            FakeBall secondBall = new(
              position: new VectorFixture(20.0, 0.0),
              velocity: new VectorFixture(-1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            CollisionService.ResolveElasticCollision(firstBall, secondBall);

            Assert.AreEqual(-1.0, firstBall.Velocity.x, 0.001);
            Assert.AreEqual(1.0, secondBall.Velocity.x, 0.001);
        }

        [TestMethod]
        public void ResolveElasticCollision_Should_Use_Mass_When_Masses_Are_Different()
        {
            FakeBall lightBall = new(
              position: new VectorFixture(0.0, 0.0),
              velocity: new VectorFixture(2.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            FakeBall heavyBall = new(
              position: new VectorFixture(20.0, 0.0),
              velocity: new VectorFixture(0.0, 0.0),
              mass: 3.0,
              diameter: 20.0);

            CollisionService.ResolveElasticCollision(lightBall, heavyBall);

            Assert.AreEqual(-1.0, lightBall.Velocity.x, 0.001);
            Assert.AreEqual(1.0, heavyBall.Velocity.x, 0.001);
        }

        [TestMethod]
        public void ResolveElasticCollision_Should_Not_Change_Velocities_When_Balls_Are_Moving_Away()
        {
            FakeBall firstBall = new(
              position: new VectorFixture(0.0, 0.0),
              velocity: new VectorFixture(-1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            FakeBall secondBall = new(
              position: new VectorFixture(20.0, 0.0),
              velocity: new VectorFixture(1.0, 0.0),
              mass: 1.0,
              diameter: 20.0);

            CollisionService.ResolveElasticCollision(firstBall, secondBall);

            Assert.AreEqual(-1.0, firstBall.Velocity.x, 0.001);
            Assert.AreEqual(1.0, secondBall.Velocity.x, 0.001);
        }

        private class FakeBall : Data.IBall
        {
            public FakeBall(IVector position, IVector velocity, double mass, double diameter)
            {
                CurrentPosition = position;
                Velocity = velocity;
                Mass = mass;
                Diameter = diameter;
            }

            public event EventHandler<IVector>? NewPositionNotification;

            public IVector Velocity { get; set; }

            public double Mass { get; }

            public double Diameter { get; }

            public IVector CurrentPosition { get; }
        }

        private record VectorFixture(double x, double y) : IVector;
    }
}
