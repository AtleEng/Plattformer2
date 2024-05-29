using System.Numerics;
using CoreEngine;
using UI;

namespace Engine
{
    public class LevelEditorManager : GameEntity
    {
        //Add all diffrent components
        public LevelEditorManager()
        {
            name = "LevelEditorManager";

            TextBox textBox = new(new UIText()
            {
                fontSize = 20
            });
            EntityManager.SpawnEntity(textBox, new Vector2(5, 55), Vector2.One, this.transform);

            LevelEditorScript levelMenuScript = new()
            {
                pathText = textBox.uIText
            };
            AddComponent<LevelEditorScript>(levelMenuScript);
        }
    }
}