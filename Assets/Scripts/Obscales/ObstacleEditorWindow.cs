#if UNITY_EDITOR
using SlowpokeStudio.Obstacle;
using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    private GridObstacleDataSO obstacleData;

    private bool[,] toggleGrid = new bool[11, 11];

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        obstacleData = (GridObstacleDataSO)EditorGUILayout.ObjectField("Obstacle Data SO", obstacleData, typeof(GridObstacleDataSO), false);

        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please assign a GridObstacleDataSO asset.", MessageType.Warning);
            return;
        }

        EditorGUILayout.Space();

        for (int y = 9; y >= 0; y--) // Invert Y for top-down view
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < 10; x++)
            {
                bool isBlocked = toggleGrid[x, y];
                bool newToggle = GUILayout.Toggle(isBlocked, "", GUILayout.Width(30), GUILayout.Height(30));

                if (newToggle != isBlocked)
                {
                    toggleGrid[x, y] = newToggle;
                    Vector2Int pos = new Vector2Int(x, y);

                    if (newToggle && !obstacleData.obstaclePositions.Contains(pos))
                        obstacleData.obstaclePositions.Add(pos);
                    else if (!newToggle)
                        obstacleData.obstaclePositions.Remove(pos);

                    EditorUtility.SetDirty(obstacleData);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Clear All Obstacles"))
        {
            obstacleData.obstaclePositions.Clear();
            toggleGrid = new bool[10, 10];
            EditorUtility.SetDirty(obstacleData);
        }

        if (GUILayout.Button("Load from SO"))
        {
            LoadDataFromSO();
        }
    }

    private void LoadDataFromSO()
    {
        toggleGrid = new bool[10, 10];
        foreach (Vector2Int pos in obstacleData.obstaclePositions)
        {
            if (pos.x >= 0 && pos.x < 10 && pos.y >= 0 && pos.y < 10)
                toggleGrid[pos.x, pos.y] = true;
        }
    }
}
#endif