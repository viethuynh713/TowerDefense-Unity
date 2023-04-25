using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using MythicEmpire.Enums;
using System.Linq;
using System;
using Unity.Burst.Intrinsics;

namespace MythicEmpire.InGame
{
    public static class InGameService
    {
        public static readonly int nPlayer = 2;

        public static readonly int columnIndexSplit = 11;
        public static readonly Vector2Int mapLogicPos = Vector2Int.zero;
        public static readonly Vector2Int monsterGateLogicPos = new Vector2Int(11, 4);
        public static readonly Dictionary<TypePlayer, Vector2Int> houseLogicPos = new Dictionary<TypePlayer, Vector2Int> {
            { TypePlayer.Player, new Vector2Int(22, 4) }, { TypePlayer.Opponent, new Vector2Int(0, 4) }
        };

        public static readonly int mapWidth = 23;
        public static readonly int mapHeight = 10;

        public static Vector3 Logic2DisplayPos(Vector2Int logicPos)
        {
            return new Vector3(logicPos.x, 0, logicPos.y);
        }

        public static Vector2Int Display2LogicPos(Vector3 displayPos)
        {
            return new Vector2Int(Mathf.RoundToInt(displayPos.x), Mathf.RoundToInt(displayPos.z));
        }



        class FPTile
        {
            public int x { get; set; }
            public int y { get; set; }
            public int cost { get; set; }
            public int distance { get; set; }
            public int costDistance => cost + distance;
            public FPTile parent { get; set; }

            //The distance is essentially the estimated distance, ignoring walls to our target. 
            //So how many tiles left and right, up and down, ignoring walls, to get there. 
            public void SetDistance(int targetX, int targetY)
            {
                distance = Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y);
            }
        }

        private static List<FPTile> GetWalkableTiles(List<string> map, FPTile currentTile, FPTile targetTile)
        {
            var possibleTiles = new List<FPTile>() {
                new FPTile { x = currentTile.x, y = currentTile.y - 1, parent = currentTile, cost = currentTile.cost + 1 },
                new FPTile { x = currentTile.x, y = currentTile.y + 1, parent = currentTile, cost = currentTile.cost + 1},
                new FPTile { x = currentTile.x - 1, y = currentTile.y, parent = currentTile, cost = currentTile.cost + 1 },
                new FPTile { x = currentTile.x + 1, y = currentTile.y, parent = currentTile, cost = currentTile.cost + 1 },
            };

            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.x, targetTile.y));

            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;

            return possibleTiles
                    .Where(tile => tile.x >= 0 && tile.x <= maxX)
                    .Where(tile => tile.y >= 0 && tile.y <= maxY)
                    .Where(tile => map[tile.y][tile.x] == ' ' || map[tile.y][tile.x] == 'B')
                    .ToList();
        }

        public static List<Vector2Int> FindPath(GameObject[][] realMap, Vector2Int startPos, Vector2Int des)
        {
            List<string> map = new List<string>();
            for (int i = 0; i < realMap.Length; i++)
            {
                string row = "";
                for (int j = 0; j < realMap[i].Length; j++)
                {
                    if (new Vector2Int(j, i) == startPos)
                    {
                        row += "A";
                    }
                    else if (new Vector2Int(j, i) == des)
                    {
                        row += "B";
                    }
                    else if (realMap[i][j].GetComponent<Tile>().IsBarrier)
                    {
                        row += "#";
                    }
                    else
                    {
                        row += " ";
                    }
                }
                map.Add(row);
            }

            var start = new FPTile();
            start.y = map.FindIndex(x => x.Contains("A"));
            start.x = map[start.y].IndexOf("A");

            var finish = new FPTile();
            finish.y = map.FindIndex(x => x.Contains("B"));
            finish.x = map[finish.y].IndexOf("B");

            start.SetDistance(finish.x, finish.y);

            var activeTiles = new List<FPTile> { start };
            var visitedTiles = new List<FPTile>();

            //This is where we created the map from our previous step etc. 

            while (activeTiles.Any())
            {
                var checkTile = activeTiles.OrderBy(x => x.costDistance).First();

                if (checkTile.x == finish.x && checkTile.y == finish.y)
                {
                    //We can actually loop through the parents of each tile to find our exact path which we will show shortly.
                    var tile = checkTile;
                    List<Vector2Int> path = new List<Vector2Int>();
                    while (true)
                    {
                        path.Insert(0, new Vector2Int(tile.x, tile.y));
                        if (map[tile.y][tile.x] == ' ')
                        {
                            var newMapRow = map[tile.y].ToCharArray();
                            newMapRow[tile.x] = '*';
                            map[tile.y] = new string(newMapRow);
                        }
                        tile = tile.parent;
                        if (tile == null)
                        {
                            return path;
                        }
                    }
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(map, checkTile, finish);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(x => x.x == walkableTile.x && x.y == walkableTile.y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might
                    //zigzag earlier but this is now straighter). 
                    if (activeTiles.Any(x => x.x == walkableTile.x && x.y == walkableTile.y))
                    {
                        var existingTile = activeTiles.First(x => x.x == walkableTile.x && x.y == walkableTile.y);
                        if (existingTile.costDistance > checkTile.costDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            return new List<Vector2Int>();
        }
    }
}
