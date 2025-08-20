using SlowpokeStudio.Grid;
using SlowpokeStudio.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Enemy
{
    public class Enemy : MonoBehaviour, IAI
    {
        private UnitMover mover;
        private PlayerController player;
        private bool isWaitingForPlayerMove = false;

        [SerializeField] internal Vector2Int CurrentGridPos => GetGridPos();

        private void Awake()
        {
            mover = GetComponent<UnitMover>();
        }

        internal void Initialize(PlayerController playerRef)
        {
            player = playerRef;
        }

        public void TakeTurn(HashSet<Vector2Int> occupiedTiles)
        {
            if (isWaitingForPlayerMove || mover.IsMoving || player == null)
            {
                return;
            }

            Vector2Int playerPos = player.CurrentGridPos;

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};

            foreach (var dir in directions)
            {
                Vector2Int targetPos = playerPos + dir;

                if (GridManager.Instance.IsObstacleAt(targetPos))
                {
                    continue;
                }
                if (occupiedTiles.Contains(targetPos))
                {
                    continue;
                }

                occupiedTiles.Add(targetPos);
                StartCoroutine(MoveToTarget(targetPos));
                isWaitingForPlayerMove = true;

                return;
            }
        }

        internal void NotifyPlayerMoved()
        {
            isWaitingForPlayerMove = false;
        }

        private IEnumerator MoveToTarget(Vector2Int targetGridPos)
        {
            yield return mover.MoveAlongPath(targetGridPos);
        }

        private Vector2Int GetGridPos()
        {
            Vector3 pos = transform.position;
            return new Vector2Int(Mathf.RoundToInt(pos.x / GridManager.Instance.spacing), Mathf.RoundToInt(pos.z / GridManager.Instance.spacing));
        }
    }
}
