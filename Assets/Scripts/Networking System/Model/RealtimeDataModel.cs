using MythicEmpire.Card;
using MythicEmpire.Enums;

namespace MythicEmpire.Networking.Model
{
    public class BuildTowerData
    {
        public string cardId;
        public int Xposition;
        public int Yposition;
        public TowerStats stats;
    }
    public class CreateMonsterData
    {
        public string cardId;
        public int Xposition;
        public int Yposition;
        public MonsterStats stats;
    }
    public class PlaceSpellData
    {
        public string cardId;
        public int Xposition;
        public int Yposition;
        public SpellStats stats;
    }

}