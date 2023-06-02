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

        // find path
        interface FPCost: IComparable
        {
            public bool LessThan(FPCost other);
            public bool GreaterThan(FPCost other);
            public FPCost Add(FPCost other);
            public int GetDistance();
            public void SetDistance(int distance);
            public int GetNAdjacentNode();
            public void SetNAdjacentNode(int nAdjacentNode);
            public int GetNTurn();
            public void SetNTurn(int nTurn);
        }
        class FPMonsterCost: FPCost
        {
            public int distance { get; set; }
            public FPMonsterCost()
            {
                distance = 0;
            }
            public FPMonsterCost(int distance)
            {
                this.distance = distance + 1;
            }
            public int CompareTo(object incomingObj)
            {
                FPMonsterCost cost = (FPMonsterCost)incomingObj;
                return distance.CompareTo(cost.distance);
            }
            public bool LessThan(FPCost other)
            {
                if (other is FPMonsterCost _other)
                {
                    if (distance <= _other.distance)
                    {
                        return true;
                    }
                    return false;
                }
                throw new ArgumentException("Invalid type for LessThan method.");
            }
            public bool GreaterThan(FPCost other)
            {
                if (other is FPMonsterCost _other)
                {
                    if (distance > _other.distance)
                    {
                        return true;
                    }
                    return false;
                }
                throw new ArgumentException("Invalid type for GreaterThan method.");
            }
            public FPCost Add(FPCost other)
            {
                if (other is FPMonsterCost _other)
                {
                    FPMonsterCost result = new FPMonsterCost();
                    result.distance = distance + _other.distance;
                    return result;
                }
                throw new ArgumentException("Invalid type for Add method.");
            }
            public int GetDistance()
            {
                return distance;
            }
            public void SetDistance(int distance)
            {
                this.distance = distance;
            }
            public int GetNAdjacentNode()
            {
                throw new Exception("No variable nAdjacentNode");
            }
            public void SetNAdjacentNode(int nAdjacentNode)
            {
                throw new Exception("No variable nAdjacentNode");
            }
            public int GetNTurn()
            {
                throw new Exception("No variable nTurn");
            }
            public void SetNTurn(int nTurn)
            {
                throw new Exception("No variable nTurn");
            }
        }
        class FPVirtualCost: FPCost
        {
            public int nAdjacentNode { get; set; }
            public int distance { get; set; }
            public int nTurn { get; set; }

            public FPVirtualCost()
            {
                nAdjacentNode = 0;
                distance = 0;
                nTurn = 0;
            }
            public FPVirtualCost(List<string> map, int x, int y, FPTile parent)
            {
                // check if checked tile is next to one other tile at maximum
                FPTile iterTile = parent.parent;
                bool isValid = true;
                while (iterTile != null)
                {
                    if ((x > 0 && iterTile.x == x - 1 && iterTile.y == y)
                        || (x < mapWidth - 1 && iterTile.x == x + 1 && iterTile.y == y)
                        || (y > 0 && iterTile.x == x && iterTile.y == y - 1)
                        || (y < mapHeight - 1 && iterTile.x == x && iterTile.y == y + 1))
                    {
                        isValid = false;
                        break;
                    }
                    iterTile = iterTile.parent;
                }
                if (!isValid)
                {
                    distance = -1;
                    return;
                }
                // check if checked tile is next to a hole tile
                nAdjacentNode = parent.cost.GetNAdjacentNode();
                if ((y > 0 && map[y - 1][x] == '#') || (y < mapHeight - 1 && map[y + 1][x] == '#')
                    || (x > 1 && map[y][x - 1] == '#') || (x < mapWidth - 2 && map[y][x + 1] == '#')
                    || (y > 0 && x > 1 && map[y - 1][x - 1] == '#')
                    || (y < mapHeight - 1 && x > 1 && map[y + 1][x - 1] == '#')
                    || (y > 0 && x < mapWidth - 2 && map[y - 1][x + 1] == '#')
                    || (y < mapHeight - 1 && x < mapWidth - 2 && map[y + 1][x + 1] == '#'))
                {
                    nAdjacentNode++;
                }
                // increase distance
                distance = parent.cost.GetDistance() + 1;
                // check if checked tile is a turn
                nTurn = parent.cost.GetNTurn();
                if (parent.parent != null)
                {
                    if (!(x - parent.x == parent.x - parent.parent.x && y - parent.y == parent.y - parent.parent.y))
                    {
                        nTurn++;
                    }
                }
            }
            public int CompareTo(object incomingObj)
            {
                FPVirtualCost cost = (FPVirtualCost)incomingObj;
                if (nAdjacentNode != cost.nAdjacentNode)
                {
                    return cost.nAdjacentNode.CompareTo(nAdjacentNode);
                }
                if (distance != cost.distance)
                {
                    return cost.distance.CompareTo(distance);
                }
                return cost.nTurn.CompareTo(nTurn);
                
            }
            public bool LessThan(FPCost other)
            {
                if (other is FPVirtualCost _other)
                {
                    if (nAdjacentNode > _other.nAdjacentNode)
                    {
                        return true;
                    }
                    if (nAdjacentNode == _other.nAdjacentNode)
                    {
                        if (distance > _other.distance)
                        {
                            return true;
                        }
                        if (distance == _other.distance)
                        {
                            if (nTurn >= _other.nTurn)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                throw new ArgumentException("Invalid type for LessThan method.");
            }

            public bool GreaterThan(FPCost other)
            {
                if (other is FPVirtualCost _other)
                {
                    if (nAdjacentNode > _other.nAdjacentNode)
                    {
                        return false;
                    }
                    if (nAdjacentNode == _other.nAdjacentNode)
                    {
                        if (distance > _other.distance)
                        {
                            return false;
                        }
                        if (distance == _other.distance)
                        {
                            if (nTurn > _other.nTurn)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                throw new ArgumentException("Invalid type for GreaterThan method.");
            }
            public FPCost Add(FPCost other)
            {
                if (other is FPVirtualCost _other)
                {
                    FPVirtualCost res = new FPVirtualCost();
                    res.nAdjacentNode = nAdjacentNode + _other.nAdjacentNode;
                    res.distance = distance + _other.distance;
                    res.nTurn = nTurn + _other.nTurn;
                    return res;
                }
                throw new ArgumentException("Invalid type for Add method.");
            }
            public int GetDistance()
            {
                return distance;
            }
            public void SetDistance(int distance)
            {
                this.distance = distance;
            }
            public int GetNAdjacentNode()
            {
                return nAdjacentNode;
            }
            public void SetNAdjacentNode(int nAdjacentNode)
            {
                this.nAdjacentNode = nAdjacentNode;
            }
            public int GetNTurn()
            {
                return nTurn;
            }
            public void SetNTurn(int nTurn)
            {
                this.nTurn = nTurn;
            }
        }
        class FPTile
        {
            public int x { get; set; }
            public int y { get; set; }
            public FPTile parent { get; set; }
            public FPCost cost { get; set; } // g(x)
            public FPCost distance { get; set; } // h(x)
            public FPCost costDistance { get => cost.Add(distance); } // f(x)

            public FPTile(bool isFindRealPath, int x = 0, int y = 0, FPTile parent = null, List<string> map = null)
            {
                this.x = x;
                this.y = y;
                this.parent = parent;
                if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
                {
                    return;
                }
                if (isFindRealPath)
                {
                    if (parent == null)
                    {
                        cost = new FPMonsterCost();
                    }
                    else
                    {
                        cost = new FPMonsterCost(parent.cost.GetDistance());
                    }
                    distance = new FPMonsterCost();
                }
                else
                {
                    if (parent == null)
                    {
                        cost = new FPVirtualCost();
                    }
                    else
                    {
                        cost = new FPVirtualCost(map, x, y, parent);
                        if (cost.GetDistance() < 0)
                        {
                            x = -1;
                        }
                    }
                    distance = new FPVirtualCost();
                }
            }
            //The distance is essentially the estimated distance, ignoring walls to our target. 
            //So how many tiles left and right, up and down, ignoring walls, to get there. 
            public void SetDistance(int targetX, int targetY)
            {
                if (cost is FPMonsterCost)
                {
                    distance = (FPMonsterCost)distance;
                    distance.SetDistance(Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y));
                }
                else if (cost is FPVirtualCost)
                {
                    distance = (FPVirtualCost)distance;
                    distance.SetDistance(Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y));
                }
            }
        }

        private static List<FPTile> GetWalkableTiles(List<string> map, FPTile currentTile, FPTile targetTile, bool isMyPlayer, bool isFindRealPath)
        {
            List<FPTile> possibleTiles;

            if (currentTile.x == columnIndexSplit)
            {
                if (isMyPlayer)
                {
                    possibleTiles = new List<FPTile>()
                    {
                        new FPTile(isFindRealPath, currentTile.x + 1, currentTile.y, currentTile, map)
                    };
                }
                else
                {
                    possibleTiles = new List<FPTile>()
                    {
                        new FPTile(isFindRealPath, currentTile.x - 1, currentTile.y, currentTile, map)
                    };
                }
            }
            else
            {
                possibleTiles = new List<FPTile>() {
                    new FPTile(isFindRealPath, currentTile.x, currentTile.y - 1, currentTile, map),
                    new FPTile(isFindRealPath, currentTile.x, currentTile.y + 1, currentTile, map),
                    new FPTile(isFindRealPath, currentTile.x - 1, currentTile.y, currentTile, map),
                    new FPTile(isFindRealPath, currentTile.x + 1, currentTile.y, currentTile, map)
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

        public static List<Vector2Int> FindPath(GameObject[][] realMap, Vector2Int startPos, Vector2Int des, bool isMyPlayer, bool isFindRealPath)
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

            var start = new FPTile(isFindRealPath);
            start.y = map.FindIndex(x => x.Contains("A"));
            start.x = map[start.y].IndexOf("A");

            var finish = new FPTile(isFindRealPath);
            finish.y = map.FindIndex(x => x.Contains("B"));
            finish.x = map[finish.y].IndexOf("B");

            start.SetDistance(finish.x, finish.y);

            var activeTiles = new List<FPTile> { start };
            var visitedTiles = new List<FPTile>();
            int idx = 1;

            List<Vector2Int> resultPath = new List<Vector2Int>();

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
                            if (isFindRealPath)
                            {
                                return path;
                            }
                            Debug.Log("Index: " + idx.ToString());
                            Debug.Log("nAdjacentNode: " + checkTile.cost.GetNAdjacentNode().ToString());
                            Debug.Log("Distance: " + checkTile.cost.GetDistance().ToString());
                            Debug.Log("nTurn: " + checkTile.cost.GetNTurn().ToString());
                            idx++;
                            resultPath = path;
                            break;
                        }
                    }
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);
                if (!isFindRealPath && checkTile.x == finish.x && checkTile.y == finish.y)
                {
                    Debug.Log("Jump!");
                    continue;
                }

                var walkableTiles = GetWalkableTiles(map, checkTile, finish, isMyPlayer, isFindRealPath);

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
                        if (existingTile.costDistance.GreaterThan(checkTile.costDistance))
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

            if (isFindRealPath)
            {
                return new List<Vector2Int>();
            }
            return resultPath;
        }
    }
}
