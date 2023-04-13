namespace MythicEmpire.Card
{


    public abstract class StatsCard
    {
        public int Energy;
    }

    public class TowerStats : StatsCard
    {
        public int Damage;
        public int Range;
        public float AttackSpeed;
    }

    public class MonsterStats : StatsCard
    {
        public int Hp;
        public float AttackSpeed;
        public float MoveSpeed;
        public int Damage;


    }

    public class SpellStats : StatsCard
    {
        public float Duration;
    }
}
