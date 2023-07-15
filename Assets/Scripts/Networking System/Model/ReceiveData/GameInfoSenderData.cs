using System.Collections.Generic;
using InGame.Map;
using MythicEmpire.Enums;

namespace Networking_System.Model.ReceiveData
{
    public class GameInfoSenderData
    {
        public string gameId;
        public ModeGame mode;
        public List<string> myListCard;
        public LogicTile[][] map;
    }
}
