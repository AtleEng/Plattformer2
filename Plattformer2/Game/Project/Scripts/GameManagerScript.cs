using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;
using UI;
using CoreEngine;

namespace Engine
{
    public class GameManagerScript : Component, IScript
    {
        UIText startText;
        UIText levelText;
        UIText timerText;
        UIText tutorialText;
        UIText endText;

        GameState currentState = GameState.startMenu;

        float currentPlayTime;

        float buttonQuitTime = 1f;
        float quitTimer;

        public GameManagerScript(UIText startText, UIText levelText, UIText timerText, UIText tutorialText, UIText endText)
        {
            this.startText = startText;
            this.levelText = levelText;
            this.timerText = timerText;
            this.tutorialText = tutorialText;
            this.endText = endText;
        }
        public override void Start()
        {
            LoadingManager.StartLoading();
        }

        public override void Update(float delta)
        {
            if (currentState == GameState.startMenu)
            {
                StartMenu(delta);
            }
            else if (currentState == GameState.startLevel)
            {
                StartLevel();
            }
            else if (currentState == GameState.playing)
            {
                Playing(delta);
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

        void StartMenu(float delta)
        {
            startText.gameEntity.isActive = true;
            levelText.gameEntity.isActive = false;
            timerText.gameEntity.isActive = false;
            tutorialText.gameEntity.isActive = false;
            endText.gameEntity.isActive = false;
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                currentState = GameState.startLevel;

                startText.gameEntity.isActive = false;
                levelText.gameEntity.isActive = true;
                timerText.gameEntity.isActive = true;
                endText.gameEntity.isActive = false;

                ChangeLevel(1);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Escape))
            {
                quitTimer += delta;
                if (quitTimer >= buttonQuitTime)
                {
                    Core.shouldClose = true;
                }
            }
        }
        void StartLevel()
        {
            levelText.text = $"Level {LoadingManager.CurrentLevel}";
            if (LoadingManager.CurrentLevel == 1)
            {
                tutorialText.gameEntity.isActive = true;
            }
            else
            {
                tutorialText.gameEntity.isActive = false;
            }

            currentState = GameState.playing;
        }
        void Playing(float delta)
        {
            currentPlayTime += delta;
            string s = currentPlayTime.ToString("0.00");
            timerText.text = $"{s}";
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                currentState = GameState.startLevel;
                startText.gameEntity.isActive = false;
                levelText.gameEntity.isActive = true;
                timerText.gameEntity.isActive = true;

                ChangeLevel(LoadingManager.CurrentLevel);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                ChangeLevel(0);
                currentState = GameState.startMenu;
            }
        }
        void WonLevel()
        {

        }
        void WonGame()
        {
            startText.gameEntity.isActive = false;
            levelText.gameEntity.isActive = false;
            endText.gameEntity.isActive = true;
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                currentState = GameState.startMenu;
            }
        }
        void Died()
        {

        }

        public void ChangeLevel(int i)
        {
            LoadingManager.Load(i);
            if (i <= LoadingManager.levels.Count)
            {
                currentState = GameState.startLevel;
            }
            else
            {
                currentState = GameState.wonGame;
            }
        }

        enum GameState
        {
            startMenu, startLevel, playing, wonLevel, wonGame, died
        }
    }
}