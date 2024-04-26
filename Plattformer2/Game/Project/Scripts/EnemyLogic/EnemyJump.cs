using System.Numerics;
using System.Collections.Generic;
using Engine;
using Physics;
using Animation;
public class EnemyJump : EnemyBehavior
{
    //Normal jumping
    float jumpForce = 0;
    float jumpTimer;
    float jumpTime;

    //isGrounded
    Collider groundCheck;
    PhysicsBody pB;

    public EnemyJump(float jumpForce, float jumpTimer, Collider groundCheck, PhysicsBody pB)
    {
        this.jumpForce = jumpForce;
        this.jumpTimer = jumpTimer;
        this.groundCheck = groundCheck;
        this.pB = pB;
    }
    public override void BehaviorUpdate(float delta)
    {
        if (groundCheck.isColliding) //If the enemy is on the ground
        {
            enemyBase.isGrounded = true;

            //This timer tracks the cooldown between jumps
            jumpTime += delta;
            if (jumpTime >= jumpTimer) //when the cooldown is done, jump!
            {
                jumpTime = 0;
                pB.velocity.Y = -jumpForce;
            }
        }
        else //isnt on the ground
        {
            enemyBase.isGrounded = false;
        }
    }
}