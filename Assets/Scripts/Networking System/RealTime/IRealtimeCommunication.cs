using System.Collections.Generic;
using System.Threading.Tasks;
using MythicEmpire.Enums;
using MythicEmpire.Networking.Model;

namespace MythicEmpire.Networking
{
    public interface IRealtimeCommunication
    {
        Task MatchMakingRequest(List<string> cards, ModeGame mode);
        Task CancelMatchMakingRequest();
        Task PlaceCardRequest(PlaceCardData data);
        Task CastleTakeDamage(SubHPData data);
    }
}