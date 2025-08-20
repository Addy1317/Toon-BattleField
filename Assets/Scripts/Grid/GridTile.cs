using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlowpokeStudio.Grid
{
    public class GridTile : MonoBehaviour
    {
        [Header("Tile Info")]
        [SerializeField] internal Vector2Int gridPosition;
        [SerializeField] internal bool isBlocked = false;
    }
}