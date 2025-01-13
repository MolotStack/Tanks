
using Tanks.Gameplay.Input;
using Tanks.Gameplay.Levels;
using Tanks.Graphics;

namespace Tanks.Gameplay
{
    public class Game
    {
        private Level _level1;
        private Level _level2;
        private Level _level3;

        private List<Level> _levels = new List<Level>();

        private Level _currentLevel;

        private int _currentLevelIndex;

        private InputHandler _inputHandler;
        private Render _render;
        public Game()
        {
            _inputHandler = new InputHandler();
            _render = new Render();
        }

        public void Run()
        {
            _inputHandler.CloseGame += CloseGame;

            LoadScene();

            _currentLevel = _level1;
            SetCurrentLevel(0);

            StartLevel();
        }

        public void CloseGame()
        {
            _inputHandler.CloseGame -= CloseGame;
            Environment.Exit(0);
        }

        private void GameLoop()
        {
            var lastTime = DateTime.Now;
            while (true)
            {
                var startTime = DateTime.Now;
                float deltaTime = (float)(startTime - lastTime).TotalSeconds;

                lastTime = startTime;

                Input();
                UpdateLogic(deltaTime);
                Render();

                Thread.Sleep(33);
            }
        }

        private void StartLevel()
        {
            GameLoop();
        }

        private void Input()
        {
            _inputHandler.InputUpdate();
        }
        private void UpdateLogic(float deltaTime)
        {
            _currentLevel.Update(deltaTime);
        }

        private void Render()
        {
            _render.Update();
        }

        private void SetCurrentLevel(int levelIndex)
        {
            if (_levels.Count <= 0 || _levels.Count - 1 < levelIndex)
            {
                return;
            }
            _currentLevel = _levels[levelIndex];
            _currentLevelIndex = levelIndex;

            _currentLevel.Victory += NextLevel;
            _currentLevel.Defeat += GameOver;

            _render.StartScene(_currentLevel);
            Thread.Sleep(1500);
            _render.Start();
        }

        private void NextLevel()
        {
            if (_levels.Count - 1 > _currentLevelIndex)
            {
                _currentLevelIndex += 1;

                SetCurrentLevel(_currentLevelIndex);
            }
            else
            {
                _render.FinalScene();
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
        }

        private void GameOver()
        {
            _render.GameOverScene();
            LoadScene();

            _currentLevel.Victory -= NextLevel;
            _currentLevel.Defeat -= GameOver;

            Thread.Sleep(1500);
            SetCurrentLevel(0);
        }

        private void LoadScene()
        {
            _levels = new List<Level>();

            _levels.Add(_level1 = new Level("Level 1", new Utilities.Vector2Int(30, 20), DateTime.Now.Microsecond, 10, 20, 1));
            _levels.Add(_level2 = new Level("Level 2", new Utilities.Vector2Int(30, 20), DateTime.Now.Microsecond, 12, 25, 3));
            _levels.Add(_level3 = new Level("Level 3", new Utilities.Vector2Int(30, 20), DateTime.Now.Microsecond, 12, 25, 5));
        }
    }
}
