using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace UI
{
    public class UIText : Component, IRendable
    {
        public int Layer { get; set; }

        public string text = "Enter text here:";
        public int fontSize = 50;
        public Color color = Color.White;
        public Vector2 pos = new();
        public void Render()
        {
            Raylib.DrawText(text, (int)pos.X, (int)pos.Y, fontSize, color);
        }
    }
}