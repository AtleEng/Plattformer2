using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class JumpPadScript : Component, IScript
    {
        float jumpForce = -25; //Upwards force

        Animator anim;

        public override void Start()
        {
            //cache entitys animator
            anim = gameEntity.GetComponent<Animator>();
        }

        //if a dynimic physicsBody enter the JumpPads triggerZone, shoot it upwards by jumpForce and play animation
        public override void OnTrigger(Collider other)
        {
            PhysicsBody? pB = other.gameEntity.GetComponent<PhysicsBody>();
            if (pB != null)
            {
                if (pB.physicsType == PhysicsBody.PhysicsType.dynamicType)
                {
                    pB.velocity.Y = jumpForce;
                    anim.PlayAnimation("Jump");
                }
            }
        }

    }
}
