using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Obstacle
{
    [CreateAssetMenu(fileName = "GridObstacleData", menuName = "Grid/Grid Obstacle Data")]
    public class GridObstacleDataSO : ScriptableObject
    {
        [Header("Obstacle Data")]
        [SerializeField] internal List<Vector2Int> obstaclePositions = new List<Vector2Int>();
    }
}
