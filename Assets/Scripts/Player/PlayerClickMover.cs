using SlowpokeStudio.Grid;
using SlowpokeStudio.Player;

using UnityEngine;

namespace SlowpokeStudio
{
    public class PlayerClickMover : MonoBehaviour
    {
        [Header("Player Click Mover Settings")]
        [SerializeField] internal PlayerController player;
        [SerializeField] internal LayerMask tileLayer;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayer))
                {
                    GridTile tile = hit.collider.GetComponent<GridTile>();
                    if (tile != null)
                    {
                        player.MoveToTile(tile.gridPosition);
                    }
                }
            }
        }
    }
}
