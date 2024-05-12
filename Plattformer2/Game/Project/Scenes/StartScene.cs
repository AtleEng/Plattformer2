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

            Action startAction = () => Core.ChangeRotEntity(new PlayScene());
            ButtonObject startButton = new ButtonObject(startAction);
            startButton.uIText.text = "Play";
            EntityManager.SpawnEntity(startButton, new Vector2(175, 475), new Vector2(300, 100), transform);

            Action editorAction = () => Core.ChangeRotEntity(new EditorScene());
            ButtonObject editorButton = new ButtonObject(editorAction);
            editorButton.uIText.text = "Levels";
            EntityManager.SpawnEntity(editorButton, new Vector2(175, 600), new Vector2(300, 100), transform);

            Action quitAction = () => Core.QuitGame();
            ButtonObject quitButton = new ButtonObject(quitAction);
            quitButton.uIText.text = "Exit";
            EntityManager.SpawnEntity(quitButton, new Vector2(175, 725), new Vector2(300, 100), transform);
        }
    }
}
