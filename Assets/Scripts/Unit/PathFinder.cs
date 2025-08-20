using SlowpokeStudio.Grid;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static A* Pathfinder class that calculates the shortest path between two grid positions.
/// </summary>
public static class Pathfinder
{
    // Finds a path from the start position to the end position using the A* algorithm.
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
    {
        HashSet<Vector2Int> closedSet = new();
        PriorityQueue<Vector2Int> openSet = new();
        openSet.Enqueue(start, 0);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new();
        Dictionary<Vector2Int, int> gScore = new() { [start] = 0 };

        while (openSet.Count > 0)
        {
            Vector2Int current = openSet.Dequeue();

            if (current == end)
            {
                return ReconstructPath(cameFrom, current);
            }

            closedSet.Add(current);

            foreach (Vector2Int dir in GetNeighbours())
            {
                Vector2Int neighbor = current + dir;
                if (GridManager.Instance.IsObstacleAt(neighbor) || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    int fScore = tentativeGScore + Heuristic(neighbor, end);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore);
                    }
                }
            }
        }

        return null; 
    }

    // Reconstructs the path by backtracking from the end to the start using the cameFrom map.
    static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new() { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    // Heuristic function: Manhattan distance (no diagonals).
    static int Heuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    // Returns all possible 4-directional neighbors (up, down, left, right).
    static Vector2Int[] GetNeighbours() => new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
}
