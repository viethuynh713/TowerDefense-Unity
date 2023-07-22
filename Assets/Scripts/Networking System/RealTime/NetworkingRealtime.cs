
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InGame.Map;
using Microsoft.AspNetCore.SignalR.Client;
using MythicEmpire.Enums;
using MythicEmpire.InGame;
using MythicEmpire.Manager;
using MythicEmpire.Manager.MythicEmpire.Manager;
using MythicEmpire.Model;
using MythicEmpire.Networking.Model;
using Networking_System.Model.Data.DataReceive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;
using Random = System.Random;


namespace MythicEmpire.Networking
{
    public enum ActionId
    {
        None,
        CreateMonster,
        BuildTower,
        PlaceSpell,
        CastleTakeDamage,
        UpdateMonsterHp,
        GetGameInfo,
        UpgradeTower,
        SellTower,
        UpdateMonsterPosition,
        AddEnergy,
        QuitGame
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
            _hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await _hubConnection.StartAsync();
            };
            _hubConnection.On<string>("NotifyStatus", ( msg) =>
            {
                Notification.Instance.NotifyStatus(msg);
            });
            // Process in Lobby
            _hubConnection.On<int>("OnReceiveMatchMakingSuccess", (data) =>
            {
                // Debug.Log("Waiting to find game ... : " + data);
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
                // SceneManager.LoadSceneAsync("Game");
                EventManager.Instance.PostEvent(EventID.OnStartGame);
            });
            _hubConnection.On("QuitGame", () =>
            {
                GameController_v2.Instance.mainThreadAction.Add(()=>SceneManager.LoadSceneAsync("Lobby"));
            });
            // Process in game
            _hubConnection.On<byte[]>("OnGameInfo" ,(data)=>
            {
                var gameInfoSenderData = JsonConvert.DeserializeObject<GameInfoSenderData>(Encoding.UTF8.GetString(data));
                Debug.Log($"Get game data Success : {Encoding.UTF8.GetString(data)} ");
                // _gameId = gameInfoSenderData.gameId;
                EventManager.Instance.PostEvent(EventID.OnGetMap, gameInfoSenderData.map);
                EventManager.Instance.PostEvent(EventID.RenderListCard,gameInfoSenderData.myListCard);
                EventManager.Instance.PostEvent(EventID.SetMode,gameInfoSenderData.mode);

            });
            _hubConnection.On<byte[]>("BuildTower", (data) =>
            {
                string jsonTowerModel = Encoding.UTF8.GetString(data);
                Debug.Log("BuildTower networking : " + jsonTowerModel);
                TowerModel model = JsonConvert.DeserializeObject<TowerModel>(jsonTowerModel);
                EventManager.Instance.PostEvent(EventID.BuildTower,model);
            });
            _hubConnection.On<byte[]>("CreateMonster", (data) =>
            {
                string jsonMonsterModel = Encoding.UTF8.GetString(data);
                Debug.Log("CreateMonster networking: " + jsonMonsterModel);
                MonsterModel model = JsonConvert.DeserializeObject<MonsterModel>(jsonMonsterModel);
                EventManager.Instance.PostEvent(EventID.CreateMonster,model);
            });
            _hubConnection.On<byte[]>("PlaceSpell", (data) =>
            {
                string jsonSpellModel = Encoding.UTF8.GetString(data);
                Debug.Log("PlaceSpell networking: " + jsonSpellModel);

                SpellModel model = JsonConvert.DeserializeObject<SpellModel>(jsonSpellModel);
                // Debug.Log("PlaceSpell networking: " + model.cardId);
                EventManager.Instance.PostEvent(EventID.PlaceSpell,model);
            });
            _hubConnection.On<byte[]>("UpdateEnergy", (data) =>
            {
                var stringEnergy = Encoding.UTF8.GetString(data);
                
                bool success = int.TryParse(stringEnergy, out int intValue);
                if (success)
                {
                    EventManager.Instance.PostEvent(EventID.UpdateEnergy,intValue);
                }
                else
                {
                    Debug.Log("Energy fail");
                }
            });
            _hubConnection.On<byte[]>("UpdateCastleHp", (data) =>
            {
                var jsonData = Encoding.UTF8.GetString(data);
                var castleData = JsonConvert.DeserializeObject<CastleTakeDamageSender>(jsonData);
                // Debug.Log("CastleHp: " + castleData.indexPackage);
                EventManager.Instance.PostEvent(EventID.UpdateCastleHp,castleData);


            });
            _hubConnection.On<byte[]>("SpawnMonsterWave", (data) =>
            {
                string jsonMonsterModel = Encoding.UTF8.GetString(data);
                MonsterModel model = JsonConvert.DeserializeObject<MonsterModel>(jsonMonsterModel);
                Debug.Log("Spawn");
                EventManager.Instance.PostEvent(EventID.SpawnWave,model);
                
            });
            _hubConnection.On<byte[]>("UpdateWaveTime", (data) =>
            {
                var jsonWave = Encoding.UTF8.GetString(data);
                Wave currentWave = JsonConvert.DeserializeObject<Wave>(jsonWave);
                EventManager.Instance.PostEvent(EventID.UpdateWaveTime,currentWave);

            });
            _hubConnection.On<byte[]>("KillMonster", (data) =>
            {
                var monsterId = Encoding.UTF8.GetString(data);
                Debug.Log($"Kill monster: {monsterId}");
                EventManager.Instance.PostEvent(EventID.KillMonster, monsterId);
            });
            _hubConnection.On<byte[]>("UpdateMonsterHp", (data) =>
            {
                var jsonData = Encoding.UTF8.GetString(data);
                var monsterData = JsonConvert.DeserializeObject<UpdateMonsterHpDataSender>(jsonData);
                // Debug.Log($"Update monsterHp: {monsterData.ToString()}");
                EventManager.Instance.PostEvent(EventID.UpdateMonsterHp, monsterData);
            });
            _hubConnection.On<byte[]>("UpgradeTower", (data) =>
            {
                var jsonData = Encoding.UTF8.GetString(data);
                UpgradeTowerDataSender dataSender = JsonConvert.DeserializeObject<UpgradeTowerDataSender>(jsonData);
                Debug.Log($"Upgrade Tower {jsonData}");
                EventManager.Instance.PostEvent(EventID.UpgradeTower, dataSender);
            });
            _hubConnection.On<byte[]>("SellTower", (data) =>
            {
                var jsonData = Encoding.UTF8.GetString(data);
                var towerModel = JsonConvert.DeserializeObject<TowerModel>(jsonData);
                Debug.Log($"SellTower: {jsonData}");
                EventManager.Instance.PostEvent(EventID.SellTower, towerModel);
            });
            _hubConnection.On<byte[]>("OnEndGame", (data) =>
            {
                Debug.Log("================ EndGame =================");
                var endgameData = JsonConvert.DeserializeObject<EndGameDataSender>(Encoding.UTF8.GetString(data));
                EventManager.Instance.PostEvent(EventID.OnEndGame, endgameData);
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

        public async Task CreateMonsterRequest(CreateMonsterData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.CreateMonster),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            Debug.Log(jsonString.ToString());
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task BuildTowerRequest(BuildTowerData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId", ActionId.BuildTower),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            
            Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task PlaceSpellRequest(PlaceSpellData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.PlaceSpell),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task CastleTakeDamage(CastleTakeDamageData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.CastleTakeDamage),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
    
        }

        public async Task GetGameInfo()
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.GetGameInfo),
                new JProperty("gameId", _gameId),
                new JProperty("data", "")
            );
            Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task UpdateMonsterHp(MonsterTakeDamageData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.UpdateMonsterHp),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            // Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task UpdatePosition(UpdateMonsterPositionData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.UpdateMonsterPosition),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            // Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task SellTowerRequest(SellTowerData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.SellTower),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            // Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task UpgradeTower(UpgradeTowerData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.UpgradeTower),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            // Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task AddEnergy(AddEnergyData data)
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.AddEnergy),
                new JProperty("gameId", _gameId),
                new JProperty("data", JsonConvert.SerializeObject(data))
            );
            // Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }

        public async Task QuitGame()
        {
            JObject jsonString = new JObject(
                new JProperty("senderId", _userModel.userId),
                new JProperty("actionId",ActionId.QuitGame),
                new JProperty("gameId", _gameId),
                new JProperty("data", "")
            );
            // Debug.Log(jsonString.ToString());

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString.ToString());
            await _hubConnection.SendAsync("OnListeningData", byteArray);
        }
    }


}
