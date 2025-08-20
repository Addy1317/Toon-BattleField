using SlowpokeStudio.Grid;
using UnityEngine;

namespace SlowpokeStudio
{
    public class GridUtility : MonoBehaviour
    {
        public static bool IsTileWalkable(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= 10 || gridPos.y < 0 || gridPos.y >= 10)
            {
                return false;
            }
            return !GridManager.Instance.IsObstacleAt(gridPos);
        }
    }
}
