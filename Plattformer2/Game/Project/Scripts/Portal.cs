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

        bool hasEnteredPortal;
        float timeToChangeScene = 1;

        Sound enterSound = Raylib.LoadSound(@"Game\Project\Audio\level-up-bonus-sequence-2-186891.mp3");

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

        public override void Update(float delta)
        {
            if (hasEnteredPortal)
            {
                timeToChangeScene -= delta;
                if (timeToChangeScene < 0)
                {
                    gM.ChangeLevel(LoadingManager.CurrentLevel + 1);
                }
            }
        }

        public override void OnTrigger(Collider other) // if triggered by player, change level
        {
            PlayerMovement? player = other.gameEntity.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Raylib.PlaySound(enterSound);
                EntityManager.DestroyEntity(player.gameEntity);
                hasEnteredPortal = true;
            }
        }

    }
}
