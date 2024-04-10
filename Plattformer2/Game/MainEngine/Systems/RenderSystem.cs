using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

using CoreEngine;
using Engine;
using Physics;

namespace Graphics
{
    public class RenderSystem : GameSystem
    {
        public float scale;
        int offsetX;
        int offsetY;

        RenderTexture2D target = new();

        float gameScreenWidth;
        float gameScreenHeight;

        List<IRendable> allRenderObjects = new();

        public override void Start()
        {
            System.Console.WriteLine("Innit window");
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.SetConfigFlags(ConfigFlags.VSyncHint);

            Raylib.InitWindow(WindowSettings.startWindowWidth, WindowSettings.startWindowHeight, "Game Window");
            Raylib.SetWindowMinSize(400, 300);

            Raylib.SetTargetFPS(60);

            Raylib.SetExitKey(KeyboardKey.Null);

            target = Raylib.LoadRenderTexture(WindowSettings.gameScreenWidth, WindowSettings.gameScreenHeight);
            SetValuesOfWindow();

        }
        public override void Update(float delta)
        {
            if (Raylib.IsWindowResized())
            {
                SetValuesOfWindow();
            }

            Raylib.BeginTextureMode(target);
            Raylib.ClearBackground(new Color(41, 189, 193, 255));
            //Render all sprites
            RenderAll();

            Raylib.EndTextureMode();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(new Color(69, 43, 63, 255));

            Raylib.DrawTexturePro(target.Texture,
    new Rectangle(0.0f, 0.0f, (float)target.Texture.Width, (float)-target.Texture.Height),
    new Rectangle(offsetX, offsetY, gameScreenWidth * scale, gameScreenHeight * scale),
    new Vector2(0, 0), 0.0f, Color.White);

            Raylib.EndDrawing();

            if (Core.shouldClose)
            {
                Raylib.UnloadRenderTexture(target);
                Raylib.CloseWindow();
            }

            if (Raylib.WindowShouldClose())
            {
                Core.shouldClose = true;
            }
        }
        void RenderAll()
        {
            foreach (GameEntity gameEntity in Core.activeGameEntities)
            {
                IRendable? rB = gameEntity.GetComponentInterface<IRendable>();
                if (rB != null) { allRenderObjects.Add(rB); }
                /*
                                Collider? collider = gameEntity.GetComponent<Collider>();
                                if (collider != null)
                                {
                                    Vector2 p = WorldSpace.ConvertToCameraPosition(gameEntity.transform.worldPosition + collider.offset);
                                    Vector2 s = WorldSpace.ConvertToCameraSize(gameEntity.transform.worldSize * collider.scale);

                                    Rectangle colliderBox = new Rectangle(
                                    (int)p.X - (int)(s.X / 2), (int)p.Y - (int)(s.Y / 2), //pos
                                    (int)s.X, (int)s.Y //size
                                    );
                                    Color color = new Color(55, 255, 55, 250);
                                    if (collider.isTrigger)
                                    {
                                        color = new Color(55, 55, 255, 250);
                                        if (collider.isColliding)
                                        {
                                            color = new Color(255, 55, 255, 250);
                                        }
                                    }
                                    else if (collider.isColliding) { color = new Color(255, 55, 55, 250); }

                                    Raylib.DrawRectangleRec(colliderBox, color);
                                }
                            */
            }
            allRenderObjects.Sort((a, b) => a.Layer.CompareTo(b.Layer));
            foreach (IRendable rB in allRenderObjects)
            {
                rB.Render();
            }
            allRenderObjects.Clear();
        }
        public void AddRenderObject(IRendable rB)
        {
            allRenderObjects.Add(rB);
            //allRenderObjects.Sort((a, b) => a.layer.CompareTo(b.layer));
        }
        public void RemoveSprite(IRendable rB)
        {
            allRenderObjects.Remove(rB);
        }
        void SetValuesOfWindow()
        {
            gameScreenWidth = WindowSettings.gameScreenWidth;
            gameScreenHeight = WindowSettings.gameScreenHeight;

            float screenAspectRatio = (float)Raylib.GetScreenWidth() / Raylib.GetScreenHeight();
            float gameAspectRatio = gameScreenWidth / gameScreenHeight;

            if (screenAspectRatio > gameAspectRatio)
            {
                // Window is wider than the game screen
                scale = Raylib.GetScreenHeight() / gameScreenHeight;
                offsetX = (int)((Raylib.GetScreenWidth() - (gameScreenWidth * scale)) * 0.5f);
                offsetY = 0;
            }
            else
            {
                // Window is taller than the game screen
                scale = (float)Raylib.GetScreenWidth() / gameScreenWidth;
                offsetX = 0;
                offsetY = (int)((Raylib.GetScreenHeight() - (gameScreenHeight * scale)) * 0.5f);
            }
        }

        void DisplayGrid()
        {
            int gridSize = 100;
            Vector2 spX = WorldSpace.ConvertToCameraPosition(new Vector2(gridSize, 0));

            Vector2 epX = WorldSpace.ConvertToCameraPosition(new Vector2(-gridSize, 0));
            Raylib.DrawLine((int)spX.X, (int)spX.Y, (int)epX.X, (int)epX.Y, Color.Red);

            Vector2 spY = WorldSpace.ConvertToCameraPosition(new Vector2(0, gridSize));

            Vector2 epY = WorldSpace.ConvertToCameraPosition(new Vector2(0, -gridSize));
            Raylib.DrawLine((int)spY.X, (int)spY.Y, (int)epY.X, (int)epY.Y, Color.Blue);

            for (int x = -gridSize; x < gridSize; x++)
            {
                Vector2 sp = WorldSpace.ConvertToCameraPosition(new Vector2(x + 0.5f, gridSize + 0.5f));

                Vector2 ep = WorldSpace.ConvertToCameraPosition(new Vector2(x + 0.5f, -gridSize - 0.5f));
                Raylib.DrawLine((int)sp.X, (int)sp.Y, (int)ep.X, (int)ep.Y, Color.RayWhite);
            }
            for (int y = -gridSize; y < gridSize; y++)
            {
                Vector2 sp = WorldSpace.ConvertToCameraPosition(new Vector2(gridSize + 0.5f, y + 0.5f));

                Vector2 ep = WorldSpace.ConvertToCameraPosition(new Vector2(-gridSize - 0.5f, y + 0.5f));
                Raylib.DrawLine((int)sp.X, (int)sp.Y, (int)ep.X, (int)ep.Y, Color.RayWhite);
            }
        }
    }
}

