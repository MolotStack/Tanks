using Tanks.Gameplay.Components;
using Tanks.Gameplay.Input;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Gameplay.Entity
{
    public class EnemyTank : BaseTank
    {
        private AIBrain _aIBrain;

        private Random _random = new Random();
        private bool _isTarget = false;

        public EnemyTank(Level level, Vector2Int currentPosition, int health, float timeReload) : base(level, currentPosition, health, timeReload)
        {
            _tag = "EnemyTank";

            _color = ConsoleColor.Red;
            _background = ConsoleColor.Black;
            _directionInput = Vector2Int.down;
        }

        private float _currentSpeed = 0.75f;
        private float _timeMove = 0;

        public override void Update(float deltaTime)
        {
            GetTarget();

            if (!_isTarget) 
            {
                if (_timeMove <= _currentSpeed)
                {
                    _timeMove += deltaTime;
                }

                if (_timeMove > _currentSpeed)
                {
                    _timeMove = 0;
                    GetDirectionMovement();
                    Move(_directionInput);
                }

                if (_timeReload < _timeShoot && !_canShoot)
                {
                    _timeReload += deltaTime;
                }
                else
                {
                    _timeReload = 0;
                    _canShoot = true;
                }
                _currentLevel.EntityRenderUpdate(this);
            }


        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        }

        public override void Die()
        {
            _isDie = true;
            _currentLevel.RemoveEntityMap(_currentPosition);
            _currentLevel.RemoveEnemy(this);
            _currentLevel.EntityRenderUpdate(this);
        }

        private void GetDirectionMovement()
        {
            if (_currentLevel.GetCell(_directionInput + _currentPosition) == null)
            {
                return;
            }

            if (_currentLevel.GetCell(Vector2Int.up + _currentPosition) != null &&
                _currentLevel.GetCell(Vector2Int.right + _currentPosition) != null &&
                _currentLevel.GetCell(Vector2Int.down + _currentPosition) != null &&
                _currentLevel.GetCell(Vector2Int.left + _currentPosition) != null)
            {
                _directionInput = Vector2Int.zero;
                return;
            }


            RandomPosition();

            if (_currentLevel.GetCell(CurrentPosition + _directionInput) != null)
            {
                RandomPosition();
            }
        }

        private void RandomPosition()
        {
            switch (_random.Next(1, 5))
            {
                case 1:
                    _directionInput = Vector2Int.up;
                    break;

                case 2:
                    _directionInput = Vector2Int.down;
                    break;

                case 3:
                    _directionInput = Vector2Int.right;
                    break;

                case 4:
                    _directionInput = Vector2Int.left;
                    break;
            }
        }

        private void GetTarget()
        {

                Vector2Int _directionPatrol = _directionInput;

                while (_currentLevel.GetCell(_directionPatrol + _currentPosition) == null)
                {
                    _directionPatrol += _directionInput;
                }

                if (_currentLevel.GetCell(_directionPatrol + _currentPosition).Tag == "PlayerTank")
                {
                    Shoot();
                    _isTarget = true;
                    _directionInput = Vector2Int.zero;
                }
                else 
                {
                    _isTarget = false;
                    return;
                }

        }

        public override void Shoot()
        {
            if (_canShoot)
            {
                _canShoot = false;
                _currentLevel.SpawnProjectile(new Projectile(true, _currentLevel, _currentPosition, _currentDirectionMovement, 1));
                _currentLevel.EntityRenderUpdate(this);
            }
        }
    }
}

 
