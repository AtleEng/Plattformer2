using System.Numerics;
using Raylib_cs;

namespace Engine
{
    public class Sprite : Component, IRendable
    {
        public int Layer { get; set; }

        public Texture2D spriteSheet;
        public Vector2 spriteGrid = Vector2.One;
        int frameIndex;
        public int FrameIndex
        {
            get
            {
                return frameIndex;
            }
            set
            {
                if (value > spriteGrid.X * spriteGrid.Y - 1)
                {
                    frameIndex = 0;
                }
                else if (value < 0)
                {
                    frameIndex = (int)(spriteGrid.X * spriteGrid.Y - 1);
                }
                else
                {
                    frameIndex = value;
                }
            }
        }
        public Color colorTint = Color.White;

        public bool isFlipedY;
        public bool isFlipedX;

        public override string PrintStats()
        {
            return $"SpriteGrid: {spriteGrid} FrameIndex: {frameIndex} Layer: {Layer}";
        }

        public void Render()
        {
            Vector2 p = WorldSpace.ConvertToCameraPosition(gameEntity.transform.worldPosition);
            Vector2 s = WorldSpace.ConvertToCameraSize(gameEntity.transform.worldSize);

            Rectangle destRec = new Rectangle(
            (int)p.X - (int)(s.X / 2), (int)p.Y - (int)(s.Y / 2), //pos
            (int)s.X, (int)s.Y //size
            );

            //Raylib.DrawRectangleRec(destRec, new Color(255, 255, 255, 255));

            if (spriteSheet.Id != 0)
            {
                int flipX = isFlipedX ? -1 : 1;
                int flipY = isFlipedY ? -1 : 1;

                int i = FrameIndex;

                int x = (int)spriteGrid.X;
                int y = (int)spriteGrid.Y;

                float gridSizeX = spriteSheet.Width / x;
                float gridSizeY = spriteSheet.Height / y;

                int posX = i % x;
                int posY = i / x;

                Rectangle source = new Rectangle(
                    (int)(posX * gridSizeX),
                    (int)(posY * gridSizeY),
                    spriteSheet.Width * flipX / spriteGrid.X,
                spriteSheet.Height * flipY / spriteGrid.Y
                );

                Raylib.DrawTexturePro(spriteSheet, source, destRec, Vector2.Zero, 0, colorTint);
            }
        }
    }
    public interface IRendable
    {
        public int Layer { get; set; }
        public void Render();
    }
}