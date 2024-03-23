using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;
using UI;

namespace Engine
{
    public class GameManagerScript : Component, IScript
    {
        UIText uIText;
        bool hasStartedGame;
        public override void Start()
        {
            uIText = gameEntity.GetComponent<UIText>();

            //Prelaod textures
            Block block = new();
            JumpPad jumpPad = new();
            WalkEnemy walkEnemy = new();
            JumpingEnemy jumpingEnemy = new();
            RandomEnemy randomEnemy = new();
            FlyingEnemy flyingEnemy = new();
            Player player = new();
            Portal portal = new();
        }

        public override void Update(float delta)
        {
            if (!hasStartedGame)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Space))
                {
                    hasStartedGame = true;

                    uIText.text = "";
                    LoadingManager.Load(1);
                }
            }
            else
            {
                if (Raylib.IsKeyPressed(KeyboardKey.One))
                {
                    LoadingManager.Load(1);
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Two))
                {
                    LoadingManager.Load(2);
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Three))
                {
                    LoadingManager.Load(3);
                }
            }
        }

    }
}