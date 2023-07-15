using System.Collections.Generic;



public class Wave
{    
    public float currentTime;
    public float maxTimeWaiting;
    public List<SystemMonster> monsterIds;

    public Wave(float maxTimeWaiting, List<SystemMonster> monsterIds)
    {
        this.currentTime = maxTimeWaiting;
        this.maxTimeWaiting = maxTimeWaiting;
        this.monsterIds = monsterIds;
    }
}
public class SystemMonster
{
    public string monsterId;
    public int hp;
    public int energyGainWhenDie;
}