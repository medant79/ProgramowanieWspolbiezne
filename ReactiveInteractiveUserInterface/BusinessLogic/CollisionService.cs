using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    internal static class CollisionService
    {
        internal static bool AreColliding(Data.IBall firstBall, Data.IBall secondBall)
        {
            double firstCenterX = firstBall.CurrentPosition.x + firstBall.Diameter / 2.0;
            double firstCenterY = firstBall.CurrentPosition.y + firstBall.Diameter / 2.0;

            double secondCenterX = secondBall.CurrentPosition.x + secondBall.Diameter / 2.0;
            double secondCenterY = secondBall.CurrentPosition.y + secondBall.Diameter / 2.0;

            double dx = firstCenterX - secondCenterX;
            double dy = firstCenterY - secondCenterY;

            double distanceSquared = dx * dx + dy * dy;

            double radiusSum = firstBall.Diameter / 2.0 + secondBall.Diameter / 2.0;

            return distanceSquared <= radiusSum * radiusSum;
        }

        internal static void ResolveElasticCollision(Data.IBall firstBall, Data.IBall secondBall)
        {
            double firstCenterX = firstBall.CurrentPosition.x + firstBall.Diameter / 2.0;
            double firstCenterY = firstBall.CurrentPosition.y + firstBall.Diameter / 2.0;

            double secondCenterX = secondBall.CurrentPosition.x + secondBall.Diameter / 2.0;
            double secondCenterY = secondBall.CurrentPosition.y + secondBall.Diameter / 2.0;

            double dx = firstCenterX - secondCenterX;
            double dy = firstCenterY - secondCenterY;

            double distanceSquared = dx * dx + dy * dy;

            if (distanceSquared == 0)
                return;

            double dvx = firstBall.Velocity.x - secondBall.Velocity.x;
            double dvy = firstBall.Velocity.y - secondBall.Velocity.y;

            double dotProduct = dvx * dx + dvy * dy;

            if (dotProduct >= 0)
                return;

            double firstMass = firstBall.Mass;
            double secondMass = secondBall.Mass;

            double impulse = (2.0 * dotProduct) / ((firstMass + secondMass) * distanceSquared);

            double firstVelocityX = firstBall.Velocity.x - impulse * secondMass * dx;
            double firstVelocityY = firstBall.Velocity.y - impulse * secondMass * dy;

            double secondVelocityX = secondBall.Velocity.x + impulse * firstMass * dx;
            double secondVelocityY = secondBall.Velocity.y + impulse * firstMass * dy;

            firstBall.Velocity = new CollisionVector(firstVelocityX, firstVelocityY);
            secondBall.Velocity = new CollisionVector(secondVelocityX, secondVelocityY);
        }

        private record CollisionVector(double x, double y) : Data.IVector;
    }
}
