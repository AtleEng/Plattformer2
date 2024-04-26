using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;
public class EnemyRun : EnemyBehavior
{
    //X Movement
    private float moveSpeed = 20;
    private float moveInput = -1;

    Collider? wallCheck;
    Collider? ledgeCheck;
    PhysicsBody pB;

    public EnemyRun(float moveSpeed, Collider? wallCheck, Collider? ledgeCheck, PhysicsBody pB)
    {
        this.moveSpeed = moveSpeed;
        this.wallCheck = wallCheck;
        this.ledgeCheck = ledgeCheck;
        this.pB = pB;
    }
    public override void BehaviorUpdate(float delta)
    {
        pB.velocity.X += moveInput * moveSpeed * delta; //move enemy
        if (wallCheck != null) //Check so it has a wallCheck
        {
            if (wallCheck.isColliding) //Check if colliding with a wall, if it is flip the wallCheck- and move in other direction
            {
                wallCheck.gameEntity.transform.position = new Vector2(-wallCheck.gameEntity.transform.position.X, wallCheck.gameEntity.transform.position.Y);
                moveInput *= -1;
            }
        }
        if (ledgeCheck != null) //Check if about to fall, if it is flip the ledgeCheck- and move in other direction
        {
            if (!ledgeCheck.isColliding && pB.velocity.Y <= 0)
            {
                ledgeCheck.gameEntity.transform.position = new Vector2(-ledgeCheck.gameEntity.transform.position.X, ledgeCheck.gameEntity.transform.position.Y);
                moveInput *= -1;
            }
        }
    }
}