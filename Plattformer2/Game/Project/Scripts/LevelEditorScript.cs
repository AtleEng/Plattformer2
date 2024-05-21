using System.Numerics;
using CoreEngine;

namespace Engine
{
    //Class that handle loading in levels
    public class LevelEditorScript : Component
    {
        List<ButtonObject> optionsButtons = new();
        List<ButtonObject> folderButtons = new();
        List<ButtonObject> levelButtons = new();

        override public void Start() //Creates the "Folder" entity
        {
            SpawnOptionsButtons();
        }
        void SpawnOptionsButtons()
        {
            Dictionary<string, Action> buttonDictionary = new()
            {
                { "Back", () => Core.ChangeRotEntity(new StartScene()) },
                { "Open", () => OpenDialog.GetFile() },
                { "Play", () => Core.ChangeRotEntity(new StartScene()) }
            };

            int i = 0;
            foreach (var keyValuePair in buttonDictionary)
            {
                ButtonObject button = new ButtonObject(keyValuePair.Value);
                button.uIText.text = keyValuePair.Key;
                EntityManager.SpawnEntity(button, new Vector2(75 + (150 * i), 25), new Vector2(150, 50), gameEntity.transform);
                i++;
            }
        }
    }
}