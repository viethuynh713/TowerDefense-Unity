
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;


namespace MythicEmpire.Networking
{
    public enum ActionID
    {
        PlaceCardRequest,
        CastleTakeDamageRequest
    }
    public class NetworkingRealtime : IPostStartable, IRealtimeCommunication

    {
        
        [Inject] private NetworkingConfig _config;
        [Inject] private UserModel _userModel;
        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;
        private string _gameId;

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
            // Process in Lobby
            _hubProxy.On<byte[]>("OnReceiveMatchMakingSuccess", (data) =>
            {
                Debug.Log("Waiting to find game ...");
                EventManager.Instance.PostEvent(EventID.ServeReceiveMatchMaking);
            });
            _hubProxy.On<byte[]>("CancelMatchMakingSuccess", (data) =>
            {
                Debug.Log("Cancel matchMaking success");
                EventManager.Instance.PostEvent(EventID.CancelMatchMakingSuccess);
            });
            _hubProxy.On<byte[]>("OnStartGame", (data) =>
            {
                _gameId = Encoding.UTF8.GetString(data);
                Debug.Log($"Start Game {_gameId}");
                EventManager.Instance.PostEvent(EventID.OnStartGame, _gameId);
            });
            
            // Process in game
            _hubProxy.On<byte[]>("OnEndGame", (data) =>
            {
                Debug.Log("EndGame");
                EventManager.Instance.PostEvent(EventID.OnEndGame);
            });
            _hubProxy.On<byte[]>("PlaceCard", (data) =>
            {
                Debug.Log("Place card networking");
                EventManager.Instance.PostEvent(EventID.PlaceCard);
            });
        }

        public async Task MatchMakingRequest(List<string> cards, ModeGame mode)
        {
            var jsonString = JsonConvert.SerializeObject(cards);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
            await _hubProxy.Invoke("OnReceiveMatchMakingRequest", _userModel.userId, byteArray, mode);
        }

        public async Task CancelMatchMakingRequest()
        {
            await _hubProxy.Invoke("OnCancelMatchMakingRequest", _userModel.userId);
        }

        public async Task PlaceCardRequest(PlaceCardData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionID.PlaceCardRequest),
                new JProperty("gameId", _gameId),
                new JProperty("data", data)
            );
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubProxy.Invoke("OnListeningData", byteArray);

        }

        public async Task CastleTakeDamage(SubHPData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionID.PlaceCardRequest),
                new JProperty("gameId", _gameId),
                new JProperty("data", data)
            );
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubProxy.Invoke("CastleHPLostRequest", _userModel.userId,byteArray);

        }
    }


}
