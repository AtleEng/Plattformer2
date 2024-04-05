using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;
using UI;

namespace Engine
{
    public class GameManagerScript : Component, IScript
    {
        UIText startText;
        UIText levelText;

        GameState currentState = GameState.startMenu;

        public GameManagerScript(UIText startText, UIText levelText)
        {
            this.startText = startText;
            this.levelText = levelText;
        }
        public override void Start()
        {

        }

        public override void Update(float delta)
        {
            if (currentState == GameState.startMenu)
            {
                StartMenu();
            }
            else if (currentState == GameState.startLevel)
            {
                StartLevel();
            }
            else if (currentState == GameState.playing)
            {
                Playing();
            }
            else if (currentState == GameState.wonLevel)
            {
                WonLevel();
            }
            else if (currentState == GameState.wonGame)
            {
                WonGame();
            }
            else
            {
                Died();
            }
        }

        void StartMenu()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                currentState = GameState.startLevel;
                EntityManager.DestroyEntity(startText.gameEntity);
                levelText.gameEntity.isActive = true;

                ChangeLevel(1);
            }
        }
        void StartLevel()
        {
            levelText.text = $"Level {LoadingManager.CurrentLevel}";

            currentState = GameState.playing;
        }
        void Playing()
        {

        }
        void WonLevel()
        {

        }
        void WonGame()
        {

        }
        void Died()
        {

        }

        void ChangeLevel(int i)
        {
            LoadingManager.Load(i);
        }

        enum GameState
        {
            startMenu, startLevel, playing, wonLevel, wonGame, died
        }
    }
}