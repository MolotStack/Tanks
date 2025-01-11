
namespace Tanks.Gameplay.Animations
{
    public class Animation<Tkey>
    {
        private Dictionary<Tkey, char[,]> _rosterAnimation = new Dictionary<Tkey, char[,]>();

        public void Add(Tkey key, char[,] sprites) 
        {
            if (!_rosterAnimation.ContainsKey(key))
            {
                _rosterAnimation.Add(key, sprites);
            }
        }

        public void Remove(Tkey key) 
        {
            if (_rosterAnimation.ContainsKey(key)) 
            {
                _rosterAnimation.Remove(key);
            }
        }

        public char[,] GetSprite(Tkey key)
        {
            return _rosterAnimation[key];
        }

    }
}
