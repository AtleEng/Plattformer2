using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class PortalScript : Component, IScript
    {
        Animator? anim;

        GameManagerScript gM;

        public override void Start()
        {
            anim = gameEntity.GetComponent<Animator>();
            if (anim != null)
            {
                anim.PlayAnimation("Idle");
            }

            GameManager gameManager = EntityManager.GetGameEntity<GameManager>();
            gM = gameManager.GetComponent<GameManagerScript>();
        }

        public override void OnTrigger(Collider other)
        {
            PlayerMovement? player = other.gameEntity.GetComponent<PlayerMovement>();
            if (player != null)
            {
                EntityManager.DestroyEntity(player.gameEntity);
                gM.ChangeLevel(LoadingManager.CurrentLevel + 1);
            }
        }

    }
}
