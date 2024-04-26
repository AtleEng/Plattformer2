using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    //This class controll all diffrent enemy behaviors
    public class EnemyAIBase : Component, IScript
    {
        public List<EnemyBehavior> behaviors = new(); //All enemys diffrent AI

        public bool isGrounded = true; //Check to see if enemy is on the ground

        //Physics
        float maxVelocityX = 10;
        float maxVelocityY = 30;
        public PhysicsBody pB;

        //Sprite & Animation
        Sprite sprite;
        Animator anim;

        EnemyStates enemyStates = EnemyStates.idle;
        public override void Start()
        {
            //Cache components
            pB = gameEntity.GetComponent<PhysicsBody>();
            anim = gameEntity.GetComponent<Animator>();
            sprite = gameEntity.GetComponent<Sprite>();
            //Get all behaviors a refrences to this class
            foreach (EnemyBehavior behavior in behaviors)
            {
                behavior.enemyBase = this;
            }
        }
        public override void Update(float delta)
        {
            //Update all behaviors
            foreach (EnemyBehavior behavior in behaviors)
            {
                behavior.BehaviorUpdate(delta);
            }
            //animation handling
            if (isGrounded)
            {
                if (pB.velocity.X != 0)
                {
                    if (enemyStates != EnemyStates.running)
                    {
                        enemyStates = EnemyStates.running;
                        anim.PlayAnimation("Run");
                    }
                }
                else
                {
                    if (enemyStates != EnemyStates.idle)
                    {
                        enemyStates = EnemyStates.idle;
                        anim.PlayAnimation("Idle");
                    }
                }
            }
            else
            {
                if (pB.velocity.Y < 0)
                {
                    if (enemyStates != EnemyStates.jump)
                    {
                        enemyStates = EnemyStates.jump;
                        anim.PlayAnimation("Jump");
                    }
                }
                else
                {
                    if (enemyStates != EnemyStates.fall)
                    {
                        enemyStates = EnemyStates.fall;
                        anim.PlayAnimation("Fall");
                    }
                }
            }
            //Turn the sprite in the direction of movement
            if (pB.velocity.X > 0)
            {
                sprite.isFlipedX = false;
            }
            else if (pB.velocity.X < 0)
            {
                sprite.isFlipedX = true;
            }

            //Cap the velocity
            pB.velocity.X = Math.Clamp(pB.velocity.X, -maxVelocityX, maxVelocityX);
            pB.velocity.Y = Math.Clamp(pB.velocity.Y, -maxVelocityY, maxVelocityY);
        }
        public enum EnemyStates
        {
            idle, running, jump, fall, landing,
        }
    }

    public abstract class EnemyBehavior //base class for behaviors
    {
        public EnemyAIBase enemyBase;
        public virtual void BehaviorUpdate(float delta) { }
    }
}