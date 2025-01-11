
using Tanks.Gameplay.Components;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels
{
    public abstract class Level
    {
        public List<IBehaviour> ForRender = new List<IBehaviour>();
        public List<IBehaviour> UpdateBehaviours = new List<IBehaviour>();

        protected Map _currentMap;

        public Level(Vector2Int size, int seed, int countSpawnpointsEnemy) 
        {
            _currentMap = new Map(size, seed, this);
            _currentMap.Generation(countSpawnpointsEnemy);
            SpawnPlayer();
            SpawnEnemy();
        }

        public abstract void Update(float deltaTime);
        public abstract IBehaviour[,] GetMap();
        public abstract IBehaviour GetCell(Vector2Int position);
        public abstract void StepEntity(IBehaviour behaviour);
        public abstract void AddEntityMap(IBehaviour behaviour);
        public abstract void RemoveEntityMap(Vector2Int position);
        public abstract void EntityRenderUpdate(IBehaviour behaviour);
        public abstract void SpawnPlayer();
        public abstract void SpawnEnemy();
        public abstract void SpawnProjectile(IBehaviour behaviour);
    }
}
