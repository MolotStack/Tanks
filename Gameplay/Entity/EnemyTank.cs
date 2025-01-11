using Tanks.Gameplay.Input;
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Gameplay.Entity
{
    internal class EnemyTank : BaseTank
    {
        public EnemyTank(Level level, Vector2Int currentPosition) : base(level, currentPosition)
        {
            _tag = "PlayerTank";

            _color = ConsoleColor.Green;
            _background = ConsoleColor.Black;
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
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
        }

        public void Die()
        {

        }

        private void OnMove(Vector2Int direction)
        {
            _directionInput = direction;
        }
}
}
