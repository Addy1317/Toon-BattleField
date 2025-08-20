using SlowpokeStudio.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [Header("Unit Mover Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float tileHeightOffset = 0.5f; 

    public bool IsMoving { get; private set; }

    internal IEnumerator MoveAlongPath(Vector2Int targetGridPos)
    {
        Vector2Int currentGridPos = GetCurrentGridPos();
        List<Vector2Int> path = Pathfinder.FindPath(currentGridPos, targetGridPos);

        if (path == null || path.Count == 0)
        {
            yield break;
        }

        IsMoving = true;

        foreach (Vector2Int step in path)
        {
            //Vector3 targetWorldPos = GridManager.Instance.GridToWorld(step);
            Vector3 targetWorldPos = GridManager.Instance.GridToWorld(step) + new Vector3(0, tileHeightOffset, 0);

            while (Vector3.Distance(transform.position, targetWorldPos) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = targetWorldPos;
        }

        IsMoving = false;
    }

    private Vector2Int GetCurrentGridPos()
    {
        Vector3 pos = transform.position;
        return new Vector2Int(Mathf.RoundToInt(pos.x / GridManager.Instance.spacing), Mathf.RoundToInt(pos.z / GridManager.Instance.spacing));
    }
}
