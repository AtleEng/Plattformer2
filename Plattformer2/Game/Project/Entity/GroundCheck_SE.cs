using Physics;

namespace Engine
{
    public class Check : GameEntity
    {
        public Check(int colliderLayer)
        {
            name = "Check";

            PhysicsBody physicsBody = new()
            {
                physicsType = PhysicsBody.PhysicsType.staticType
            };
            AddComponent<PhysicsBody>(physicsBody);
            Collider collider = new
            (
                true, colliderLayer
            );
            AddComponent<Collider>(collider);
            AddComponent<Sprite>(new Sprite());
        }
    }
}