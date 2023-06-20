using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Enums;
using MythicEmpire.Map;
using static UnityEditor.PlayerSettings;
using System.Diagnostics;
using System;

namespace MythicEmpire.InGame
{
    public class MapService : MonoBehaviour
    {
        private GameObject[][] currentMap;
        private Tile[][] currentMapLogic;
        private int width;
        private int height;
        private Vector2Int startPoint = InGameService.monsterGateLogicPos;
        private Dictionary<TypePlayer, Vector2Int> endPoints = InGameService.houseLogicPos;

        [SerializeField] private GameObject emptyTile;
        [SerializeField] private GameObject barrierTile;
        [SerializeField] private GameObject monsterGateTile;
        [SerializeField] private GameObject bridgeTile;
        [SerializeField] private GameObject houseTile;
        [SerializeField] private GameObject coverTile;

        // Start is called before the first frame update
        void Start()
        {
            // initial empty map
            width = InGameService.mapWidth;
            height = InGameService.mapHeight;
            InitMapLogic();
            InitMap();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitMapLogic()
        {
            currentMapLogic = new Tile[height][];
            for (int i = 0; i < height; i++)
            {
                currentMapLogic[i] = new Tile[width];
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    currentMapLogic[i][j] = new Tile();
                    // generate tiles at the first and last column
                    if (j == 0 || j == width - 1)
                    {
                        bool isHouseTile = false;
                        foreach (var pos in InGameService.houseLogicPos)
                        {
                            if (pos.Value == new Vector2Int(j, i))
                            {
                                isHouseTile = true;
                                break;
                            }
                        }
                        currentMapLogic[i][j].IsBarrier = isHouseTile ? false : true;
                    }
                    // generate tiles in the rest
                    else
                    {
                        currentMapLogic[i][j].IsBarrier = false;
                    }
                    // mark the tile is on player field or opponent field
                    if (j != InGameService.columnIndexSplit)
                    {
                        currentMapLogic[i][j].Owner = j < InGameService.columnIndexSplit ? OwnerType.Opponent : OwnerType.Player;
                    }
                }
            }
            // init hole
            InitHole1();
            //var virtualPath = InitVirtualPath();
            //InitHole2(virtualPath);
        }

        public void InitMap()
        {
            // generate a null map
            currentMap = new GameObject[height][];
            for (int i = 0; i < height; i++)
            {
                currentMap[i] = new GameObject[width];
                for (int j = 0; j < width; j++)
                {
                    currentMap[i][j] = null;
                }
            }
            // generate houses
            foreach (var pos in InGameService.houseLogicPos)
            {
                currentMap[pos.Value.y][pos.Value.x] = Instantiate(houseTile, InGameService.Logic2DisplayPos(pos.Value), Quaternion.Euler(0, pos.Value.x == 0 ? 90 : -90, 0));
            }
            // generate monster gate
            currentMap[InGameService.monsterGateLogicPos.y][InGameService.monsterGateLogicPos.x] = Instantiate(monsterGateTile, InGameService.Logic2DisplayPos(InGameService.monsterGateLogicPos), Quaternion.Euler(0, 90, 0));
            // generate bridge tile
            for (int i = 0; i < height; i++)
            {
                currentMap[i][InGameService.columnIndexSplit] = Instantiate(bridgeTile, InGameService.Logic2DisplayPos(new Vector2Int(InGameService.columnIndexSplit, i)), Quaternion.Euler(0, 90, 0));
            }
            // generate the rest
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (currentMap[i][j] == null)
                    {
                        if (currentMapLogic[i][j].IsBarrier)
                        {
                            currentMap[i][j] = Instantiate(barrierTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.identity);
                        }
                        else
                        {
                            currentMap[i][j] = Instantiate(emptyTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.identity);
                        }
                    }
                    currentMap[i][j].transform.parent = transform;
                }
            }
        }

        public void UpdateMap()
        {

        }

        public bool IsValidPosition(Vector2Int logicPos, bool isMyPlayer)
        {
            if (logicPos.y >= 0 && logicPos.y < height)
            {
                if ((isMyPlayer && (logicPos.x > InGameService.columnIndexSplit && logicPos.x < width - 1))
                    || (!isMyPlayer && (logicPos.x > 0 && logicPos.x < InGameService.columnIndexSplit)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool BuildTower(Vector2Int pos, bool isMyPlayer, Tower tower)
        {
            if (currentMap[pos.y][pos.x].GetComponent<Tile>().IsBarrier)
            {
                return false;
            }
            if (IsValidPosition(pos, isMyPlayer))
            {
                currentMap[pos.y][pos.x].GetComponent<Tile>().IsBarrier = true;
                TypePlayer tp = isMyPlayer ? TypePlayer.Player : TypePlayer.Opponent;
                if (InGameService.FindPathForMonster(currentMap, startPoint, endPoints[tp], isMyPlayer).Count > 0)
                {
                    currentMap[pos.y][pos.x].GetComponent<Tile>().BuildTower(tower.transform);

                    Monster[] monsterList = FindObjectsOfType<Monster>();
                    foreach (Monster monster in monsterList)
                    {
                        monster.FindPath(pos);
                    }
                    return true;
                }
                currentMap[pos.y][pos.x].GetComponent<Tile>().IsBarrier = false;
            }
            return false;
        }

        public void SellTower(Vector2Int logicPos)
        {
            currentMap[logicPos.y][logicPos.x].GetComponent<Tile>().SellTower();
        }

        public bool IsGenMonsterValid(Vector2Int pos, bool isMyPlayer)
        {
            // it is invalid if the tile is barrier
            if (currentMap[pos.y][pos.x].GetComponent<Tile>().IsBarrier)
            {
                return false;
            }
            // it is valid if the monster is generated in owner field
            // note: the middle column is valid
            if (isMyPlayer)
            {
                return pos.x >= InGameService.columnIndexSplit ? true : false;
            }
            return pos.x <= InGameService.columnIndexSplit ? true : false;
        }

        public bool IsGenSpellValid(Vector3 displayPos, bool isMyPlayer)
        {
            Vector2Int logicPos = InGameService.Display2LogicPos(displayPos);
            if (logicPos.x > 0 && logicPos.x < width - 1 && logicPos.y >= 0 && logicPos.y < height)
            {
                return true;
            }
            return false;
        }

        public GameObject[][] CurrentMap { get { return currentMap; } }
        private void InitHole(Vector2Int generalPos)
        {
            Vector2Int playerPos = InGameService.PrivateLogicPos2PublicLogicPos(generalPos, true);
            Vector2Int opponentPos = InGameService.PrivateLogicPos2PublicLogicPos(generalPos, false);
            currentMapLogic[playerPos.y][playerPos.x].IsBarrier = true;
            currentMapLogic[opponentPos.y][opponentPos.x].IsBarrier = true;
        }
        private void InitHole1()
        {
            List<Vector2Int> tileList = new List<Vector2Int>();
            for (int i = 1; i < InGameService.localMapWidth - 1; i++)
            {
                for (int j = 1; j < InGameService.mapHeight - 1; j++)
                {
                    tileList.Add(new Vector2Int(i, j));
                }
            }
            HoleTileLinkedList holeTileLinkedList = new HoleTileLinkedList(tileList, 9);
            List<Vector2Int> holePosList = holeTileLinkedList.createAllHole();
            foreach (Vector2Int pos in holePosList)
            {
                InitHole(pos);
            }
        }
        
        private List<Vector2Int> InitVirtualPath()
        {
            Vector2Int startPos = new Vector2Int(startPoint.x + 1, startPoint.y);
            Vector2Int des = new Vector2Int(endPoints[TypePlayer.Player].x - 1, endPoints[TypePlayer.Player].y);
            MapGraph graph = new MapGraph();
            for (int i = 0; i < currentMapLogic.Length; i++)
            {
                for (int j = InGameService.columnIndexSplit + 1; j < currentMapLogic[i].Length - 1; j++)
                {
                    if (!currentMapLogic[i][j].IsBarrier)
                    {
                        if (i < currentMapLogic.Length - 1 && !currentMapLogic[i + 1][j].IsBarrier)
                        {
                            graph.AddEdge(new Vector2Int(j, i), new Vector2Int(j, i + 1));
                        }
                        if (j < currentMapLogic[i].Length - 1 && !currentMapLogic[i][j + 1].IsBarrier)
                        {
                            graph.AddEdge(new Vector2Int(j, i), new Vector2Int(j + 1, i));
                        }
                    }
                }
            }
            return graph.DFS(startPos, des);
        }

        private void InitHole2(List<Vector2Int> path)
        {
            // get a tile list which can generate barrier
            List<Vector2Int> validTile = new List<Vector2Int>();
            for (int i = 0; i < currentMapLogic.Length; i++)
            {
                for (int j = InGameService.columnIndexSplit + 1; j < currentMapLogic[i].Length - 1; j++)
                {
                    if (!currentMapLogic[i][j].IsBarrier)
                    {
                        if (!path.Contains(new Vector2Int(j, i)))
                        {
                            if (path.Contains(new Vector2Int(j - 1, i)) || path.Contains(new Vector2Int(j + 1, i))
                                || path.Contains(new Vector2Int(j, i - 1)) || path.Contains(new Vector2Int(j, i + 1)))
                            {
                                validTile.Add(new Vector2Int(j, i));
                            }
                        }
                    }
                }
            }
            // init hole
            int nHole = UnityEngine.Random.Range(2, 7);
            for (int i = 0; i < nHole; i++)
            {
                Vector2Int holePos = validTile[UnityEngine.Random.Range(0, validTile.Count)];
                InitHole(InGameService.PublicLogicPos2PrivateLogicPos(holePos, true));
                validTile.Remove(holePos);
                validTile.Remove(new Vector2Int(holePos.x - 1, holePos.y));
                validTile.Remove(new Vector2Int(holePos.x + 1, holePos.y));
                validTile.Remove(new Vector2Int(holePos.x, holePos.y - 1));
                validTile.Remove(new Vector2Int(holePos.x, holePos.y + 1));
                if (validTile.Count == 0)
                {
                    break;
                }
            }
        }
    }
}