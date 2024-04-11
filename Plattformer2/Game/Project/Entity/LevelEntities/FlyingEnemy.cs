using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;

namespace Engine
{
    public class FlyingEnemy : GameEntity, IKill
    {
        static Texture2D texture;
        public FlyingEnemy()
        {
            if (texture.Id == 0)
            {
                texture = Raylib.LoadTexture(@"Game\Project\Images\EnemiesSprites.png");
            }
            name = "DroneEnemy";

            //sprite
            Sprite sprite = new Sprite
            {
                spriteSheet = texture,
                spriteGrid = new Vector2(5, 4),
                FrameIndex = 5
            };
            AddComponent<Sprite>(sprite);

            //animation
            Animator animator = new(sprite);

            animator.AddAnimation("Run", new(new int[] { 5, 6, 7, 8 }, 0.1f, true));

            AddComponent<Animator>(animator);

            //physics
            PhysicsBody physicsBody = new PhysicsBody
            {
                dragX = 10f,
                dragY = 0,
                Gravity = new Vector2(0, 0),
                velocity = Vector2.Zero
            };
            AddComponent<PhysicsBody>(physicsBody);

            Collider collider = new Collider
            (
                false, 3
            )
            {
                scale = new Vector2(0.75f, 1)
            };
            AddComponent<Collider>(collider);

            Check wallCheck = new(2);
            EntityManager.SpawnEntity(wallCheck, new Vector2(-0.3f, 0f), new Vector2(0.4f, 0.4f), this.transform);

            EnemyAIBase enemyAI = new();
            enemyAI.beheviors.Add(new EnemyRun(15, wallCheck.GetComponent<Collider>(), null, physicsBody));
            AddComponent<EnemyAIBase>(enemyAI);
        }
    }
}