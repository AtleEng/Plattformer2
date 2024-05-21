using System.Numerics;
using CoreEngine;

namespace Engine
{
    public class LevelMenuManager : GameEntity
    {
        //Add all diffrent components
        public LevelMenuManager()
        {
            name = "LevelMenuManager";
            LevelMenuScript levelMenuScript = new();
            AddComponent<LevelMenuScript>(levelMenuScript);
        }
    }
}