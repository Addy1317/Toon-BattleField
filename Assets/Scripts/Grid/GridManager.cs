using SlowpokeStudio.Enemy;
using UnityEngine;

namespace SlowpokeStudio.Grid
{
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int gridSizeX = 10;
        [SerializeField] private int gridSizeY = 10;
        [SerializeField] internal float spacing = 1.1f;

        [Header("References")]
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Transform gridParent;

        [Header("Enemy Manager Reference")]
        [SerializeField] private EnemyManager EnemyManagerRef;

        private GridTile[,] gridTiles;
        public static GridManager Instance { get; private set; }
        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Duplicate GridManager detected. Destroying extra one.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Debug.Log("GridManager Singleton initialized.");
        }

        private void Start()
        {
            GenerateGrid();
            IsInitialized = true;
        }

        private void GenerateGrid()
        {
            Debug.Log("GridManager: Grid initialized.");

            if (tilePrefab == null)
            {
                Debug.LogError("Tile Prefab is not assigned in GridManager.");
                return;
            }

            gridTiles = new GridTile[gridSizeX, gridSizeY];

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 spawnPosition = new Vector3(x * spacing, 0, y * spacing);
                    GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, gridParent);
                    tile.name = $"Tile_{x}_{y}";

                    GridTile gridTile = tile.GetComponent<GridTile>();
                    if (gridTile != null)
                    {
                        gridTile.gridPosition = new Vector2Int(x, y);
                        gridTiles[x, y] = gridTile; 
                    }
                    else
                    {
                        Debug.LogWarning("Tile prefab is missing GridTile script.");
                    }
                }
            }

            Debug.Log("Grid generated successfully.");
            Debug.Log($"GridTiles length: {gridTiles.GetLength(0)}x{gridTiles.GetLength(1)}");

            if (gridTiles[0, 0] != null)
            {
                Debug.Log("gridTiles[0,0] is assigned ✅");
            }
            else
            {
                Debug.LogError("gridTiles[0,0] is NULL ❌");
            }
        }

        internal bool IsObstacleAt(Vector2Int pos)
        {
            if (gridTiles == null)
            {
                Debug.LogError("GridTiles not initialized. Make sure GenerateGrid() was called.");
                return true;
            }

            if (pos.x < 0 || pos.x >= gridSizeX || pos.y < 0 || pos.y >= gridSizeY)
            {
                return true;
            }

            return gridTiles[pos.x, pos.y].isBlocked;
        }

        internal Vector3 GridToWorld(Vector2Int pos)
        {
            return new Vector3(pos.x * spacing, 0f, pos.y * spacing);
        }

        internal GridTile GetTileAtPosition(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= gridSizeX || pos.y < 0 || pos.y >= gridSizeY)
                return null;

            return gridTiles[pos.x, pos.y];
        }

        internal bool IsEnemyAt(Vector2Int pos)
        {
            return EnemyManagerRef != null && EnemyManagerRef.occupiedTiles.Contains(pos);
        }
    }
}