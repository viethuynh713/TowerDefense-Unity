using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int pos;
    private bool isBarrier;
    [SerializeField] private Transform tile;
    // private Tower tower;
    // Start is called before the first frame update
    void Start()
    {
        isBarrier = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToEmptyTile()
    {
        isBarrier = false;
    }

    public void ChangeToBarrier()
    {
        isBarrier = true;
    }
}
