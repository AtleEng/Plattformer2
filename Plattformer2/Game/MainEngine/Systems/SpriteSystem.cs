using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;

using CoreEngine;
using Engine;
using Physics;

namespace Graphics
{
    public class SpriteSystem : GameSystem
    {
        public float scale;
        int offsetX;
        int offsetY;

        RenderTexture2D target = new();

        float gameScreenWidth;
        float gameScreenHeight;

        List<Sprite> allSprites = new();

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
                Sprite? spriteComponent = gameEntity.GetComponent<Sprite>();
                if (spriteComponent != null) { allSprites.Add(spriteComponent); }
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

            foreach (Sprite sprite in allSprites)
            {
                Vector2 p = WorldSpace.ConvertToCameraPosition(sprite.gameEntity.transform.worldPosition);
                Vector2 s = WorldSpace.ConvertToCameraSize(sprite.gameEntity.transform.worldSize);

                Rectangle destRec = new Rectangle(
                (int)p.X - (int)(s.X / 2), (int)p.Y - (int)(s.Y / 2), //pos
                (int)s.X, (int)s.Y //size
                );

                //Raylib.DrawRectangleRec(destRec, new Color(255, 255, 255, 255));

                if (sprite.spriteSheet.Id != 0)
                {
                    int flipX = sprite.isFlipedX ? -1 : 1;
                    int flipY = sprite.isFlipedY ? -1 : 1;

                    int i = sprite.FrameIndex;

                    int x = (int)sprite.spriteGrid.X;
                    int y = (int)sprite.spriteGrid.Y;

                    float gridSizeX = sprite.spriteSheet.Width / x;
                    float gridSizeY = sprite.spriteSheet.Height / y;

                    int posX = i % x;
                    int posY = i / x;

                    Rectangle source = new Rectangle(
                        (int)(posX * gridSizeX),
                        (int)(posY * gridSizeY),
                        sprite.spriteSheet.Width * flipX / sprite.spriteGrid.X,
                    sprite.spriteSheet.Height * flipY / sprite.spriteGrid.Y
                    );

                    Raylib.DrawTexturePro(sprite.spriteSheet, source, destRec, Vector2.Zero, 0, sprite.colorTint);
                }
                //Raylib.DrawCircle((int)p.X, (int)p.Y, 5, Color.Red);
            }
            //DisplayGrid();
            //Raylib.DrawText($"GameEntitys:{Core.gameEntities.Count}\nFPS:{Raylib.GetFPS()}", 20, 20, 20, Color.RayWhite);
            allSprites.Clear();
        }
        void AddSprite(Sprite sprite)
        {
            allSprites.Add(sprite);
            allSprites.Sort((a, b) => a.layer.CompareTo(b.layer));
        }
        void RemoveSprite(Sprite sprite)
        {
            allSprites.Remove(sprite);
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

