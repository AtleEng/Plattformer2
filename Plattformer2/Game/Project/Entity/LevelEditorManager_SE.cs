using System.Numerics;
using CoreEngine;

namespace Engine
{
    public class LevelEditorManager : GameEntity
    {
        //Add all diffrent components
        public LevelEditorManager()
        {
            name = "LevelEditorManager";
            LevelEditorScript levelMenuScript = new();
            AddComponent<LevelEditorScript>(levelMenuScript);
        }
    }
}