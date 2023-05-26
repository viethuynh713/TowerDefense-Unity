
using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;
using MythicEmpire.Enums;
using MythicEmpire.Networking;
using MythicEmpire.Networking.Model;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MythicEmpire.Networking
{
    public class NetworkingRealtime : IPostStartable, IRealtimeCommunication

    {
        [Inject] private NetworkingConfig _config;
        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;

        public void PostStart()
        {
            _hubConnection = new HubConnection(_config.RealtimeURL);
            _hubProxy = _hubConnection.CreateHubProxy("MythicEmpireRealtime");

            try
            {
                _hubConnection.Start();

            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public void MatchMakingRequest(List<string> cards, ModeGame mode)
        {
            throw new NotImplementedException();
        }

        public void CancelMatchMakingRequest()
        {
            throw new NotImplementedException();
        }

        public void PlaceCard(PlaceCardData data)
        {
            throw new NotImplementedException();
        }

        public void SubCastleHP(SubHPData data)
        {
            throw new NotImplementedException();
        }
    }
}
