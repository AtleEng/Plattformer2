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
        UIText levelText;
        UIText timerText;
        UIText tutorialText;

        GameState currentState = GameState.startPlay;

        float currentPlayTime;

        public GameManagerScript(UIText levelText, UIText timerText, UIText tutorialText)
        {
            this.levelText = levelText;
            this.timerText = timerText;
            this.tutorialText = tutorialText;
        }
        public override void Start()
        {
            //Starts the loading manager
            LoadingManager.StartLoading();

        }

        public override void Update(float delta)
        {
            //State handler
            if (currentState == GameState.startPlay)
            {
                ChangeLevel(1);
                currentState = GameState.startLevel;
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
                levelText.gameEntity.isActive = true;
                timerText.gameEntity.isActive = true;

                ChangeLevel(LoadingManager.CurrentLevel);
            }
            //Go back to MainMenu if ESC is pressed
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                Core.ChangeRotEntity(new StartScene());
            }
        }
        void WonLevel() //Didnt add anything here
        {

        }
        void WonGame() //When all levels are complited
        {
            Core.ChangeRotEntity(new StartScene());
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

        enum GameState //The diffrent gameStates, wonLevel and died could in the future be used to create effects when winning/losing
        {
            startPlay, startLevel, playing, wonLevel, died, wonGame
        }
    }
}