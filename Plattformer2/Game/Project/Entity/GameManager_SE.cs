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
                text = "Press space to start",
                pos = new(150, 350)
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

            AddComponent<GameManagerScript>(new GameManagerScript(startUIText, levelUIText, endUIText));
        }
    }
}