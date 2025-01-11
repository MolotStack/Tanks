
using Tanks.Utilities;

namespace Tanks.Gameplay.Input
{
    public class InputHandler
    {
        public Vector2Int Direction => _currentDirection;
        public static InputHandler Instance { get; private set; }

        public Action CloseGame;
        public Action<Vector2Int> OnChangedDirection;
        public Action OnShoot;

        public InputHandler()
        {
            Instance = this;
        }

        private Vector2Int _currentDirection = Vector2Int.zero;

        public void InputUpdate()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow or ConsoleKey.W: OnChangedDirection?.Invoke(_currentDirection = Vector2Int.up); break;
                    case ConsoleKey.DownArrow or ConsoleKey.S: OnChangedDirection?.Invoke(_currentDirection = Vector2Int.down); break;
                    case ConsoleKey.LeftArrow or ConsoleKey.A: OnChangedDirection?.Invoke(_currentDirection = Vector2Int.left); break;
                    case ConsoleKey.RightArrow or ConsoleKey.D: OnChangedDirection?.Invoke(_currentDirection = Vector2Int.right); break;
                    case ConsoleKey.Spacebar: OnShoot?.Invoke(); break;
                    case ConsoleKey.Escape:
                        CloseGame?.Invoke();
                        break;

                }
            }
            else
            {
                if (_currentDirection != Vector2Int.zero)
                {
                    OnChangedDirection?.Invoke(_currentDirection = Vector2Int.zero);
                }
            }

        }

        public void SetDefaulDirection()
        {
            _currentDirection = Vector2Int.zero;
        }
    }
}
