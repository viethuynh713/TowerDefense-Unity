namespace MythicEmpire.Networking.Model
{
    public class MonsterModel
    {
        public string monsterId;
        public string cardId;
        public string ownerId;
        public int monsterHp;
        public int maxHp;
        public float XLogicPosition;
        public float YLogicPosition;
        public int EnergyGainWhenDie;

    }
    public class TowerModel
    {
        public string towerId;
        public string cardId;
        public string ownerId;
        public int XLogicPosition;
        public int YLogicPosition;

    }
    public class SpellModel
    {
        public string spellId;
        public string cardId;
        public string ownerId;
        public int XLogicPosition;
        public int YLogicPosition;

    }
}