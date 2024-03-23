using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Animation;
using Physics;
using UI;

namespace Engine
{
    public class GameManager : GameEntity
    {

        public GameManager()
        {
            name = "GameManager";

            Camera.zoom = 1.6f;
            UIText uIText = new()
            {
                Layer = 10,
                text = "Press space to start"
            };
            AddComponent<UIText>(uIText);

            AddComponent<GameManagerScript>(new GameManagerScript());
        }
    }
}