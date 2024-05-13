using System.Numerics;
using CoreEngine;
namespace Engine
{
    //The entity all other entitys are children to
    public class PlayScene : GameEntity
    {
        GameManager gameManager;
        public PlayScene(string folderPath)
        {
            gameManager = new GameManager();
            LoadingManager.prePath = folderPath;
        }
        public override void OnInnit()
        {
            name = "PlayScene";

            EntityManager.SpawnEntity(gameManager, Vector2.Zero);
        }
    }
}