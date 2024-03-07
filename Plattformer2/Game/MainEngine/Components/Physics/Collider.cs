using System.Numerics;
using Engine;

namespace Physics
{
    public class Collider : Component
    {
        public Vector2 offset;
        public Vector2 scale = Vector2.One;

        public bool isTrigger;

        public int layer;

        public bool isColliding;
        public Collider(bool isTrigger, int layer)
        {
            this.isTrigger = isTrigger;
            this.layer = layer;
        }
    }

}