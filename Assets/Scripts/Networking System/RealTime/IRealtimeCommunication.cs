using System.Collections.Generic;
using System.Threading.Tasks;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Networking.Model;
using Networking_System.Model;

namespace MythicEmpire.Networking
{
    public interface IRealtimeCommunication
    {
        Task MatchMakingRequest(List<string> cards, ModeGame mode);
        Task CancelMatchMakingRequest();
        Task CreateMonsterRequest(CreateMonsterData data);
        Task BuildTowerRequest(BuildTowerData data);
        Task PlaceSpellRequest(PlaceSpellData data);
        Task CastleTakeDamage(CastleTakeDamageData data);
        Task GetGameInfo();
        Task UpdateMonsterHp(MonsterTakeDamageData data);
        Task UpdatePosition(UpdateMonsterPositionData data);
        Task SellTowerRequest(SellTowerData data);
        Task UpgradeTower(UpgradeTowerData data);
    }
}