
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InGame.Map;
using Microsoft.AspNetCore.SignalR.Client;
using MythicEmpire.Enums;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;


namespace MythicEmpire.Networking
{
    public enum ActionID
    {
        PlaceCardRequest,
        CastleTakeDamageRequest
    }
    public class NetworkingRealtime : IStartable, IRealtimeCommunication
    {
        [Inject] private NetworkingConfig _config;
        [Inject] private UserModel _userModel;
        private HubConnection _hubConnection;
        private string _gameId = "admin-test";

        public void Start()
        {

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_config.RealtimeURL)
                .Build();
            
            _hubConnection.On<string, string>("ReceiveMessage", (user, msg) =>
            {
                Debug.Log($"{user} send message: {msg}");
            });
            // Process in Lobby
            _hubConnection.On<int>("OnReceiveMatchMakingSuccess", (data) =>
            {
                Debug.Log("Waiting to find game ... : " + data);
                EventManager.Instance.PostEvent(EventID.ServerReceiveMatchMaking,data);
            });
            _hubConnection.On("CancelSuccess", () =>
            {
                // Debug.Log("Cancel matchMaking success");
                EventManager.Instance.PostEvent(EventID.CancelMatchMakingSuccess);
            });
            _hubConnection.On<byte[]>("OnStartGame", (data) =>
            {
                _gameId = Encoding.UTF8.GetString(data);
                Debug.Log($"Start Game {_gameId}");
                SceneManager.LoadSceneAsync("Game");
                // EventManager.Instance.PostEvent(EventID.OnStartGame, _gameId);
            });
            // Process in game
            _hubConnection.On<byte[]>("OnGetMap" ,(data)=>
            {
                LogicTile[][] map = JsonConvert.DeserializeObject<LogicTile[][]>(Encoding.UTF8.GetString(data));
                EventManager.Instance.PostEvent(EventID.OnGetMap, map);

            });
            _hubConnection.On<byte[]>("OnGetCards" ,(data)=>
            {
                List<string> cards = JsonConvert.DeserializeObject<List<string>>(Encoding.UTF8.GetString(data));
                EventManager.Instance.PostEvent(EventID.OnGetCard, cards);

            });
            _hubConnection.On<byte[]>("OnEndGame", (data) =>
            {
                // Debug.Log("EndGame");
                // EventManager.Instance.PostEvent(EventID.OnEndGame);
            });
            _hubConnection.On<byte[]>("PlaceCard", (data) =>
            {
                Debug.Log("Place card networking");
                string byteArray = Encoding.UTF8.GetString(data);
                EventManager.Instance.PostEvent(EventID.PlaceCard,byteArray);
            });
            
            _hubConnection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log("Fail connect : " + task.Exception);
                }
            });
            
        }
    
        public async Task MatchMakingRequest(List<string> cards, ModeGame mode)
        {
            var jsonString = JsonConvert.SerializeObject(cards);
            byte[] byteCardsArray = Encoding.UTF8.GetBytes(jsonString);
            
            await _hubConnection.SendAsync("OnReceiveMatchMakingRequest", _userModel.userId, byteCardsArray, mode);
        }
    
        public async Task CancelMatchMakingRequest()
        {
            
            await _hubConnection.SendAsync("OnCancelMatchMakingRequest");
        }
    
        public async Task PlaceCardRequest(PlaceCardData data)
        {
            JObject jsonString = new JObject(
                    new JProperty("senderId", _userModel.userId),
                    new JProperty("actionId",ActionID.PlaceCardRequest),
                    new JProperty("gameId", _gameId),
                    new JProperty("data", JsonConvert.SerializeObject(data))
                );
            Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);

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
            await _hubConnection.SendAsync("CastleHPLostRequest", _userModel.userId,byteArray);
    
        }
     }


}
