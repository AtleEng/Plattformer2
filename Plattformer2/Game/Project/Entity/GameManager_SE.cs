using System.Numerics;
using UI;

namespace Engine
{
    public class GameManager : GameEntity
    {
        public GameManager()
        {
            name = "GameManager";

            Camera.zoom = 1.6f;

            UIText startUIText = new()
            {
                Layer = 10,
                text = "-Press space to start\n\n\n\n\n\n-Hold down ECS to quit",
                pos = new(150, 500)
            };
            TextBox startText = new(startUIText);
            EntityManager.SpawnEntity(startText, Vector2.Zero, Vector2.One, this.transform);

            UIText levelUIText = new()
            {
                Layer = 10,
                text = "Level",
                pos = new(10, 10)
            };
            TextBox levelText = new(levelUIText);
            EntityManager.SpawnEntity(levelText, Vector2.Zero, Vector2.One, this.transform);

            UIText timerUIText = new()
            {
                Layer = 10,
                text = "Time:",
                pos = new(950, 10)
            };
            TextBox timerText = new(timerUIText);
            EntityManager.SpawnEntity(timerText, Vector2.Zero, Vector2.One, this.transform);

            UIText tutorialUIText = new()
            {
                Layer = 10,
                text = "-Press a/d to move left/right\n\n\n-Press space to jump (hold for higher jumps)\n\n\n-Press ecs to exit to menu",
                pos = new(10, 650),
                fontSize = 40
            };
            TextBox tutorialText = new(tutorialUIText);
            EntityManager.SpawnEntity(tutorialText, Vector2.Zero, Vector2.One, this.transform);

            UIText endUIText = new()
            {
                Layer = 10,
                text = "You won!\n\n\nPress ECS to return to menu",
                pos = new(150, 350)
            };
            TextBox endText = new(endUIText)
            {
                isActive = false
            };
            EntityManager.SpawnEntity(endText, Vector2.Zero, Vector2.One, this.transform);

            AddComponent<GameManagerScript>(new GameManagerScript(startUIText, levelUIText, timerUIText, tutorialUIText, endUIText));
        }
    }
}