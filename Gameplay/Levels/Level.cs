
using System.Xml.Linq;
using Tanks.Gameplay.Components;
using Tanks.Gameplay.Entity;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels
{
    public class Level
    {
        public event Action Victory;
        public event Action Defeat;

        public string Name => _name;
        private string _name;

        public List<IBehaviour> ForRender = new List<IBehaviour>();
        public List<IBehaviour> UpdateBehaviours = new List<IBehaviour>();

        public int CountEnemy => _enemyTanks.Count;

        protected Map _currentMap;

        private PlayerTank _playerTank;
        private List<EnemyTank> _enemyTanks;

        public Level(string nameLevel, Vector2Int size, int seed, int countSpawnpointsEnemy, int countWaterCell, int countEnemy) 
        {
            _name = nameLevel;

            _currentMap = new Map(size, seed, this, countSpawnpointsEnemy, countWaterCell);
            _currentMap.Generation();

            _enemyTanks = new List<EnemyTank>();

            SpawnPlayer();
            SpawnEnemy(countEnemy);
        }

        public void Update(float deltaTime)
        {
            if (UpdateBehaviours.Count > 0)
            {
                for (int i = 0; i < UpdateBehaviours.Count; i++)
                {
                    UpdateBehaviours[i].Update(deltaTime);
                }
            }

            if (_playerTank.IsDie)
            {
                Defeat.Invoke();
            }
        }

        public Map GetMap()
        {
            return _currentMap;
        }
        public IBehaviour GetCell(Vector2Int position)
        {
            return _currentMap.GetMap()[position.y, position.x];
        }
        public void StepEntity(IBehaviour behaviour)
        {
            _currentMap.StepEntity(behaviour);
        }

        public void AddEntityMap(IBehaviour behaviour)
        {
            _currentMap.AddEntity(behaviour);
        }
        public void RemoveEntityMap(Vector2Int position)
        {
            _currentMap.RemoveEntity(position);
        }


        public void EntityRenderUpdate(IBehaviour behaviour)
        {
            ForRender.Add(behaviour);
        }


        public void SpawnPlayer()
        {
            _playerTank = new PlayerTank(this, _currentMap.GetSpawnPointPlayer(), 5, 0.5f);
            UpdateBehaviours.Add(_playerTank);
            AddEntityMap(_playerTank);
        }

        public void SpawnEnemy(int countEnemy)
        {
            foreach (var spawnPointEnemy in _currentMap.GetSpawnPointEnemy())
            {
                if (countEnemy <= 0)
                {
                    break;
                }
                countEnemy--;
                _enemyTanks.Add(new EnemyTank(this, spawnPointEnemy, 2, 1f));
            }

            foreach (var enemyTank in _enemyTanks)
            {
                UpdateBehaviours.Add(enemyTank);
                AddEntityMap(enemyTank);
            }
        }
        public void RemoveEnemy(EnemyTank enemy)
        {
            if (_enemyTanks.Count > 0)
            {
                _enemyTanks.Remove(enemy);
                UpdateBehaviours.Remove(enemy);
            }

            if (CountEnemy == 0)
            {
                Victory?.Invoke();
            }
        }

        public void SpawnProjectile(IBehaviour behaviour)
        {
            if (!behaviour.IsDie)
            {
                UpdateBehaviours.Add(behaviour);
            }
        }
    }
}
