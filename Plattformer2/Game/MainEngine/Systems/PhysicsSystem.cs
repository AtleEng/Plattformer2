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
        void UpdatePhysics(PhysicsBody physicsBody, float delta)
        {
            if (physicsBody.physicsType == PhysicsBody.PhysicsType.staticType)
            {
                return;
            }
            // Apply drag separately to X and Y
            physicsBody.velocity.X *= 1 - physicsBody.dragX * delta;
            physicsBody.velocity.Y *= 1 - physicsBody.dragY * delta;

            physicsBody.acceleration += physicsBody.Gravity * delta * 60;

            physicsBody.velocity += physicsBody.acceleration * delta;

            physicsBody.gameEntity.transform.position += physicsBody.velocity * delta;

            physicsBody.acceleration = Vector2.Zero;
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