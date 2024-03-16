using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class GameManagerScript : Component, IScript
    {

        public override void Start()
        {
            //LoadingManager.SaveLevel("Level1", _level1);

            LoadingManager.Load(1);
        }

        public override void Update(float delta)
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