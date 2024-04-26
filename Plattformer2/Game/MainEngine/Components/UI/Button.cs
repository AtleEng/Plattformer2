using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace UI
{
    //this is not used so I wont explain it
    public class Button : Component, IScript
    {
        Action OnKilcked;
        public bool isHovering;
        public Button(Action KlickAction)
        {
            OnKilcked = KlickAction;
        }
        public override void Update(float delta)
        {
            Vector2 mPos = WorldSpace.GetGameMousePos();
            if (Raylib.CheckCollisionPointRec
                (mPos, new Rectangle(gameEntity.transform.worldPosition.X - gameEntity.transform.worldSize.X / 2,
                gameEntity.transform.worldPosition.Y - gameEntity.transform.worldSize.Y / 2,
                gameEntity.transform.worldSize.X, gameEntity.transform.worldSize.Y)))
            {
                isHovering = true;
                if (Raylib.IsMouseButtonDown(0))
                {
                    OnKilcked();
                }
            }
            else { isHovering = false; }
        }
    }
}