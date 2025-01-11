using Tanks.Gameplay.Components;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;
using Tanks.Gameplay.Animations;

namespace Tanks.Gameplay.Entity
{
    public class BaseTank : IBehaviour
    {
        private Vector2Int _currentDirectionMovement;

        private Animation<Vector2Int> _animations = new Animation<Vector2Int>();
        protected Vector2Int _directionInput;

        private Projectile _projectile;

        public BaseTank(Level level, Vector2Int currentPosition)
        {
            _isDie = false;
            _currentLevel = level;
            _currentPosition = currentPosition;
            _lastPosition = _currentPosition;

            _animations.Add(Vector2Int.up, new char[,] { { '@', '@', '@' }, { '^', 'O', '#' }, { '@', '@', '@' } });
            _animations.Add(Vector2Int.down, new char[,] { { '@', '@', '@' }, { '#', 'O', 'v' }, { '@', '@', '@' } });
            _animations.Add(Vector2Int.left, new char[,] { { '@', '<', '@' }, { '@', 'O', '@' }, { '@', '#', '@' } });
            _animations.Add(Vector2Int.right, new char[,] { { '@', '#', '@' }, { '@', 'O', '@' }, { '@', '>', '@' } });

            _currentSprite = _animations.GetSprite(Vector2Int.up);
            _currentDirectionMovement = Vector2Int.up;

            _currentLevel.EntityRenderUpdate(this);
        }

        public void Move(Vector2Int direction)
        {
            Vector2Int newPosition = direction + _currentPosition;

            if (direction != _currentDirectionMovement)
            {
                if (direction != Vector2Int.zero) 
                {
                    Rotation(direction);
                    _currentLevel.EntityRenderUpdate(this);
                }
                return;
            }

            if (_currentLevel.GetCell(_currentPosition + direction) != null)
            {
                if (_currentLevel.GetCell(_currentPosition + direction).Tag != "Projectile")
                {
                    return;
                }

            }
            _lastPosition = _currentPosition;
            _currentPosition = newPosition;

            _currentLevel.StepEntity(this);
            _currentLevel.EntityRenderUpdate(this);
        }

        private void Rotation(Vector2Int direction)
        {
            _currentSprite = _animations.GetSprite(direction);
            _currentDirectionMovement = direction;
        }

        public void Shoot()
        {
            _currentLevel.SpawnProjectile(new Projectile(_currentLevel, _currentPosition + _currentDirectionMovement, _currentDirectionMovement, 1));
        }

        public override void Update(float deltaTime)
        {

        }

        public override void TakeDamage(int damage)
        {

        }
    }
}
