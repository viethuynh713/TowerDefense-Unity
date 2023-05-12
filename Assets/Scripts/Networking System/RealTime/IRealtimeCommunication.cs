using System.Collections.Generic;
using MythicEmpire.Enums;
using MythicEmpire.Networking.Model;

namespace MythicEmpire.Networking
{
    public interface IRealtimeCommunication
    {
        void MatchMakingRequest(List<string> cards, ModeGame mode);
        void CancelMatchMakingRequest();
        void PlaceCard(PlaceCardData data);
        void SubCastleHP(SubHPData data);
    }
}