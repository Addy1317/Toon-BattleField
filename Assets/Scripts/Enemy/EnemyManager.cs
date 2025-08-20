using SlowpokeStudio.Grid;
using SlowpokeStudio.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController playerController;

        [Header("Enemy Settings")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private List<Vector2Int> enemySpawnPositions;
        [SerializeField] internal HashSet<Vector2Int> occupiedTiles = new();

        private List<Enemy> enemies = new();

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GridManager.Instance != null && GridManager.Instance.IsInitialized);

            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            if (!GridManager.Instance.IsInitialized)
            {
                Debug.LogError("GridManager not initialized. Cannot spawn enemies.");
                return;
            }

            foreach (var pos in enemySpawnPositions)
            {
                if (GridManager.Instance.IsObstacleAt(pos))
                {
                    continue;
                }

                Vector3 worldPos = GridManager.Instance.GridToWorld(pos) + new Vector3(0, 0.5f, 0);
                GameObject enemyGO = Instantiate(enemyPrefab, worldPos, Quaternion.identity, transform);

                Enemy enemyScript = enemyGO.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.Initialize(playerController);
                    enemies.Add(enemyScript);
                }
                else
                {
                    Debug.LogError("Enemy prefab is missing the Enemy.cs script.");
                }
            }

            Debug.Log($"Spawned {enemies.Count} enemies.");
        }

        internal void NotifyEnemiesPlayerMoved()
        {
            occupiedTiles.Clear();

            foreach (var enemy in enemies)
            {
                occupiedTiles.Add(enemy.CurrentGridPos); 
            }

            foreach (var enemy in enemies)
            {
                enemy.NotifyPlayerMoved();
                enemy.TakeTurn(occupiedTiles); 
            }
        }
    }
}
