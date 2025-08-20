using System.Collections.Generic;
using UnityEngine;

public interface IAI
{
   // void TakeTurn();
    public void TakeTurn(HashSet<Vector2Int> occupiedTiles);

}
