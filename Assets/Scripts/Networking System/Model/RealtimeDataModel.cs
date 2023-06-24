using MythicEmpire.Card;
using MythicEmpire.Enums;

namespace MythicEmpire.Networking.Model
{
    public class PlaceCardData
    {
        public string cardId;
        public CardType typeOfCard;
        public int Xposition;
        public int Yposition;
        public StatsCard stats;
    }

    public class SubHPData
    {
        public int subHP;
    }
}