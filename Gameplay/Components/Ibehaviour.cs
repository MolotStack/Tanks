
using Tanks.Gameplay.Levels;
using Tanks.Utilities;

namespace Tanks.Gameplay.Components
{
    public abstract class IBehaviour
    {
        public Level CurrentLevel => _currentLevel;
        public Vector2Int CurrentPosition => _currentPosition;
        public Vector2Int LastPosition => _lastPosition;
        public string Tag => _tag;
        public bool IsDie => _isDie;
        public char[,] CurrentSprite => _currentSprite;
        public ConsoleColor Color => _color;
        public ConsoleColor Background => _background;

        protected Level _currentLevel;
        protected Vector2Int _currentPosition;
        protected Vector2Int _lastPosition;
        protected string _tag = "";
        protected bool _isDie;
        protected char[,] _currentSprite;
        protected ConsoleColor _color;
        protected ConsoleColor _background;


        public virtual void TakeDamage(int damage)
        {

        }
        public virtual void Update(float deltaTime)
        {

        }
    }
}
