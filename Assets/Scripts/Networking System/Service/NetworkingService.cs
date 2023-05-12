using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MythicEmpire.Card;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.PlayerInfos;
using Newtonsoft.Json;
using VContainer;

namespace MythicEmpire.Networking
{
    public class NetworkingService:ICardServiceNetwork,IUserInfosNetwork,IVerifyUserNetwork
    {
        [Inject]private NetworkingConfig _config;
        [Inject] private UserModel _userModel;
        [Inject] private CardManager _cardManager;
        private readonly HttpClient _httpClient = new HttpClient();
        
        public async Task RegisterRequest(string nickName, string email, string password)
        {
            var url = $"{_config.ServiceURL}AuthenControl/register?email={email}&nickName={nickName}&password={HashPassword(password)}";
            
            CommonScript.Common.Log(url);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            
            var response  = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                Notification.Instance.PopupNotifyWaring(await response.Content.ReadAsStringAsync());
                
            }
            else
            {
                Notification.Instance.NotifyStatus(await response.Content.ReadAsStringAsync());
                EventManager.Instance.PostEvent(EventID.OnRegisterSuccess);

            }

        }

        public async Task LoginRequest(string email, string password)
        {
            var url = $"{_config.ServiceURL}AuthenControl/login?email={email}&userPassword={HashPassword(password)}";
            CommonScript.Common.Log(url);
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response  = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Notification.Instance.PopupNotifyWaring(await response.Content.ReadAsStringAsync());
                
            }
            else
            {
               var obj = JsonConvert.DeserializeObject<UserModel>(response.Content.ReadAsStringAsync().Result);
               _userModel.userId = obj.userId;
               _userModel.rank = obj.rank;
               _userModel.gold = obj.gold;
               _userModel.nickName = obj.nickName;
               _userModel.email = obj.email;
               _userModel.password = obj.password;
               _userModel.cardListID = obj.cardListID;
               _userModel.friendListID = obj.friendListID;
                
                EventManager.Instance.PostEvent(EventID.OnLoginSuccess);
            }
        }

        public Task SendOTPRequest(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task ConfirmOTPRequest(string email, string otp)
        {
            throw new System.NotImplementedException();
        }

        public Task ResetPasswordRequest(string email, string newPassword)
        {
            throw new System.NotImplementedException();
        }


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                var sb = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    sb.Append(hashedBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public async Task PurchaseCardRequest(string cardId)
        {
            var url = $"{_config.ServiceURL}CardControl/buy-card/{_userModel.userId}";
            CommonScript.Common.Log(url);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(cardId.ToString()), "cardId");
            request.Content = content;
            
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {


                var obj = JsonConvert.DeserializeObject<UserModel>(response.Content.ReadAsStringAsync().Result);

                _userModel.cardListID = obj.cardListID;
                _userModel.gold = obj.gold;

                var newCardInfo = _cardManager.GetCardById(cardId);
                Notification.Instance.NotifyStatus($"Buy card {newCardInfo.CardName} successfully");
                EventManager.Instance.PostEvent(EventID.OnBuyCardSuccess);
            }
            else
            {
                Notification.Instance.PopupNotifyWaring(await response.Content.ReadAsStringAsync());

            }
        }

        public async Task UpgradeCardRequest(string cardId)
        {
            var url = $"{_config.ServiceURL}CardControl/upgrade-card/{_userModel.userId}";
            CommonScript.Common.Log(url);
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(cardId), "oldCardId");
            request.Content = content;
            
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<UserModel>(response.Content.ReadAsStringAsync().Result);

                _userModel.cardListID = obj.cardListID;
                _userModel.gold = obj.gold;

                var newCardInfo = _cardManager.GetCardById(cardId);
                Notification.Instance.NotifyStatus($"Upgrade card {newCardInfo.CardName} successfully");
                EventManager.Instance.PostEvent(EventID.OnUpgradeCardSuccess,_userModel.cardListID[^1] );
            }
            else
            {
                Notification.Instance.PopupNotifyWaring(await response.Content.ReadAsStringAsync());

            }
        }

        public async Task BuyGachaRequest(GachaType packType)
        {
            var url = $"{_config.ServiceURL}CardControl/buy-gacha/{_userModel.userId}";
            CommonScript.Common.Log(url);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(packType.ToString()), "packType");
            request.Content = content;
            
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<UserModel>(response.Content.ReadAsStringAsync().Result);

                _userModel.cardListID = obj.cardListID;
                _userModel.gold = obj.gold;

                var newCardInfo = _cardManager.GetCardById(_userModel.cardListID[^1]);
                
                Notification.Instance.PopupNotifyWaring(
                    $"Give card : {newCardInfo.CardName} - {newCardInfo.CardRarity}");
                EventManager.Instance.PostEvent(EventID.OnBuyGachaSuccess);
            }
            else
            {
                Notification.Instance.PopupNotifyWaring(await response.Content.ReadAsStringAsync());

            }
        }

        public Task UpdateInfosRequest(string newNickName)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateRankRequest(string rankAdded)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateGoldRequest(string goldAdded)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<string>> GetListCardRequest()
        {
            throw new System.NotImplementedException();
        }
    }
}