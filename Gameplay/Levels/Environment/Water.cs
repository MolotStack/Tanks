
using Tanks.Gameplay.Components;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels.Environment
{
    public class Water : IBehaviour
    {

        public Water(Vector2Int position, Level currentLevel)
        {
            _currentLevel = currentLevel;

            _tag = "Water";
            _isDie = false;
            _currentSprite = new char[,] { { '#', '#', '#' }, { '#', '#', '#' }, { '#', '#', '#' } };

            _color = ConsoleColor.Blue;
            _background = ConsoleColor.Blue;

            _currentPosition = position;
            _lastPosition = _currentPosition;

        }
    }
}
