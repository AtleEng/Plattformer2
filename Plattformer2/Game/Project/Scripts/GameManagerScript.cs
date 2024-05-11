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
            //Starts the loading manager
            LoadingManager.StartLoading();
        }

        public override void Update(float delta)
        {
            //State handler
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
            //Set which texts should be displayed
            startText.gameEntity.isActive = true;
            levelText.gameEntity.isActive = false;
            timerText.gameEntity.isActive = false;
            tutorialText.gameEntity.isActive = false;
            endText.gameEntity.isActive = false;
            //If space is pressed then start the game
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                currentState = GameState.startLevel;

                startText.gameEntity.isActive = false;
                levelText.gameEntity.isActive = true;
                timerText.gameEntity.isActive = true;
                endText.gameEntity.isActive = false;

                ChangeLevel(1);
            }
            if (Raylib.IsKeyDown(KeyboardKey.Escape)) //If escaped is holded down quit
            {
                quitTimer += delta;
                if (quitTimer >= buttonQuitTime)
                {
                    Core.shouldClose = true;
                }
            }
            else //restet the timer
            {
                quitTimer = 0;
            }
        }
        void StartLevel()
        {
            //Display the current level number
            levelText.text = $"Level {LoadingManager.CurrentLevel}";
            //Only show instructions at first level
            if (LoadingManager.CurrentLevel == 1)
            {
                tutorialText.gameEntity.isActive = true;
            }
            else
            {
                tutorialText.gameEntity.isActive = false;
            }
            //Change state to playing
            currentState = GameState.playing;
        }
        void Playing(float delta)
        {
            //Timer
            currentPlayTime += delta;
            string s = currentPlayTime.ToString("0.00");
            timerText.text = $"{s}";
            //Restart level if R is pressed
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                currentState = GameState.startLevel;
                startText.gameEntity.isActive = false;
                levelText.gameEntity.isActive = true;
                timerText.gameEntity.isActive = true;

                ChangeLevel(LoadingManager.CurrentLevel);
            }
            //Go back to MainMenu if ESC is pressed
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                ChangeLevel(0);
                currentState = GameState.startMenu;
            }
        }
        void WonLevel() //Didnt add anything here
        {

        }
        void WonGame() //When all levels are complited
        {
            startText.gameEntity.isActive = false;
            levelText.gameEntity.isActive = false;
            endText.gameEntity.isActive = true;
            //press escape to go to MainMenu
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                currentState = GameState.startMenu;
            }
        }
        void Died() //Didnt add anything here, you just respawn
        {

        }

        public void ChangeLevel(int i) //Changes level and get the correct state
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

        public void StartGame()
        {
            currentState = GameState.startLevel;

            startText.gameEntity.isActive = false;
            levelText.gameEntity.isActive = true;
            timerText.gameEntity.isActive = true;
            endText.gameEntity.isActive = false;

            ChangeLevel(1);
        }
        public void QuitGame()
        {
            Core.shouldClose = true;
        }
        enum GameState //The diffrent gameStates, wonLevel and died could in the future be used to create effects when winning/losing
        {
            startMenu, startLevel, playing, wonLevel, wonGame, died
        }
    }
}