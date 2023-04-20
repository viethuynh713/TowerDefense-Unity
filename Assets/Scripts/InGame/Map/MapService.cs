using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire
{
    public class MapService : MonoBehaviour
    {
        private const int width = 10;
        private const int height = 10;
        [SerializeField] private GameObject sampleTile;
        [SerializeField] private GameObject house;
        private GameObject[][] currentMap;

        // Start is called before the first frame update
        void Start()
        {
            // initial map
            currentMap = new GameObject[height][];
            for (int i = 0; i < height; i++)
            {
                currentMap[i] = new GameObject[width];
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    currentMap[i][j] = Instantiate(sampleTile, transform.position + new Vector3(j, 0, i), Quaternion.identity);
                    currentMap[i][j].transform.parent = transform;
                }
            }
            // initial house
            GameObject houseObj;
            if (transform.parent.gameObject.GetComponent<PlayerController>().IsMine)
            {
                houseObj = Instantiate(house, transform.position + new Vector3(10, 0, 5), Quaternion.Euler(new Vector3(0, -90, 0)));
            }
            else
            {
                houseObj = Instantiate(house, transform.position + new Vector3(-1, 0, 5), Quaternion.Euler(new Vector3(0, 90, 0)));
            }
            houseObj.transform.parent = transform;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitMap()
        {

        }

        class TreeTileLinkedList
        {
            public LinkedList<InitTree1Node> treeTileLinkedList;
            public int nTree;
            public TreeTileLinkedList(List<Vector2Int> tileList, int nTree)
            {
                // create a link list with the first node is start state
                treeTileLinkedList = new LinkedList<InitTree1Node>();
                treeTileLinkedList.AddLast(new InitTree1Node(tileList, null));
                this.nTree = nTree;
            }
            public List<Vector2Int> createAllTree()
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
        private void InitTree1()
        {
            List<Vector2Int> tileList = new List<Vector2Int>();
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    tileList.Add(new Vector2Int(i, j));
                }
            }
            TreeTileLinkedList treeTileLinkedList = new TreeTileLinkedList(tileList, 9);
            List<Vector2Int> treePosList = treeTileLinkedList.createAllTree();
        }

        public void UpdateMap()
        {

        }

        public void IsValidPosition()
        {

        }
    }
}