using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MythicEmpire.Enums;
using static UnityEditor.PlayerSettings;

namespace MythicEmpire.InGame
{
    public class MapService : MonoBehaviour
    {
        private GameObject[][] currentMap;
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
            InitMap();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitMapLogic()
        {
            Tile[][] currentMap = new Tile[height][];
            for (int i = 0; i < height; i++)
            {
                currentMap[i] = new Tile[width];
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
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
                        currentMap[i][j] = new Tile();
                        currentMap[i][j].IsBarrier = isHouseTile ? false : true;
                    }
                    // generate tiles at the middle column
                    else if (j == InGameService.columnIndexSplit)
                    {
                        currentMap[i][j] = new Tile();
                        currentMap[i][j].IsBarrier = false;
                    }
                    // generate empty tiles in each side of players
                    else
                    {
                        currentMap[i][j] = new Tile();
                        currentMap[i][j].IsBarrier = false;
                        currentMap[i][j].Type = j < InGameService.columnIndexSplit ? TypeTile.Opponent : TypeTile.Player;
                    }
                    currentMap[i][j].transform.parent = transform;
                }
            }
        }

        public void InitMap()
        {
            currentMap = new GameObject[height][];
            for (int i = 0; i < height; i++)
            {
                currentMap[i] = new GameObject[width];
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
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
                        currentMap[i][j] = isHouseTile
                            ? Instantiate(houseTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.Euler(0, j == 0 ? 90 : -90, 0))
                            : Instantiate(barrierTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.identity);
                    }
                    // generate tiles at the middle column
                    else if (j == InGameService.columnIndexSplit)
                    {
                        currentMap[i][j] = InGameService.monsterGateLogicPos == new Vector2Int(j, i)
                            ? Instantiate(monsterGateTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.Euler(0, 90, 0))
                            : Instantiate(bridgeTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.Euler(0, 90, 0));
                    }
                    // generate empty tiles in each side of players
                    else
                    {
                        currentMap[i][j] = Instantiate(emptyTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.identity);
                        currentMap[i][j].GetComponent<Tile>().Type = j < InGameService.columnIndexSplit
                            ? TypeTile.Opponent : TypeTile.Player;
                    }
                    currentMap[i][j].transform.parent = transform;
                }
            }
            // generate cover tiles for decorating
            for (int j = 0; j < width; j++)
            {
                Instantiate(coverTile, new Vector3(j, 0, -1), Quaternion.identity);
                Instantiate(coverTile, new Vector3(j, 0, height), Quaternion.identity);
            }
            // init hole
            InitHole1();
            var virtualPath = InitVirtualPath();
            InitHole2(virtualPath);
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

        public GameObject[][] CurrentMap { get { return currentMap; } }

        class HoleTileLinkedList
        {
            public LinkedList<InitTree1Node> treeTileLinkedList;
            public int nTree;
            public HoleTileLinkedList(List<Vector2Int> tileList, int nTree)
            {
                // create a link list with the first node is start state
                treeTileLinkedList = new LinkedList<InitTree1Node>();
                treeTileLinkedList.AddLast(new InitTree1Node(tileList, null));
                this.nTree = nTree;
            }
            public List<Vector2Int> createAllHole()
            {
                while (treeTileLinkedList.Count < nTree)
                {
                    SelectTile();
                }
                List<Vector2Int> res = new List<Vector2Int>();
                foreach (InitTree1Node node in treeTileLinkedList)
                {
                    res.Add(node.selectedTile.Value);
                }
                return res;
            }
            public void SelectTile()
            {
                if (treeTileLinkedList.Last.Value.CanAdd())
                {
                    InitTree1Node lastNode = treeTileLinkedList.Last.Value;
                    treeTileLinkedList.AddLast(new InitTree1Node(lastNode.restTile, lastNode.selectedTile));
                }
                else
                {
                    treeTileLinkedList.RemoveLast();
                    treeTileLinkedList.Last.Value.RemoveDo();
                }
            }
        }
        class InitTree1Node
        {
            public List<Vector2Int> restTile;
            public Vector2Int? selectedTile;
            public InitTree1Node(List<Vector2Int> prevRestTile, Vector2Int? prevSelectedTile)
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
                    selectedTile = restTile[Random.Range(0, restTile.Count)];
                }
            }
            public bool CanAdd()
            {
                return selectedTile != null;
            }
            public void RemoveDo()
            {
                restTile.Remove(selectedTile.Value);
                selectedTile = restTile[Random.Range(0, restTile.Count)];
            }
        }
        private void InitHole(Vector2Int pos)
        {
            Vector2Int playerPos = InGameService.PrivateLogicPos2PublicLogicPos(pos, true);
            Vector2Int opponentPos = InGameService.PrivateLogicPos2PublicLogicPos(pos, false);
            Destroy(currentMap[playerPos.y][playerPos.x].gameObject);
            Destroy(currentMap[opponentPos.y][opponentPos.x].gameObject);
            currentMap[playerPos.y][playerPos.x] = Instantiate(barrierTile, InGameService.Logic2DisplayPos(playerPos), Quaternion.identity);
            currentMap[opponentPos.y][opponentPos.x] = Instantiate(barrierTile, InGameService.Logic2DisplayPos(opponentPos), Quaternion.identity);
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
            List<Vector2Int> path = InGameService.FindPathForCreatingMap(currentMap, startPoint, endPoints[TypePlayer.Player]);
            //string s = "";
            //foreach (Vector2Int pos in path)
            //{
            //    s += pos.ToString() + " -> ";
            //}
            //Debug.Log(s);
            return path;
        }

        private void InitHole2(List<Vector2Int> path)
        {
            // get a tile list which can generate barrier
            List<Vector2Int> validTile = new List<Vector2Int>();
            for (int i = 0; i < currentMap.Length; i++)
            {
                for (int j = InGameService.columnIndexSplit + 1; j < currentMap[i].Length - 1; j++)
                {
                    if (!currentMap[i][j].GetComponent<Tile>().IsBarrier)
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
            int nHole = Random.Range(2, 7);
            for (int i = 0; i < nHole; i++)
            {
                Vector2Int holePos = validTile[Random.Range(0, validTile.Count)];
                Debug.Log(holePos);
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