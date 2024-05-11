using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CoreEngine;
using Engine;

namespace UI
{
    //Component used to display Text on the UI
    public class UIText : Component, IRendable
    {
        //Controll which layer the text is being redered at (lower layer behind higher layer)
        public int Layer { get; set; } = 10;
        //the diplayed texts text
        public string text = "Enter text here:";
        //size of text
        public int fontSize = 50;
        //color of text
        public Color color = Color.White;
        //Posistion realative to entity
        public Vector2 pos = new();
        public void Render()//What the sprite render call
        {
            Raylib.DrawText(text, (int)pos.X, (int)pos.Y, fontSize, color);
        }
    }
}