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
            float minX = WindowSettings.gameScreenWidth / (Camera.zoom * WorldSpace.pixelsPerUnit * 2);
            float minY = WindowSettings.gameScreenHeight / (Camera.zoom * WorldSpace.pixelsPerUnit * 2);

            float maxX = LoadingManager.LevelSize.X - 1 - WindowSettings.gameScreenWidth / (Camera.zoom * WorldSpace.pixelsPerUnit * 2);
            float maxY = LoadingManager.LevelSize.Y - 1 - WindowSettings.gameScreenHeight / (Camera.zoom * WorldSpace.pixelsPerUnit * 2);

            //System.Console.WriteLine(LoadingManager.LevelSize.X);
            System.Console.WriteLine($"{minX}, {maxX}");
            Camera.position.X = Raymath.Lerp(Camera.position.X, player.worldPosition.X + offset.X, delta * smoothing);
            Camera.position.Y = Raymath.Lerp(Camera.position.Y, player.worldPosition.Y + offset.Y, delta * smoothing);

            Camera.position.X = Math.Clamp(Camera.position.X, minX, maxX);
            Camera.position.Y = Math.Clamp(Camera.position.Y, minY, maxY);
        }
    }
}