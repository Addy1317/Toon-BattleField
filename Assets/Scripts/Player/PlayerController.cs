using SlowpokeStudio.Enemy;
using SlowpokeStudio.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Ref")]
        [SerializeField] internal LayerMask tileLayer;
        [SerializeField] internal EnemyManager enemyManager;
        public Vector2Int CurrentGridPos { get; private set; }

        private UnitMover mover;

        private void Awake()
        {
            mover = GetComponent<UnitMover>();
        }

        private void Update()
        {
            if (!GridManager.Instance || !GridManager.Instance.IsInitialized)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayer))
                {
                    GridTile tile = hit.collider.GetComponent<GridTile>();
                    if (tile != null)
                    {
                        Debug.Log("GridManager initialized? " + GridManager.Instance.IsInitialized);
                        MoveToTile(tile.gridPosition);
                    }
                }
            }
        }

        // Initiates movement to a target grid position if valid and not an obstacle.
        internal void MoveToTile(Vector2Int targetPos)
        {
            if (GridManager.Instance == null || GridManager.Instance.IsObstacleAt(targetPos))
            {
                Debug.Log("Invalid target or grid not initialized.");
                return;
            }

            if (!mover.IsMoving)
                StartCoroutine(MoveAndNotify(targetPos));
        }

        // Moves the player to the target position, updates the current position, and notifies the EnemyManager once movement is complete.
        private IEnumerator MoveAndNotify(Vector2Int targetPos)
        {
            yield return mover.MoveAlongPath(targetPos);
            CurrentGridPos = targetPos;

            enemyManager?.NotifyEnemiesPlayerMoved();
        }
    }
}
