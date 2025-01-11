
using Tanks.Gameplay.Components;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Gameplay.Entity
{
    public class Projectile : IBehaviour
    {
        private Level _currentLevel;

        private Vector2Int _direction;
        private float _speed = 0.045f;
        private float _timeToMove;
        private int _damege = 1;

        public Projectile(Level currentLevel,Vector2Int position, Vector2Int direction, int damage)
        {
            _tag = "Projectile";
            _isDie = false;

            _currentPosition = position;
            _lastPosition = _currentPosition;
            _direction = direction;

            _currentLevel = currentLevel;

            _currentSprite = new char[,] { { ' ', '0', ' ' }, { '0', '0', '0' }, { ' ', '0', ' ' } };
            _color = ConsoleColor.DarkRed;
            _background = ConsoleColor.Black;


            if (_currentLevel.GetCell(_currentPosition) != null && _currentLevel.GetCell(_currentPosition).Tag != "Projectile") 
            {
                DetectedCollision(_currentLevel.GetMap()[position.y, position.x]);
            }

            if (!_isDie)
            {
                _currentLevel.AddEntityMap(this);
                _currentLevel.EntityRenderUpdate(this);
            }
        }

        public override void Update(float deltaTime) 
        {
            _timeToMove += deltaTime;
            if (!_isDie && _timeToMove > _speed) 
            {
                _timeToMove = 0;
                Move();
                _currentLevel.EntityRenderUpdate(this);
            }

        }

        private void Move()
        {
            Vector2Int newPosition = _currentPosition + _direction;

            if (_currentLevel.GetCell(newPosition) != null && _currentLevel.GetCell(newPosition).Tag != "Projectile")
            {
                DetectedCollision(_currentLevel.GetMap()[newPosition.y, newPosition.x]);
            }

            _lastPosition = _currentPosition;
            _currentPosition = newPosition;
        }

        private void NotSpawn()
        {
            _isDie = true;
        }

        private void DetectedCollision(IBehaviour behaviour)
        {
             switch (_currentLevel.GetCell(behaviour.CurrentPosition).Tag)
            {
                case "Wall": NotSpawn(); break;

                case "DestructibleWall1" or "PlayerTank":
                    {
                        _isDie = true;
                        _currentLevel.UpdateBehaviours.Remove(this);
                        _currentLevel.GetCell(behaviour.CurrentPosition).TakeDamage(_damege);
                        _currentLevel.EntityRenderUpdate(this);
                        _currentLevel.EntityRenderUpdate(behaviour);
                    }
                    break;

                case "Water": 
                    _background = _currentLevel.GetCell(behaviour.CurrentPosition).Background;
                    _currentLevel.EntityRenderUpdate(this);
                    break;

                default:
                    _background = ConsoleColor.Black;
                    break;
            }

        }
    }
}
