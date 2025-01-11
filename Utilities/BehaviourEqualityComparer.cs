using System.Diagnostics.CodeAnalysis;
using Tanks.Gameplay.Components;

namespace Tanks.Utilities
{
    public class BehaviourEqualityComparer : IEqualityComparer<IBehaviour>
    {
        public bool Equals(IBehaviour? x, IBehaviour? y)
        {
            if (x == null && y == null) return true;

            if (x.CurrentPosition == y.CurrentPosition && x.CurrentSprite == y.CurrentSprite && ReferenceEquals(x,y)) 
            {
                return true;
            }

            return false;
        }

        public int GetHashCode([DisallowNull] IBehaviour obj)
        {
            throw new NotImplementedException();
        }
    }
}
