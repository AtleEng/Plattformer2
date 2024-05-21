using System.Numerics;
using CoreEngine;
using UI;

namespace Engine
{
    public class LevelEditorScene : GameEntity
    {
        //Add all diffrent components
        public override void OnInnit()
        {
            name = "EditorScene";

            LevelEditorManager levelEditor = new LevelEditorManager();
            EntityManager.SpawnEntity(levelEditor);
        }
    }
}