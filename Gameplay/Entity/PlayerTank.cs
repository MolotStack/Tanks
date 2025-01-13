using Tanks.Gameplay.Input;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Gameplay.Entity
{
    internal class PlayerTank : BaseTank
    {
        public PlayerTank(Level level, Vector2Int currentPosition, int health, float timeReload) : base(level, currentPosition, health, timeReload)
        {
            _tag = "PlayerTank";

            _color = ConsoleColor.Green;
            _background = ConsoleColor.Black;


            InputHandler.Instance.OnChangedDirection += OnMove;
            InputHandler.Instance.OnShoot += Shoot;
        }

        private float _currentSpeed = 0.10f;
        private float _timeMove = 0;

        public override void Update(float deltaTime)
        {
            if (_timeMove <= _currentSpeed)
            {
                _timeMove += deltaTime;
            }

            if (_timeMove > _currentSpeed && _directionInput != Vector2Int.zero)
            {
                _timeMove = 0;
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
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        }

        public override void Die()
        {
            InputHandler.Instance.OnChangedDirection -= OnMove;
            InputHandler.Instance.OnShoot -= Shoot;

            _isDie = true;
            _currentLevel.RemoveEntityMap(_currentPosition);
            _currentLevel.EntityRenderUpdate(this);
        }

        private void OnMove(Vector2Int direction)
        {
            _directionInput = direction;
        }
    }
}
