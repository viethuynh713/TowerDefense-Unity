using System.Collections.Generic;
using System.Linq;
using InGame.Map;
using MythicEmpire.Enums;
using UnityEngine;
using MythicEmpire.InGame;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Map;


public class MapService_v2 : MonoBehaviour
{
    private Tile[][] _currentMap;
    private CreateLogicMapService _service;
    private int _height;
    private int _width;
    
    [SerializeField] private Tile normalTilePrefab;
    [SerializeField] private Tile barrierTilePrefab;
    [SerializeField] private Tile monsterGateTilePrefab;
    [SerializeField] private Tile bridgeTilePrefab;
    [SerializeField] private Tile houseTilePrefab;

    private void Start()
    {
        EventManager.Instance.RegisterListener(EventID.OnGetMap, (o)=>GameController_v2.Instance.mainThreadAction.Add(()=>InitMap(o)));
    }
    private void InitMap(object mapObject)
    {
        LogicTile[][] currentMapLogic = (LogicTile[][])mapObject;
        _height = currentMapLogic.Length;
        _width = currentMapLogic[0].Length;
        
        // generate a null map
        _currentMap = new Tile[_height][];
        for (int i = 0; i < _height; i++)
        {
            _currentMap[i] = new Tile[_width];
            for (int j = 0; j < _width; j++)
            {
                _currentMap[i][j] = null;
            }
        }

        foreach (var listTile in currentMapLogic)
        {
            foreach (var logicTile in listTile)
            {
                switch (logicTile.TypeOfType)
                {
                    case TypeTile.Barrier:
                        var barrierTile = Instantiate(barrierTilePrefab,
                            InGameService.Logic2DisplayPos(new Vector2Int(logicTile.XLogicPosition,logicTile.YLogicPosition)),
                            Quaternion.identity);
                        
                        barrierTile.SetInfo(new Vector2Int(logicTile.XLogicPosition,logicTile.YLogicPosition),logicTile.TypeOfType,logicTile.OwnerId);
                        barrierTile.transform.SetParent(transform);
                        _currentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = barrierTile;
                        break;

                    case TypeTile.Castle:
                        var castleTile = Instantiate(houseTilePrefab,
                            InGameService.Logic2DisplayPos(new Vector2Int(logicTile.XLogicPosition,
                                logicTile.YLogicPosition)),
                            Quaternion.Euler(0, logicTile.XLogicPosition == 0 ? -180 : 180, 0));
                        castleTile.SetInfo(new Vector2Int(logicTile.XLogicPosition,logicTile.YLogicPosition),logicTile.TypeOfType,logicTile.OwnerId);
                        castleTile.transform.SetParent(transform);
                        _currentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = castleTile;
                        break;

                    case TypeTile.Gate:
                        var monsterGateTile = Instantiate(monsterGateTilePrefab,
                            InGameService.Logic2DisplayPos(new Vector2Int(logicTile.XLogicPosition,
                                logicTile.YLogicPosition)),
                            Quaternion.Euler(0, 90, 0));
                        monsterGateTile.SetInfo(new Vector2Int(logicTile.XLogicPosition,logicTile.YLogicPosition),logicTile.TypeOfType,logicTile.OwnerId);
                        monsterGateTile.transform.SetParent(transform);

                        _currentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = monsterGateTile;
                        break;

                    case TypeTile.Normal:
                        var normalTile = Instantiate(normalTilePrefab,
                            InGameService.Logic2DisplayPos(new Vector2Int(logicTile.XLogicPosition,
                                logicTile.YLogicPosition)),
                            Quaternion.identity);
                        normalTile.SetInfo(new Vector2Int(logicTile.XLogicPosition,logicTile.YLogicPosition),logicTile.TypeOfType,logicTile.OwnerId);
                        normalTile.transform.SetParent(transform);

                        _currentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = normalTile;
                        break;
                    case TypeTile.Bridge:
                        var bridgeTile = Instantiate(bridgeTilePrefab,
                            InGameService.Logic2DisplayPos(new Vector2Int(logicTile.XLogicPosition,
                                logicTile.YLogicPosition)),
                            Quaternion.Euler(0, 90, 0));
                        bridgeTile.SetInfo(new Vector2Int(logicTile.XLogicPosition,logicTile.YLogicPosition),logicTile.TypeOfType,logicTile.OwnerId);
                        bridgeTile.transform.SetParent(transform);

                        _currentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = bridgeTile;


                        break;


                }

            }
        }
    }
    public bool IsValidPosition(Vector2Int logicPos, string playerId)
    {
        if (logicPos.y >= 0 && logicPos.y < _width)
        {
            if (_currentMap[logicPos.x][logicPos.y].typeOfTile == TypeTile.Normal
                && _currentMap[logicPos.x][logicPos.y].ownerId == playerId)
            {
                Debug.Log("Valid position");
                return true;
            }
        }
        Debug.Log("InValid position");

        return false;
    }
    public void BanPosition(int x, int y)
    {
        _currentMap[x][y].typeOfTile = TypeTile.Barrier;
    }
    public bool BuildTower(Vector2Int logicPosition, Tower tower)
    {
        
        return false;
    }

    public void ReleaseTile(Vector2Int position)
    {
        
    }
}
