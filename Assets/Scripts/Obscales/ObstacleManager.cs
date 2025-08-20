using SlowpokeStudio.Grid;
using SlowpokeStudio.Obstacle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Obstacle
{
    public class ObstacleManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] internal GridObstacleDataSO obstacleData;
        [SerializeField] internal GameObject obstaclePrefab; 
        [SerializeField] internal float obstacleYOffset = 0.5f;

        // Waits until GridManager is initialized before spawning obstacles
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GridManager.Instance != null && GridManager.Instance.IsInitialized);
            SpawnObstacles();
        }

        // Spawns obstacles at positions defined in obstacleData and marks corresponding grid tiles as blocked.
        private void SpawnObstacles()
        {
            if (!GridManager.Instance.IsInitialized)
            {
                Debug.LogError("ObstacleManager: Grid not ready!");
                return;
            }

            if (obstacleData == null)
            {
                Debug.LogWarning("ObstacleDataSO not assigned!");
                return;
            }

            foreach (Vector2Int pos in obstacleData.obstaclePositions)
            {
                if (!GridManager.Instance.IsObstacleAt(pos))
                {
                    var tile = GridManager.Instance.GetTileAtPosition(pos);
                    if (tile != null)
                    {
                        tile.isBlocked = true;
                    }
                    Vector3 worldPos = GridManager.Instance.GridToWorld(pos) + new Vector3(0, 0.5f, 0);
                    Instantiate(obstaclePrefab, worldPos, Quaternion.identity, transform);
                }
            }

            Debug.Log("Obstacles Spawned.");
        }
    }
}
