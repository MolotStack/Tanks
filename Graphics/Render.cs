
using System.Drawing;
using Tanks.Gameplay.Components;
using Tanks.Gameplay.Input;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Graphics
{
    public class Render
    {

        private int _sizeSprite = 3;
        private Level _currentLevel;

        public void Start()
        {

            DrawnMap();
        }

        public void StartScene(Level currentLevel)
        {
            Console.CursorVisible = false;
            _currentLevel = currentLevel;

            Console.Clear();
            Console.SetCursorPosition(50, 30);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_currentLevel.Name);
        }
        public void GameOverScene()
        {
            Console.Clear();
            Console.SetCursorPosition(50, 30);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over!");
        }

        public void FinalScene()
        {
            Console.Clear();
            Console.SetCursorPosition(50, 30);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Thanks for playing!");
        }

        public void Update()
        {
            CurrentLevelChangedRender();
            InfoRender();
        }

        private void DrawnMap()
        {
            Console.Clear();

            IBehaviour[,] map = _currentLevel.GetMap().GetMap();

            Console.SetCursorPosition(0, 3);

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.SetCursorPosition(j * _sizeSprite, 3 + i * _sizeSprite);

                    if (map[i, j] != null)
                    {
                        for (int y = 0; y < map[i,j].CurrentSprite.GetLength(0); y++)
                        {
                            Console.SetCursorPosition(j * _sizeSprite, 3 + y + i * _sizeSprite);
                            for (int x = 0; x < map[i, j].CurrentSprite.GetLength(1); x++)
                            {
                                Console.ForegroundColor = map[i, j].Color;
                                Console.BackgroundColor = map[i, j].Background;
                                Console.Write(map[i, j].CurrentSprite[x,y]);
                            }
                        }
                    }
                }
            }
        }

        private void CurrentLevelChangedRender()
        {
            if (_currentLevel.ForRender.Count > 0)
            {
                foreach (var item in _currentLevel.ForRender)
                {
                    DrawObject(item);
                }
            }

            _currentLevel.ForRender.Clear();
        }

        private void EraseObject(Vector2Int position)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            for (int i = 0; i < _sizeSprite; i++)
            {
                Console.SetCursorPosition(position.x * _sizeSprite, 3 + position.y * _sizeSprite + i);
                for (int j = 0; j < _sizeSprite; j++)
                {
                    Console.Write(" ");
                }
            }
        }

        private void DrawObject(IBehaviour behaviour)
        {
            if (behaviour.IsDie)
            {
                EraseObject(behaviour.LastPosition);
                return;
            }

            if (behaviour.LastPosition != behaviour.CurrentPosition)
            {
                EraseObject(behaviour.LastPosition);
            }

            for (int i = 0; i < behaviour.CurrentSprite.GetLength(0); i++)
            {
                for (int j = 0; j < behaviour.CurrentSprite.GetLength(1); j++)
                {
                    Console.SetCursorPosition(behaviour.CurrentPosition.x * _sizeSprite + j, 3 + behaviour.CurrentPosition.y  * _sizeSprite + i);
                    Console.ForegroundColor = behaviour.Color;
                    Console.BackgroundColor = behaviour.Background;
                    Console.Write(behaviour.CurrentSprite[j, i]);
                }
            }
        }
        private void InfoRender()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.Write("                                    ");
            Console.SetCursorPosition(0, 0);
            Console.Write("Осталось противников: " + _currentLevel.CountEnemy);
        }
    }
}
