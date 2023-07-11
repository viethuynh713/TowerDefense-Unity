using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.Map
{
    public class HoleTileLinkedList
    {
        public LinkedList<InitHole1Node> treeTileLinkedList;    // tile list to generate hole
        public int nHole;   // number of hole to generate
        public HoleTileLinkedList(List<Vector2Int> tileList, int nHole)
        {
            // create a link list with the first node is start state
            treeTileLinkedList = new LinkedList<InitHole1Node>();
            treeTileLinkedList.AddLast(new InitHole1Node(tileList, null));
            this.nHole = nHole;
        }
        public List<Vector2Int> createAllHole()
        {
            // select tile to generate hole if not enough hole yet
            while (treeTileLinkedList.Count < nHole)
            {
                SelectTile();
            }
            // get tile posititon list to generate hole
            List<Vector2Int> res = new List<Vector2Int>();
            foreach (InitHole1Node node in treeTileLinkedList)
            {
                res.Add(node.selectedTile.Value);
            }
            return res;
        }
        public void SelectTile()
        {
            // if selected tile at this step is not null
            if (treeTileLinkedList.Last.Value.CanAdd())
            {
                // add a new tile to generate hole
                InitHole1Node lastNode = treeTileLinkedList.Last.Value;
                treeTileLinkedList.AddLast(new InitHole1Node(lastNode.restTile, lastNode.selectedTile));
            }
            // otherwise
            else
            {
                // turn back and select other tile to generate hole
                treeTileLinkedList.RemoveLast();
                treeTileLinkedList.Last.Value.RemoveDo();
            }
        }
    }
}
