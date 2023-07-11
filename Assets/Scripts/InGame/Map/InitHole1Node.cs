using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Map
{
    public class InitHole1Node
    {
        public List<Vector2Int> restTile;   // tile lists which can be selected to generate hole at next step
        public Vector2Int? selectedTile;    // tile selected to generate hole
        public InitHole1Node(List<Vector2Int> prevRestTile, Vector2Int? prevSelectedTile)
        {
            // set rest tile list (do by remove tiles around selectedTile)
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
            // select a random tile to generate hole
            if (restTile.Count == 0)
            {
                selectedTile = null;
            }
            else
            {
                selectedTile = restTile[UnityEngine.Random.Range(0, restTile.Count)];
            }
        }
        // check if tile is selected at this step
        public bool CanAdd()
        {
            return selectedTile != null;
        }
        // turn back and select other tile if the selectedTile is invalid (means choosing this tile will cause cannot
        // select enough tile to generate hole in future)
        public void RemoveDo()
        {
            restTile.Remove(selectedTile.Value);
            selectedTile = restTile[UnityEngine.Random.Range(0, restTile.Count)];
        }
    }
}
