using System.Numerics;
using CoreEngine;
using UI;

namespace Engine
{
    public class EditorScene : GameEntity
    {
        //Add all diffrent components
        public override void OnInnit()
        {
            name = "EditorScene";

            LevelMenuManager levelEditor= new LevelMenuManager();
            EntityManager.SpawnEntity(levelEditor);

            Action editorAction = () => Core.ChangeRotEntity(new StartScene());
            ButtonObject editorButton = new ButtonObject(editorAction);
            editorButton.uIText.text = "Back";
            EntityManager.SpawnEntity(editorButton, new Vector2(175, 725), new Vector2(300, 100), transform);
        }
    }
}