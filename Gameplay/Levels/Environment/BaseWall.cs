using Tanks.Gameplay.Components;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels
{
    public class BaseWall : IBehaviour
    {
        public BaseWall(Vector2Int position, Level currentLevel)
        {
            _currentLevel = currentLevel;

            _tag = "Wall";
            _isDie = false;
            _currentSprite = new char[,] { { '#', '#', '#' }, { '#', '#', '#' }, { '#', '#', '#' } };

            _color = ConsoleColor.Blue;
            _background = ConsoleColor.Black;

            _currentPosition = position;
            _lastPosition = _currentPosition;
        }
    }
}
