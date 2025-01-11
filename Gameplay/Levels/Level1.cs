using Tanks.Gameplay.Components;
using Tanks.Gameplay.Entity;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels
{
    public class Level1 : Level
    {
        private PlayerTank _playerTank;
        public Level1(Vector2Int size, int seed, int countSpawnpointsEnemy) : base(size, seed, countSpawnpointsEnemy)
        {

        }

        public void CloseLevel()
        {

        }

        public override void Update(float deltaTime)
        {
            if (UpdateBehaviours.Count > 0)
            {
                for (int i = 0; i < UpdateBehaviours.Count; i++)
                {
                    UpdateBehaviours[i].Update(deltaTime);
                }
            }
        }

        public override IBehaviour[,] GetMap()
        {
            return _currentMap.GetMap();
        }
        public override IBehaviour GetCell(Vector2Int position)
        {
            return _currentMap.GetMap()[position.y,position.x];
        }
        public override void StepEntity(IBehaviour behaviour)
        {
            _currentMap.StepEntity(behaviour);
        }

        public override void AddEntityMap(IBehaviour behaviour)
        {
            _currentMap.AddEntity(behaviour);
        }
        public override void RemoveEntityMap(Vector2Int position)
        {
            _currentMap.RemoveEntity(position);
        }


        public override void EntityRenderUpdate(IBehaviour behaviour)
        {
            ForRender.Add(behaviour);
        }


        public override void SpawnPlayer()
        {
            _playerTank = new PlayerTank(this, _currentMap.GetSpawnPointPlayer());
            UpdateBehaviours.Add(_playerTank);
        }

        public override void SpawnEnemy()
        {

        }

        public override void SpawnProjectile(IBehaviour behaviour)
        {
            if (!behaviour.IsDie)
            {
                UpdateBehaviours.Add(behaviour);
            }
        }
    }
}
