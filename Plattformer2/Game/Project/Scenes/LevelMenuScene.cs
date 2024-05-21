using System.Numerics;
using CoreEngine;
using UI;

namespace Engine
{
    public class LevelMenuScene : GameEntity
    {
        //Add all diffrent components
        public override void OnInnit()
        {
            name = "EditorScene";

            LevelMenuManager levelEditor = new LevelMenuManager();
            EntityManager.SpawnEntity(levelEditor);
        }
    }
}