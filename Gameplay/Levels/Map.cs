
using System.Collections.Generic;
using Tanks.Gameplay.Components;
using Tanks.Gameplay.Levels.Environment;
using Tanks.Utilities;

namespace Tanks.Gameplay.Levels
{
    public class Map
    {
        private IBehaviour[,] _map;

        private Vector2Int _spawnPositionPlayer;
        private List<Vector2Int> _spawnPositionEnemy;
        private Level _currentLevel;

        private Random _random;

        private int _countEnemySpawnPoint;

        public Map(Vector2Int size, int seed, Level currentLevel, int countEnemySpawnPoint)
        {
            _map = new IBehaviour[size.y, size.x];
            _random = new Random(seed);
            
            _spawnPositionEnemy = new List<Vector2Int>();

            _currentLevel = currentLevel;
            _countEnemySpawnPoint = countEnemySpawnPoint;
        }

        public void Generation()
        {
            FirstStageGeneration();
            SecondStageGeneration();

            SelectPositionEnemy();
            SelectPositionPlayer();

            ThirdStageGeneration();
            FourthStageGeneration();
        }

        public Vector2Int GetSpawnPointPlayer()
        {
            return _spawnPositionPlayer;
        }
        public void RemoveEntity(Vector2Int position)
        {
            _map[position.y, position.x] = null;
        }
        public void AddEntity(IBehaviour behaviour)
        {
            _map[behaviour.CurrentPosition.y, behaviour.CurrentPosition.x] = behaviour;
        }
        public void StepEntity(IBehaviour behaviour)
        {
            _map[behaviour.LastPosition.y, behaviour.LastPosition.x] = null;
            _map[behaviour.CurrentPosition.y, behaviour.CurrentPosition.x] = behaviour;
        }

        public IBehaviour[,] GetMap()
        {
            return _map;
        }

        private void FirstStageGeneration()// генерирует заполненное поле
        {
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if ((i == 0 || j == 0) || (i == _map.GetLength(0) - 1 || j == _map.GetLength(1) - 1))
                    {
                        _map[i, j] = new BaseWall(new Vector2Int(j, i), _currentLevel);
                    }
                    else 
                    {
                        _map[i, j] = new DestructibleWall(new Vector2Int(j, i), _currentLevel);
                    }
                }
            }
        }

        private void SecondStageGeneration() // рандомно удаляет части поля
        {
            int countEmpty;
            int countRepiatHorizontal;
            int countRepiatVertical;

            bool isRemoveHorizontal = true;
            bool isRemoveVertical = true;

            for (int i = 0;i < _map.GetLength(0); i++)
            {
                countEmpty = 15;
                countRepiatHorizontal = 0;
                countRepiatVertical = 0;

                isRemoveVertical = true;

                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    Vector2Int currentPosition = new Vector2Int(j, i);
                    isRemoveHorizontal = true;
                    if (j > 0)
                    {
                        if (_map[currentPosition.y, currentPosition.x - 1] == null)
                        {
                            countRepiatHorizontal = 3;
                            for (int xPosition = currentPosition.x - 1; _map[currentPosition.y, xPosition] == null; xPosition--)
                            {
                                countRepiatHorizontal--;
                                if (countRepiatHorizontal <= 0)
                                {
                                    isRemoveHorizontal = false;
                                    break;
                                }
                                else
                                {
                                    isRemoveHorizontal = true;
                                }
                            }
                        }
                    }

                    if (i > 0)
                    {
                        if (_map[currentPosition.y - 1, currentPosition.x] == null) 
                        {
                            countRepiatVertical = 3;
                            for (int yPosition = currentPosition.y - 1; _map[yPosition, currentPosition.x] == null; yPosition--)
                            {
                                countRepiatVertical--;
                                if (countRepiatVertical <= 0)
                                {
                                    isRemoveVertical = false;
                                    break;
                                }
                                else 
                                {
                                    isRemoveVertical = true;
                                }
                            }
                        }
                    }


                    
                    if (countEmpty > 0 && _random.Next(1, 3) > 1 && _map[i, j].Tag == "DestructibleWall1" 
                        && isRemoveHorizontal && isRemoveVertical)
                    {
                        RemoveEntity(currentPosition);
                        countEmpty--;
                    }
                }
            }
        
        }

        private void SelectPositionEnemy()
        {
            int countEnemyY = _countEnemySpawnPoint;
            int countEnemyX = _countEnemySpawnPoint;
            for (int i = 1; i < 3; i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (_map[i,j] == null && countEnemyX > 0)
                    {
                        _spawnPositionEnemy.Add(new Vector2Int(j, i));
                        countEnemyX--;
                    }

                    if (countEnemyX <= 0)
                    {
                        break; 
                    }
                }
            }
            
 

            for (int j = 1; j < 3; j++)
            {
                for (int i = 0; i < _map.GetLength(0); i++)
                {
                    if (_map[i, j] == null && countEnemyY > 0)
                    {
                        _spawnPositionEnemy.Add(new Vector2Int(j, i));
                        countEnemyY--;
                    }

                    if (countEnemyY <= 0)
                    {
                        break;
                    }
                }
            }
        }

        private void SelectPositionPlayer()
        {
            for (int i = _map.GetLength(0) - 1; i > 0; i--)
            {
                for (int j = _map.GetLength(1) - 1; j > 0; j--)
                {
                    if (_map[i, j] == null)
                    {
                        if (_random.Next(1,4) > 2)
                        {
                            _spawnPositionPlayer = new Vector2Int(j, i);
                            return;
                        }

                    }
                }
            }
        }

        private void ThirdStageGeneration() // делает примитивные пути от противника до игрока
        {
            foreach (var positionEnemy in _spawnPositionEnemy)
            {
                MovingTowardsPlayer(positionEnemy);
            }
        }

        private void FourthStageGeneration() // делает примитивные пути от противника до игрока
        {
            foreach (var positionEnemy in _spawnPositionEnemy)
            {
                MovingTowardsPlayer(positionEnemy);
            }
        }

        private void MovingTowardsPlayer(Vector2Int positionEnemy)
        {
            ////_map[positionEnemy.y, positionEnemy.x] = _debugWall;

            if (positionEnemy == _spawnPositionPlayer)
            {
                return;
            }

            int xDir = positionEnemy.x == _spawnPositionPlayer.x ? 0 : positionEnemy.x < _spawnPositionPlayer.x ? 1: -1;
            int yDir = positionEnemy.y == _spawnPositionPlayer.y ? 0 : positionEnemy.y < _spawnPositionPlayer.y ? 1 : -1;

            if (_map[positionEnemy.y + yDir, positionEnemy.x] == null && yDir != 0)
            {
                MovingTowardsPlayer(new Vector2Int(positionEnemy.x, positionEnemy.y + yDir));
            }
            else if (_map[positionEnemy.y, positionEnemy.x + xDir] == null && xDir != 0)
            {
                MovingTowardsPlayer(new Vector2Int(positionEnemy.x + xDir, positionEnemy.y));
            }
            else
            {
                Vector2Int newPosition = new Vector2Int();

                if (Math.Abs(_spawnPositionPlayer.x - positionEnemy.x) > Math.Abs(_spawnPositionPlayer.y - positionEnemy.y))
                {
                    newPosition = new Vector2Int(positionEnemy.x + xDir, positionEnemy.y);
                }
                else if (Math.Abs(_spawnPositionPlayer.x - positionEnemy.x) < Math.Abs(_spawnPositionPlayer.y - positionEnemy.y))
                {
                    newPosition = new Vector2Int(positionEnemy.x, positionEnemy.y + yDir);
                }
                else
                {
                    int randomDirection = _random.Next(1, 3);

                    if (randomDirection > 1)
                    {
                        newPosition = new Vector2Int(positionEnemy.x + xDir, positionEnemy.y);
                    }
                    else 
                    {
                        newPosition = new Vector2Int(positionEnemy.x, positionEnemy.y + yDir);
                    }
                }
                RemoveEntity(newPosition);
                MovingTowardsPlayer(newPosition);
            }
        }


    }
}
