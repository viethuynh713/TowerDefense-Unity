using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class MapService : MonoBehaviour
    {
        private GameObject[][] currentMap;

        [SerializeField] private GameObject emptyTile;
        [SerializeField] private GameObject barrierTile;
        [SerializeField] private GameObject monsterGateTile;
        [SerializeField] private GameObject bridgeTile;
        [SerializeField] private GameObject houseTile;
        [SerializeField] private GameObject coverTile;

        // Start is called before the first frame update
        void Start()
        {
            // initial map
            currentMap = new GameObject[InGameService.mapHeight][];
            for (int i = 0; i < InGameService.mapHeight; i++)
            {
                currentMap[i] = new GameObject[InGameService.mapWidth];
            }
            for (int i = 0; i < InGameService.mapHeight; i++)
            {
                for (int j = 0; j < InGameService.mapWidth; j++)
                {
                    if (j == 0 || j == InGameService.mapWidth - 1)
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
                    else if (j == InGameService.columnIndexSplit)
                    {
                        currentMap[i][j] = InGameService.monsterGateLogicPos == new Vector2Int(j, i)
                            ? Instantiate(monsterGateTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.Euler(0, 90, 0))
                            : Instantiate(bridgeTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.Euler(0, 90, 0));
                    }
                    else
                    {
                        currentMap[i][j] = Instantiate(emptyTile, InGameService.Logic2DisplayPos(new Vector2Int(j, i)), Quaternion.identity);
                        currentMap[i][j].GetComponent<Tile>().Type = j < InGameService.columnIndexSplit
                            ? Enums.TypeTile.Opponent : Enums.TypeTile.Player;
                    }
                    currentMap[i][j].transform.parent = transform;
                }
            }
            for (int j = 0; j < InGameService.mapWidth; j++)
            {
                Instantiate(coverTile, new Vector3(j, 0, -1), Quaternion.identity);
                Instantiate(coverTile, new Vector3(j, 0, InGameService.mapHeight), Quaternion.identity);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitMap()
        {

        }

        public void UpdateMap()
        {

        }

        public void IsValidPosition()
        {

        }

        public GameObject[][] CurrentMap { get { return currentMap; } }

        //class TreeTileLinkedList
        //{
        //    public LinkedList<InitTree1Node> treeTileLinkedList;
        //    public int nTree;
        //    public TreeTileLinkedList(List<Vector2Int> tileList, int nTree)
        //    {
        //        // create a link list with the first node is start state
        //        treeTileLinkedList = new LinkedList<InitTree1Node>();
        //        treeTileLinkedList.AddLast(new InitTree1Node(tileList, null));
        //        this.nTree = nTree;
        //    }
        //    public List<Vector2Int> createAllTree()
        //    {
        //        while (treeTileLinkedList.Count < nTree)
        //        {
        //            SelectTile();
        //        }
        //        List<Vector2Int> res = new List<Vector2Int>();
        //        foreach (InitTree1Node node in treeTileLinkedList)
        //        {
        //            res.Add(node.selectedTile.Value);
        //        }
        //        return res;
        //    }
        //    public void SelectTile()
        //    {
        //        if (treeTileLinkedList.Last.Value.CanAdd())
        //        {
        //            InitTree1Node lastNode = treeTileLinkedList.Last.Value;
        //            treeTileLinkedList.AddLast(new InitTree1Node(lastNode.restTile, lastNode.selectedTile));
        //        }
        //        else
        //        {
        //            treeTileLinkedList.RemoveLast();
        //            treeTileLinkedList.Last.Value.RemoveDo();
        //        }
        //    }
        //}
        //class InitTree1Node
        //{
        //    public List<Vector2Int> restTile;
        //    public Vector2Int? selectedTile;
        //    public InitTree1Node(List<Vector2Int> prevRestTile, Vector2Int? prevSelectedTile)
        //    {
        //        restTile = prevRestTile;
        //        if (prevSelectedTile != null)
        //        {
        //            for (int i = prevSelectedTile.Value.x - 1; i <= prevSelectedTile.Value.x + 1; i++)
        //            {
        //                for (int j = prevSelectedTile.Value.y - 1; j <= prevSelectedTile.Value.y + 1; j++)
        //                {
        //                    restTile.Remove(new Vector2Int(i, j));
        //                }
        //            }
        //        }
        //        if (restTile.Count == 0)
        //        {
        //            selectedTile = null;
        //        }
        //        else
        //        {
        //            selectedTile = restTile[Random.Range(0, restTile.Count)];
        //        }
        //    }
        //    public bool CanAdd()
        //    {
        //        return selectedTile != null;
        //    }
        //    public void RemoveDo()
        //    {
        //        restTile.Remove(selectedTile.Value);
        //        selectedTile = restTile[Random.Range(0, restTile.Count)];
        //    }
        //}
        //private void InitTree1()
        //{
        //    List<Vector2Int> tileList = new List<Vector2Int>();
        //    for (int i = 1; i < InGameService.mapHeight - 1; i++)
        //    {
        //        for (int j = 1; j < InGameService.mapWidth - 1; j++)
        //        {
        //            tileList.Add(new Vector2Int(i, j));
        //        }
        //    }
        //    TreeTileLinkedList treeTileLinkedList = new TreeTileLinkedList(tileList, 9);
        //    List<Vector2Int> treePosList = treeTileLinkedList.createAllTree();
        //}
    }
}