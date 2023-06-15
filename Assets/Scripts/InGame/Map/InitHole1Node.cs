using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Map
{
    public class InitHole1Node
    {
        public List<Vector2Int> restTile;
        public Vector2Int? selectedTile;
        public InitHole1Node(List<Vector2Int> prevRestTile, Vector2Int? prevSelectedTile)
        {
            restTile = prevRestTile;
            if (prevSelectedTile != null)
            {
                for (int i = prevSelectedTile.Value.x - 1; i <= prevSelectedTile.Value.x + 1; i++)
                {
                    for (int j = prevSelectedTile.Value.y - 1; j <= prevSelectedTile.Value.y + 1; j++)
                    {
                        restTile.Remove(new Vector2Int(i, j));
                    }
                }
            }
            if (restTile.Count == 0)
            {
                selectedTile = null;
            }
            else
            {
                selectedTile = restTile[UnityEngine.Random.Range(0, restTile.Count)];
            }
        }
        public bool CanAdd()
        {
            return selectedTile != null;
        }
        public void RemoveDo()
        {
            restTile.Remove(selectedTile.Value);
            selectedTile = restTile[UnityEngine.Random.Range(0, restTile.Count)];
        }
    }
}
