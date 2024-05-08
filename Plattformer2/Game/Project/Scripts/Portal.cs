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
            //Play the idle animation
            anim = gameEntity.GetComponent<Animator>();
            if (anim != null)
            {
                anim.PlayAnimation("Idle");
            }
            //Get the gameManagerScript
            GameManager gameManager = EntityManager.GetGameEntity<GameManager>();
            gM = gameManager.GetComponent<GameManagerScript>();
        }

        public override void OnTrigger(Collider other) // if triggered by player, change level
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
