using System.Numerics;
using CoreEngine;
using Engine;

namespace Physics
{
    [Serializable]
    public class PhysicsBody : Component
    {
        public PhysicsBody() { }

        public PhysicsType physicsType = PhysicsType.dynamicType;
        public float mass = 1;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 acceleration = Vector2.Zero;
        public float dragX = 0f;
        public float dragY = 0f;

        public float elasticity = 0f;

        public Vector2 Gravity = new Vector2(0, 9.82f);

        public void AddForce(Vector2 force)
        {
            if (mass == 0) { mass = 1; }

            acceleration += force / mass;
        }
        public override string PrintStats()
        {
            return $"Velocity: {velocity}";
        }

        public enum PhysicsType
        {
            dynamicType, staticType
        }
    }
}