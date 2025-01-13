
using Tanks.Gameplay.Components;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Gameplay.Entity
{
    public class Projectile : IBehaviour
    {
        private Level _currentLevel;

        private Vector2Int _direction;
        private float _speed = 0.05f;
        private float _timeToMove;
        private int _damege = 1;
        private bool _isEnemy = false;

        public Projectile(bool isEnemy, Level currentLevel,Vector2Int position, Vector2Int direction, int damage)
        {
            _tag = "Projectile";
            _isDie = false;
            _isEnemy = isEnemy;

            _currentPosition = position + direction;
            _lastPosition = _currentPosition;
            _direction = direction;

            _currentLevel = currentLevel;

            _currentSprite = new char[,] { { ' ', '0', ' ' }, { '0', '0', '0' }, { ' ', '0', ' ' } };
            _color = ConsoleColor.DarkRed;
            _background = ConsoleColor.Black;

            Spawn();
        }

        public override void Update(float deltaTime) 
        {
            _timeToMove += deltaTime;
            if (!_isDie && _timeToMove > _speed)
            {
                _timeToMove = 0;
                Move();

                if (_currentLevel.GetCell(CurrentPosition) != null)
                {
                    if (_currentLevel.GetCell(CurrentPosition).Tag == "Water")
                    {
                        _background = _currentLevel.GetCell(CurrentPosition).Background;
                    }
                }
                else
                {
                    _background = ConsoleColor.Black;
                }

                _currentLevel.EntityRenderUpdate(this);
            }
            else
            {
                if (_currentLevel.GetCell(CurrentPosition) != null)
                {
                    _currentLevel.EntityRenderUpdate(_currentLevel.GetCell(CurrentPosition));
                }
            }

        }

        private void Spawn()
        {
            if (!DetectedCollision(_currentLevel.GetMap().GetMap()[CurrentPosition.y, CurrentPosition.x])) 
            { 
                _currentLevel.EntityRenderUpdate(this);
            }

            if (_currentLevel.GetCell(CurrentPosition) != null)
            {
                if (_currentLevel.GetCell(CurrentPosition).Tag == "Water")
                {
                    _background = _currentLevel.GetCell(CurrentPosition).Background;
                }
            }
            else
            {
                _background = ConsoleColor.Black;
            }

            _currentLevel.EntityRenderUpdate(this);
            if (_currentLevel.GetCell(_lastPosition) != null)
            {
                _currentLevel.EntityRenderUpdate(_currentLevel.GetCell(_lastPosition));
            }
        }

        private void Move()
        {
            Vector2Int newPosition = _currentPosition + _direction;

            if (_currentLevel.GetCell(newPosition) != null)
            {
                DetectedCollision(_currentLevel.GetMap().GetMap()[newPosition.y, newPosition.x]);
            }

            if (_currentLevel.GetCell(_lastPosition) != null) 
            {
                _currentLevel.EntityRenderUpdate(_currentLevel.GetCell(_lastPosition));
            }

            _lastPosition = _currentPosition;
            _currentPosition = newPosition;
        }

        private bool DetectedCollision(IBehaviour behaviour)
        {
            if (behaviour != null) 
            {
                switch (_currentLevel.GetCell(behaviour.CurrentPosition).Tag)
                {
                    case "Wall":
                        _isDie = true; 
                        _currentLevel.UpdateBehaviours.Remove(this); 
                        break;

                    case "DestructibleWall1":
                
                        _isDie = true;
                        _currentLevel.UpdateBehaviours.Remove(this);
                        _currentLevel.GetCell(behaviour.CurrentPosition).TakeDamage(_damege);
                        _currentLevel.EntityRenderUpdate(behaviour);
                        break;

                    case "PlayerTank":
                        if (_isEnemy)
                        {
                            _isDie = true;
                            _currentLevel.UpdateBehaviours.Remove(this);
                            _currentLevel.GetCell(behaviour.CurrentPosition).TakeDamage(_damege);
                            _currentLevel.EntityRenderUpdate(behaviour);
                        }
                        else
                        {
                            _isDie = true;
                            _currentLevel.UpdateBehaviours.Remove(this);
                            _currentLevel.EntityRenderUpdate(behaviour);
                        }
                        break;

                    case "EnemyTank":
                            _isDie = true;
                            _currentLevel.UpdateBehaviours.Remove(this);
                            _currentLevel.GetCell(behaviour.CurrentPosition).TakeDamage(_damege);
                            _currentLevel.EntityRenderUpdate(behaviour);
                        break;

                    case "Water":
                        //_currentLevel.EntityRenderUpdate(behaviour);
                        break;

                    default:
                    break;

                }
                return true;
            }
            return false;

        }
    }
}
