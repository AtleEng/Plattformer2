using System.Numerics;
using CoreEngine;
using UI;

namespace Engine
{
    public class StartScene : GameEntity
    {
        //Add all diffrent components
        public override void OnInnit()
        {
            name = "StartScene";

            Dictionary<string, Action> buttonDictionary = new()
            {
                { "Play", () => Core.ChangeRotEntity(new PlayScene(@"Game\Project\Levels\Main\")) },
                { "Levels", () => Core.ChangeRotEntity(new LevelMenuScene()) },
                { "Editor", () => Core.ChangeRotEntity(new LevelEditorScene()) },
                { "Back", () => Core.QuitGame() }
            };

            int i = 0;
            foreach (var keyValuePair in buttonDictionary)
            {
                ButtonObject button = new ButtonObject(keyValuePair.Value);
                button.uIText.text = keyValuePair.Key;
                EntityManager.SpawnEntity(button, new Vector2(175, (125 * i) + 75), new Vector2(300, 100), transform);
                i++;
            }
        }
    }
}
