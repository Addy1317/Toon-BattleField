using SlowpokeStudio.Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SlowpokeStudio.Grid
{
    public class TIleHoverDetector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TextMeshProUGUI tileInfoText;

        [Header("Raycast Settings")]
        [SerializeField] private LayerMask tileLayerMask; 

        private void Update()
        {
            DetectHoveredTile();
        }

        private void DetectHoveredTile()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayerMask))
            {
                GridTile gridTile = hit.collider.GetComponent<GridTile>();

                if (gridTile != null)
                {
                    tileInfoText.text = $"Hovered Tile: ({gridTile.gridPosition.x}, {gridTile.gridPosition.y})";
                }
            }
            else
            {
                tileInfoText.text = "Hovered Tile: None";
            }
        }
    }
}
