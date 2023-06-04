using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using MythicEmpire.Enums;
using System.Linq;
using System;
using Unity.Burst.Intrinsics;
using System.Runtime.CompilerServices;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI.Table;
using System.Diagnostics;

namespace MythicEmpire.InGame
{
    public static class InGameService
    {
        public static readonly int nPlayer = 2;

        public static readonly int columnIndexSplit = 10;
        public static readonly Vector2Int mapLogicPos = Vector2Int.zero;
        public static readonly Vector2Int monsterGateLogicPos = new Vector2Int(10, 4);
        public static readonly Dictionary<TypePlayer, Vector2Int> houseLogicPos = new Dictionary<TypePlayer, Vector2Int> {
            { TypePlayer.Player, new Vector2Int(20, 4) }, { TypePlayer.Opponent, new Vector2Int(0, 4) }
        };
        public static readonly Vector3 rootVector = Vector3.forward;
        public static readonly float infinitesimal = 0.01f;

        public static readonly int localMapWidth = 9;
        public static readonly int mapWidth = 21;
        public static readonly int mapHeight = 9;

        public static readonly int playerHp = 20;
        public static readonly int maxEnergy = 100;
        public static readonly int playerEnergy = 15;
        public static readonly int nWave = 5;
        public static readonly int waveTimeDelay = 10;

        public static readonly int monsterLayerMask = 1 << 3;

        public static readonly int maxTowerLevel = 3;

        public static readonly Dictionary<Tuple<CardType, string>, int> cardCost = new Dictionary<Tuple<CardType, string>, int>()
        {
            { new Tuple<CardType, string>(CardType.TowerCard, "1"), 8 }
        };

        public static readonly List<List<Tuple<string, int>>> monsterWave = new List<List<Tuple<string, int>>>
        {
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 1),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 2),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 3),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 4),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 5),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 6),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 7),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 8),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 9),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 10),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 11),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 12),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 13),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 14),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 15),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 16),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 17),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 18),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 19),
            },
            new List<Tuple<string, int>>
            {
                new Tuple<string, int>("1", 20),
            },
        };

        public static Vector2Int PrivateLogicPos2PublicLogicPos(Vector2Int logicPos, bool isMyPlayer)
        {
            if (isMyPlayer)
            {
                return new Vector2Int(logicPos.x + columnIndexSplit + 1, logicPos.y);
            }
            return new Vector2Int(columnIndexSplit - 1 - logicPos.x, mapHeight - logicPos.y - 1);
        }

        public static Vector2Int PublicLogicPos2PrivateLogicPos(Vector2Int logicPos, bool isMyPlayer)
        {
            if (isMyPlayer)
            {
                return new Vector2Int(logicPos.x - columnIndexSplit - 1, logicPos.y);
            }
            return new Vector2Int(columnIndexSplit - 1 - logicPos.x, mapHeight - 1 - logicPos.y);
        }

        public static Vector3 Logic2DisplayPos(Vector2Int logicPos)
        {
            return new Vector3(logicPos.x, 0, logicPos.y);
        }

        public static Vector3 Logic2DisplayPos(Vector2Int logicPos, bool isMyPlayer)
        {
            if (isMyPlayer)
            {
                return new Vector3(logicPos.x + columnIndexSplit + 1, 0, logicPos.y);
            }
            return new Vector3(columnIndexSplit - 1 - logicPos.x, 0, mapHeight - logicPos.y - 1);
        }

        public static Vector2Int Display2LogicPos(Vector3 displayPos)
        {
            return new Vector2Int(Mathf.RoundToInt(displayPos.x), Mathf.RoundToInt(displayPos.z));
        }

        public static bool IsValidLogicPos(Vector2Int logicPos, bool isMyPlayer)
        {
            if (logicPos.y > 0 && logicPos.y < mapHeight)
            {
                if (isMyPlayer)
                {
                    if (logicPos.x > columnIndexSplit && logicPos.x < mapWidth - 1)
                    {
                        return true;
                    }
                    return false;
                }
                if (logicPos.x > 0 && logicPos.x < columnIndexSplit)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        // find path for monster
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

        private static List<FPTile> GetWalkableTiles(List<string> map, FPTile currentTile, FPTile targetTile, bool isMyPlayer)
        {
            List<FPTile> possibleTiles;

            if (currentTile.x == columnIndexSplit)
            {
                if (isMyPlayer)
                {
                    possibleTiles = new List<FPTile>()
                    {
                        new FPTile { x = currentTile.x + 1, y = currentTile.y, parent = currentTile, cost = currentTile.cost + 1 }
                    };
                }
                else
                {
                    possibleTiles = new List<FPTile>()
                    {
                        new FPTile { x = currentTile.x - 1, y = currentTile.y, parent = currentTile, cost = currentTile.cost + 1 }
                    };
                }
            }
            else
            {
                possibleTiles = new List<FPTile>() {
                    new FPTile { x = currentTile.x, y = currentTile.y - 1, parent = currentTile, cost = currentTile.cost + 1 },
                    new FPTile { x = currentTile.x, y = currentTile.y + 1, parent = currentTile, cost = currentTile.cost + 1 },
                    new FPTile { x = currentTile.x - 1, y = currentTile.y, parent = currentTile, cost = currentTile.cost + 1 },
                    new FPTile { x = currentTile.x + 1, y = currentTile.y, parent = currentTile, cost = currentTile.cost + 1 }
                };
            }

            possibleTiles.ForEach(tile => tile.SetDistance(targetTile.x, targetTile.y));

            var maxX = map.First().Length - 1;
            var maxY = map.Count - 1;

            return possibleTiles
                    .Where(tile => tile.x >= 0 && tile.x <= maxX)
                    .Where(tile => tile.y >= 0 && tile.y <= maxY)
                    .Where(tile => map[tile.y][tile.x] == ' ' || map[tile.y][tile.x] == 'B')
                    .ToList();
        }

        public static List<Vector2Int> FindPathForMonster(GameObject[][] realMap, Vector2Int startPos, Vector2Int des, bool isMyPlayer)
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

                var walkableTiles = GetWalkableTiles(map, checkTile, finish, isMyPlayer);

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

        // find path for creating map
        class GenerateMapNodeCost
        {
            public List<Vector2Int> path { get; set; }
            public int nAdjacentNode { get; set; }
            public int distance { get; set; }
            public int nTurn { get; set; }
            public GenerateMapNodeCost()
            {
                path = new List<Vector2Int>();
                nAdjacentNode = 0;
                distance = 0;
                nTurn = 0;
            }
            public GenerateMapNodeCost Clone()
            {
                var clone = new GenerateMapNodeCost();
                clone.path = new List<Vector2Int>(path);
                clone.nAdjacentNode = nAdjacentNode;
                clone.distance = distance;
                clone.nTurn = nTurn;
                return clone;
            }
            public static bool operator >(GenerateMapNodeCost lhs, GenerateMapNodeCost rhs)
            {
                if (lhs.nAdjacentNode > rhs.nAdjacentNode) return true;
                if (lhs.nAdjacentNode < rhs.nAdjacentNode) return false;
                if (lhs.distance > rhs.distance) return true;
                if (lhs.distance < rhs.distance) return false;
                if (lhs.nTurn > rhs.nTurn) return true;
                if (lhs.nTurn < rhs.nTurn) return false;
                return false;
            }
            public static bool operator <(GenerateMapNodeCost lhs, GenerateMapNodeCost rhs)
            {
                if (lhs.nAdjacentNode > rhs.nAdjacentNode) return false;
                if (lhs.nAdjacentNode < rhs.nAdjacentNode) return true;
                if (lhs.distance > rhs.distance) return false;
                if (lhs.distance < rhs.distance) return true;
                if (lhs.nTurn > rhs.nTurn) return false;
                if (lhs.nTurn < rhs.nTurn) return true;
                return false;
            }
        }
        class Graph
        {
            Dictionary<Vector2Int, LinkedList<Vector2Int>> linkedListArray;

            public Graph()
            {
                linkedListArray = new Dictionary<Vector2Int, LinkedList<Vector2Int>>();
            }

            /// The method takes two nodes for which to add edge.
            public void AddEdge(Vector2Int u, Vector2Int v, bool blnBiDir = true)
            {
                LinkedList<Vector2Int> value;
                if (linkedListArray.TryGetValue(u, out value))
                {
                    var last = value.Last;
                    value.AddAfter(last, v);
                }
                else
                {
                    linkedListArray.Add(u, new LinkedList<Vector2Int>());
                    linkedListArray[u].AddFirst(v);
                }

                if (blnBiDir)
                {
                    if (linkedListArray.TryGetValue(v, out value))
                    {
                        var last = value.Last;
                        value.AddAfter(last, u);
                    }
                    else
                    {
                        linkedListArray.Add(v, new LinkedList<Vector2Int>());
                        linkedListArray[v].AddFirst(u);
                    }
                }
            }

            internal GenerateMapNodeCost DFSHelper(Vector2Int src, Vector2Int des, GenerateMapNodeCost cost)
            {
                if (src == des)
                {
                    return cost;
                }
                GenerateMapNodeCost bestCost = null;
                if (linkedListArray[src] != null)
                {
                    foreach (var item in linkedListArray[src])
                    {
                        var costClone = cost.Clone();
                        var path = cost.path;
                        // check if the item is in path or adjacent any node in path
                        if (path.Contains(item))
                        {
                            continue;
                        }
                        bool isValid = true;
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            var node = path[i];
                            if ((node.x > 0 && item.x == node.x - 1 && item.y == node.y)
                                || (node.x < mapWidth - 1 && item.x == node.x + 1 && item.y == node.y)
                                || (node.y > 0 && item.x == node.x && item.y == node.y - 1)
                                || (node.y < mapHeight - 1 && item.x == node.x && item.y == node.y + 1))
                            {
                                isValid = false;
                                break;
                            }
                        }
                        if (!isValid)
                        {
                            continue;
                        }
                        // check if the item is adjacent any holes
                        LinkedList<Vector2Int> value;
                        if ((item.y > 0 && !linkedListArray.TryGetValue(new Vector2Int(item.x, item.y - 1), out value))
                            || (item.y < mapHeight - 1 && !linkedListArray.TryGetValue(new Vector2Int(item.x, item.y + 1), out value))
                            || (item.x > columnIndexSplit + 1 && !linkedListArray.TryGetValue(new Vector2Int(item.x - 1, item.y), out value))
                            || (item.x < mapWidth - 2 && !linkedListArray.TryGetValue(new Vector2Int(item.x + 1, item.y), out value))
                            || (item.y > 0 && item.x > columnIndexSplit + 1 && !linkedListArray.TryGetValue(new Vector2Int(item.x - 1, item.y - 1), out value))
                            || (item.y < mapHeight - 1 && item.x > columnIndexSplit + 1 && !linkedListArray.TryGetValue(new Vector2Int(item.x - 1, item.y + 1), out value))
                            || (item.y > 0 && item.x < mapWidth - 2 && !linkedListArray.TryGetValue(new Vector2Int(item.x + 1, item.y - 1), out value))
                            || (item.y < mapHeight - 1 && item.x < mapWidth - 2 && !linkedListArray.TryGetValue(new Vector2Int(item.x + 1, item.y + 1), out value)))
                        {
                            costClone.nAdjacentNode++;
                        }
                        // increase distance
                        costClone.distance++;
                        // check if the item is a turn
                        if (path.Count > 1
                            && !(item.x - path[path.Count - 1].x == path[path.Count - 1].x - path[path.Count - 2].x
                                && item.y - path[path.Count - 1].y == path[path.Count - 1].y - path[path.Count - 2].y))
                        {
                            costClone.nTurn++;
                        }
                        costClone.path.Add(item);
                        var checkCost = DFSHelper(item, des, costClone);
                        if (bestCost == null || (checkCost != null && checkCost > bestCost))
                        {
                            bestCost = checkCost;
                        }
                    }
                }
                return bestCost;
            }

            internal List<Vector2Int> DFS(Vector2Int src, Vector2Int des)
            {
                GenerateMapNodeCost cost = new GenerateMapNodeCost();
                cost.path.Add(src);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var resCost = DFSHelper(src, des, cost);
                stopwatch.Stop();
                UnityEngine.Debug.Log("Elapsed Time is {0} ms" + stopwatch.ElapsedMilliseconds.ToString());
                if (resCost == null)
                {
                    throw new Exception("Path is null. Maybe the graph is invalid.");
                }
                return resCost.path;
            }
        }

        public static List<Vector2Int> FindPathForCreatingMap(GameObject[][] realMap, Vector2Int startPos, Vector2Int des)
        {
            startPos.x++;
            des.x--;
            Graph graph = new Graph();
            for (int i = 0; i < realMap.Length; i++)
            {
                for (int j = columnIndexSplit + 1; j < realMap[i].Length - 1; j++)
                {
                    if (!realMap[i][j].GetComponent<Tile>().IsBarrier)
                    {
                        if (i < realMap.Length - 1 && !realMap[i + 1][j].GetComponent<Tile>().IsBarrier)
                        {
                            graph.AddEdge(new Vector2Int(j, i), new Vector2Int(j, i + 1));
                        }
                        if (j < realMap[i].Length - 1 && !realMap[i][j + 1].GetComponent<Tile>().IsBarrier)
                        {
                            graph.AddEdge(new Vector2Int(j, i), new Vector2Int(j + 1, i));
                        }
                    }
                }
            }
            return graph.DFS(startPos, des);
        }
    }
}