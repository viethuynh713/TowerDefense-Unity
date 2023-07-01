﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Networking.Model;

namespace MythicEmpire.Networking
{
    public interface IRealtimeCommunication
    {
        Task MatchMakingRequest(List<string> cards, ModeGame mode);
        Task CancelMatchMakingRequest();
        Task CreateMonsterRequest(CreateMonsterData data);
        Task BuildTowerRequest(BuildTowerData data);
        Task PlaceSpellRequest(PlaceSpellData data);
        Task CastleTakeDamage(SubHPData data);
        Task GetMap();
        Task GetCard();
    }
}