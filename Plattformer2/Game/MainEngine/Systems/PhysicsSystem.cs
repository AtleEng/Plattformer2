using System.Numerics;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace Physics
{
    public class PhysicsSystem : GameSystem
    {
        public override void Update(float delta)
        {
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                PhysicsBody? physicsBody = gameEntity.GetComponent<PhysicsBody>();

                if (physicsBody != null)
                {
                    UpdatePhysics(physicsBody, delta);
                    Core.UpdateChildren(gameEntity.transform.parent);
                }
            }
        }
        void UpdatePhysics(PhysicsBody pB, float delta)
        {
            if (pB.physicsType == PhysicsBody.PhysicsType.staticType)
            {
                return;
            }
            // Apply drag separately to X and Y
            pB.velocity.X *= 1 - pB.dragX * delta;
            pB.velocity.Y *= 1 - pB.dragY * delta;

            pB.acceleration += pB.Gravity * delta * 60;

            pB.velocity += pB.acceleration * delta;

            pB.gameEntity.transform.position += pB.velocity * delta;

            pB.acceleration = Vector2.Zero;
        }
        public static class PhysicsSettings
        {
            public static bool[,] collisionMatrix = new bool[4, 4]
            {
            //true = collide / false = ignore collision
            //player ground check enemy
            { false, true, false, true}, //player
            { true, false, true, true}, //ground
            { false, true, false, false}, //check
            { false, true, false, false} //enemy
            };
        }
    }
}