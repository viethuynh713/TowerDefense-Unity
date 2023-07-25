using System;
using System.Collections.Generic;
using System.Linq;
using InGame.Map;
using MythicEmpire.Enums;
using UnityEngine;
using MythicEmpire.InGame;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Map;
using MythicEmpire.Model;
using VContainer;


public class MapService_v2 : MonoBehaviour
{
    public Tile[][] CurrentMap;
    private int _height;
    private int _width;
    
    [SerializeField] private Tile normalTilePrefab;
    [SerializeField] private Tile barrierTilePrefab;
    [SerializeField] private Tile monsterGateTilePrefab;
    [SerializeField] private Tile bridgeTilePrefab;
    [SerializeField] private Tile houseTilePrefab;
    
    
    [SerializeField] private CastleHpUI castleHpUI;

    [Inject] private UserModel _userModel;
    private Dictionary<string, Vector2Int> _castles;
    private Vector2Int _monsterGate;
    private void Start()
    {
        _castles = new Dictionary<string, Vector2Int>();
        _monsterGate = new Vector2Int();
        EventManager.Instance.RegisterListener(EventID.OnGetMap, (o)=>GameController_v2.Instance.mainThreadAction.Add(()=>InitMap(o)));
    }
    private void InitMap(object mapObject)
    {
        LogicTile[][] currentMapLogic = (LogicTile[][])mapObject;
        _height = currentMapLogic.Length;
        _width = currentMapLogic[0].Length;
        
        // generate a null map
        CurrentMap = new Tile[_height][];
        for (int i = 0; i < _height; i++)
        {
            CurrentMap[i] = new Tile[_width];
            for (int j = 0; j < _width; j++)
            {
                CurrentMap[i][j] = null;
            }
        }

        foreach (var listTile in currentMapLogic)
        {
            foreach (var logicTile in listTile)
            {
                var logicPosition = new Vector2Int(logicTile.YLogicPosition, logicTile.XLogicPosition);
                switch (logicTile.TypeOfType)
                {
                    case TypeTile.Barrier:
                        var barrierTile = Instantiate(barrierTilePrefab,
                            InGameService.Logic2DisplayPos(logicPosition),
                            Quaternion.identity);
                        
                        barrierTile.SetInfo(logicPosition,logicTile.TypeOfType,logicTile.OwnerId);
                        
                        barrierTile.transform.SetParent(transform);
                        
                        CurrentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = barrierTile;
                        break;

                    case TypeTile.Castle:
                        var castleTile = Instantiate(houseTilePrefab,
                            InGameService.Logic2DisplayPos(logicPosition),
                            Quaternion.Euler(0, logicTile.YLogicPosition == 0 ? 90 : -90, 0));
                        
                        castleTile.SetInfo(logicPosition,logicTile.TypeOfType,logicTile.OwnerId);
                        castleTile.transform.SetParent(transform);
                        
                        var castleUI = Instantiate(castleHpUI,castleTile.transform);
                        castleUI.Init(logicTile.OwnerId,logicTile.OwnerId == _userModel.userId);
                        CurrentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = castleTile;
                        _castles.Add(logicTile.OwnerId, logicPosition);
                        break;

                    case TypeTile.Gate:
                        var monsterGateTile = Instantiate(monsterGateTilePrefab,
                            InGameService.Logic2DisplayPos(logicPosition),
                            Quaternion.Euler(0, 90, 0));
                        monsterGateTile.SetInfo(logicPosition,logicTile.TypeOfType,logicTile.OwnerId);
                        monsterGateTile.transform.SetParent(transform);
                        _monsterGate = logicPosition;
                        CurrentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = monsterGateTile;
                        break;

                    case TypeTile.Normal:
                        var normalTile = Instantiate(normalTilePrefab,
                            InGameService.Logic2DisplayPos(logicPosition),
                            Quaternion.identity);
                        normalTile.SetInfo(logicPosition,logicTile.TypeOfType,logicTile.OwnerId);
                        normalTile.transform.SetParent(transform);

                        CurrentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = normalTile;
                        break;
                    case TypeTile.Bridge:
                        var bridgeTile = Instantiate(bridgeTilePrefab,
                            InGameService.Logic2DisplayPos(logicPosition),
                            Quaternion.Euler(0, 90, 0));
                        bridgeTile.SetInfo(logicPosition,logicTile.TypeOfType,logicTile.OwnerId);
                        bridgeTile.transform.SetParent(transform);

                        CurrentMap[logicTile.XLogicPosition][logicTile.YLogicPosition] = bridgeTile;
                        
                        break;


                }

            }
        }
    }
    public bool IsValidPosition(Vector2Int logicPos, string playerId)
    {
        if (logicPos.y >= 0 && logicPos.y < _width)
        {
            if (CurrentMap[logicPos.y][logicPos.x].typeOfTile == TypeTile.Normal
                && CurrentMap[logicPos.y][logicPos.x].ownerId == playerId)
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
        CurrentMap[y][x].typeOfTile = TypeTile.Barrier;
    }
    public void ReleaseTile( int x, int y)
    {
        CurrentMap[y][x].typeOfTile = TypeTile.Normal;

    }

    public Vector2Int GetRivalCastlePosition(string ownerId)
    {
        foreach (var castle in _castles)
        {
            if (castle.Key != ownerId) return castle.Value;
        }

        return new Vector2Int();
    }
}
