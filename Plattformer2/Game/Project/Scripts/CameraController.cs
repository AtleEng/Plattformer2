using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class CameraController : Component, IScript
    {
        public CameraController(Transform player)
        {
            this.player = player;
        }
        Transform player;
        Vector2 offset = new(0, 0);
        float smoothing = 5;
        public override void Update(float delta)
        {
            Camera.position.X = Raymath.Lerp(Camera.position.X, player.worldPosition.X + offset.X, delta * smoothing);
        }
    }
}