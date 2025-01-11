
using Tanks.Gameplay.Input;
using Tanks.Gameplay.Levels;
using Tanks.Graphics;

namespace Tanks.Gameplay
{
    public class Game
    {
        private Level1 _level1;

        private List<Level> _levels = new List<Level>();

        private Level _currentLevel;

        private int _currentLevelIndex;

        private InputHandler _inputHandler;
        private Render _render;
        public Game()
        {
            _inputHandler = new InputHandler();
            _render = new Render();

            _levels = new List<Level>();

            _levels.Add(_level1 = new Level1(new Utilities.Vector2Int(30, 15), 54544545, 10));
        }

        public void Run()
        {
            _currentLevel = _level1;
            SetCurrentLevel(0);

            StartLevel();
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

            _render.DrawnMap(_currentLevel);
        }
    }
}
