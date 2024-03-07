using System.Numerics;
using Raylib_cs;

namespace Engine
{
    [Serializable]
    public class Sprite : Component
    {
        public Sprite() { }

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
        public int layer;
        public bool isFlipedY;
        public bool isFlipedX;

        public override string PrintStats()
        {
            return $"SpriteGrid: {spriteGrid} FrameIndex: {frameIndex} Layer: {layer}";
        }
    }
}