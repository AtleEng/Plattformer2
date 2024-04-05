using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    [Serializable]
    public class PortalScript : Component, IScript
    {
        Animator? anim;

        public override void Start()
        {
            anim = gameEntity.GetComponent<Animator>();
            if (anim != null)
            {
                anim.PlayAnimation("Idle");
            }
        }

        public override void OnTrigger(Collider other)
        {
            PlayerMovement? player = other.gameEntity.GetComponent<PlayerMovement>();
            if (player != null)
            {
                EntityManager.DestroyEntity(player.gameEntity);
                LoadingManager.Load(LoadingManager.CurrentLevel + 1);
            }
        }

    }
}
