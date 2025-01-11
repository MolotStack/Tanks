using Tanks.Gameplay.Components;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels.Environment
{
    public class DestructibleWall : IBehaviour
    {
        private char[,] _fullSprite => new char[,] { { '#', '#', '#' }, { '#', '#', '#' }, { '#', '#', '#' } };
        private char[,] _damageSprite => new char[,] { { '/', '/', '/' }, { '/', '/', '/' }, { '/', '/', '/' } };
        private int _health = 2;

        public DestructibleWall(Vector2Int position, Level currentLevel)
        {
            _tag = "DestructibleWall1";
            _isDie = false;

            _currentSprite = _fullSprite;

            _color = ConsoleColor.DarkBlue;
            _background = ConsoleColor.DarkGray;

            _currentPosition = position;
            _lastPosition = _currentPosition;
            _currentLevel = currentLevel;
        }

        public override void TakeDamage(int damage)
        {
            if (!_isDie)
            {
                _health -= damage;
                if (_health <= 0)
                {
                    Die();
                }
                _currentSprite = _damageSprite;
            } else 
            {
                Die();
            }
            _currentLevel.EntityRenderUpdate(this);
        }
        private void Die()
        {
            _isDie = true;
            _currentLevel.RemoveEntityMap(_currentPosition);
            _currentLevel.EntityRenderUpdate(this);
        }
    }
}
