using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Map
{
    public class HoleTileLinkedList
    {
        public LinkedList<InitHole1Node> treeTileLinkedList;
        public int nTree;
        public HoleTileLinkedList(List<Vector2Int> tileList, int nTree)
        {
            // create a link list with the first node is start state
            treeTileLinkedList = new LinkedList<InitHole1Node>();
            treeTileLinkedList.AddLast(new InitHole1Node(tileList, null));
            this.nTree = nTree;
        }
        public List<Vector2Int> createAllHole()
        {
            while (treeTileLinkedList.Count < nTree)
            {
                SelectTile();
            }
            List<Vector2Int> res = new List<Vector2Int>();
            foreach (InitHole1Node node in treeTileLinkedList)
            {
                res.Add(node.selectedTile.Value);
            }
            return res;
        }
        public void SelectTile()
        {
            if (treeTileLinkedList.Last.Value.CanAdd())
            {
                InitHole1Node lastNode = treeTileLinkedList.Last.Value;
                treeTileLinkedList.AddLast(new InitHole1Node(lastNode.restTile, lastNode.selectedTile));
            }
            else
            {
                treeTileLinkedList.RemoveLast();
                treeTileLinkedList.Last.Value.RemoveDo();
            }
        }
    }
}
