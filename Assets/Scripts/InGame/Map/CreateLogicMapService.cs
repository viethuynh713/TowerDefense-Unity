using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MythicEmpire.Enums;
using MythicEmpire.InGame;
using MythicEmpire.Map;
using Newtonsoft.Json;
using UnityEngine;

namespace InGame.Map
{
    public class CreateLogicMapService
    {

        private readonly int _width;
        private readonly int _height;

        private readonly Dictionary<TypePlayer, Vector2Int> _castleLogicPosition;

        private readonly Vector2Int _monsterGatePosition;
        
        private LogicTile[][] _mapLogicResult;

        public CreateLogicMapService (int height, int width)
        {
            _width = width;
            _height = height;
            _castleLogicPosition = new Dictionary<TypePlayer, Vector2Int> {
                    { TypePlayer.Opponent, new Vector2Int(0, (height-1)/2) },
                    { TypePlayer.Player, new Vector2Int(width-1, (height-1)/2) }
                };
            _monsterGatePosition = new Vector2Int((width-1)/2, (height-1)/2);
        }
        public async Task<LogicTile[][]> CreateLogicMap()
        {
            //Create new LogicTile
            _mapLogicResult = new LogicTile[_height][];
            
            for (int i = 0; i < _height; i++)
            {
                _mapLogicResult[i] = new LogicTile[_width];
            }
            
            
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width ; j++)
                {

                    _mapLogicResult[i][j] = new LogicTile(i,j);
                    // mark the tile is on player field or opponent field
                    if (j < (_width -1)/2)
                    {
                        _mapLogicResult[i][j].OwnerId = "1";
                        
                    }
                    else if (j > (_width - 1) / 2)
                    {
                        _mapLogicResult[i][j].OwnerId = "2";
                    }
                    // mark type of LogicTile
                    if (j == 0 || j == _width - 1 || i == 0 || i == _height -1)
                    {
                        _mapLogicResult[i][j].TypeOfType = TypeTile.Barrier;
                    }
                    else if(j == (_width -1)/2)
                    {
                        if (i == (_height - 1) / 2)
                        {
                            _mapLogicResult[i][j].TypeOfType = TypeTile.Gate;
                        }
                        else
                        {
                            _mapLogicResult[i][j].TypeOfType = TypeTile.Bridge;
                        }
                    }
                    else
                    {
                        _mapLogicResult[i][j].TypeOfType = TypeTile.Normal;
                    }
                }
            }
            foreach (var pos in _castleLogicPosition)
            {
                _mapLogicResult[pos.Value.y][pos.Value.x].TypeOfType = TypeTile.Castle;
            }
            InitHoleStep1((_width-1)/2-1,_height-2);
            
            var virtualPath = InitVirtualPath();
            
            InitHoleStep3(virtualPath);
            return _mapLogicResult;
        }

        private void InitHoleStep3(List<Vector2Int> path)
        {
            // get a tile list which can generate barrier
            List<Vector2Int> validTile = new List<Vector2Int>();
            for (int i = 0; i < _mapLogicResult.Length; i++)
            {
                for (int j = (_width-1)/2 + 1; j < _mapLogicResult[i].Length - 1; j++)
                {
                    if (_mapLogicResult[i][j].TypeOfType == TypeTile.Normal)
                    {
                        if (!path.Contains(new Vector2Int(j, i)))
                        {
                            if (path.Contains(new Vector2Int(j - 1, i)) 
                                || path.Contains(new Vector2Int(j + 1, i)) 
                                || path.Contains(new Vector2Int(j, i - 1)) 
                                || path.Contains(new Vector2Int(j, i + 1)))
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
                InitHole(PublicLogicPos2PrivateLogicPos(holePos, true));
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
        private List<Vector2Int> InitVirtualPath()
        {
            Vector2Int startPos = new Vector2Int(_monsterGatePosition.x + 1, _monsterGatePosition.y);
            Vector2Int des = _castleLogicPosition[TypePlayer.Player];
            MapGraph graph = new MapGraph();
            for (int i = 0; i < _mapLogicResult.Length; i++)
            {
                for (int j = (_width-1)/2 + 1; j < _mapLogicResult[i].Length - 1; j++)
                {
                    if (_mapLogicResult[i][j].TypeOfType == TypeTile.Normal)
                    {
                        if (i < _mapLogicResult.Length - 1 && _mapLogicResult[i + 1][j].TypeOfType == TypeTile.Normal)
                        {
                            graph.AddEdge(new Vector2Int(j, i), new Vector2Int(j, i + 1));
                        }
                        if (j < _mapLogicResult[i].Length - 1 && _mapLogicResult[i + 1][j].TypeOfType == TypeTile.Normal)
                        {
                            graph.AddEdge(new Vector2Int(j, i), new Vector2Int(j + 1, i));
                        }
                    }
                }
            }
            return graph.DFS(startPos, des);
        }

        private void InitHoleStep1(int localMapWidth, int localMapHeight)
        {
            List<Vector2Int> tileList = new List<Vector2Int>();
            for (int i = 1; i < localMapWidth - 1; i++)
            {
                for (int j = 1; j < localMapHeight - 1; j++)
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
        private void InitHole(Vector2Int generalPos)
        {
            Vector2Int playerPos = PrivateLogicPos2PublicLogicPos(generalPos, true);
            Vector2Int opponentPos = PrivateLogicPos2PublicLogicPos(generalPos, false);
            _mapLogicResult[playerPos.y][playerPos.x].TypeOfType = TypeTile.Barrier;
            _mapLogicResult[opponentPos.y][opponentPos.x].TypeOfType = TypeTile.Barrier;
        }

        private Vector2Int PrivateLogicPos2PublicLogicPos(Vector2Int logicPos, bool isMyPlayer)
        {
            if (isMyPlayer)
            {
                return new Vector2Int(logicPos.x + (_width -1)/2 + 1, logicPos.y);
            }
            return new Vector2Int((_width -1)/2 - 1 - logicPos.x, _height - logicPos.y - 1);
        }

        private Vector2Int PublicLogicPos2PrivateLogicPos(Vector2Int logicPos, bool isMyPlayer)
        {
            if (isMyPlayer)
            {
                return new Vector2Int(logicPos.x - (_width -1)/2  - 1, logicPos.y);
            }
            return new Vector2Int((_width -1)/2  - 1 - logicPos.x, _height - 1 - logicPos.y);
        }
    }
}
