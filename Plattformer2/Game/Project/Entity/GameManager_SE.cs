using System.Numerics;
using UI;

namespace Engine
{
    public class GameManager : GameEntity
    {
        //Add all diffrent components
        public GameManager()
        {
            name = "GameManager";

            Camera.zoom = 1.6f;

            UIText levelUIText = new()
            {
                Layer = 10,
                text = "Level",
            };
            TextBox levelText = new(levelUIText);
            EntityManager.SpawnEntity(levelText, new(10, 10), Vector2.One, this.transform);

            UIText timerUIText = new()
            {
                Layer = 10,
                text = "Time:",
            };
            TextBox timerText = new(timerUIText);
            EntityManager.SpawnEntity(timerText, new(950, 10), Vector2.One, this.transform);

            UIText tutorialUIText = new()
            {
                Layer = 10,
                text = "-Press a/d to move left/right\n\n\n-Press space to jump (hold for higher jumps)\n\n\n-Press ecs to exit to menu",
                fontSize = 40
            };
            TextBox tutorialText = new(tutorialUIText);
            EntityManager.SpawnEntity(tutorialText, new(10, 650), Vector2.One, this.transform);

            GameManagerScript gM = new GameManagerScript(levelUIText, timerUIText, tutorialUIText);
            AddComponent<GameManagerScript>(gM);
        }
    }
}