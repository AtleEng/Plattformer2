using System.Numerics;
using CoreEngine;
namespace Engine
{
    //The entity all other entitys are children to
    public class PlayScene : GameEntity
    {
        public override void OnInnit()
        {
            name = "PlayScene";

            EntityManager.SpawnEntity(new GameManager(), Vector2.Zero);
        }
    }
}