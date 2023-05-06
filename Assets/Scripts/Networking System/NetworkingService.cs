using System.Collections;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MythicEmpire.Enums;
using MythicEmpire.Manager;
using MythicEmpire.PlayerInfos;
using Newtonsoft.Json;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;

namespace MythicEmpire.Networking
{
    public class NetworkingService:ICardServiceNetwork,IUserInfosNetwork,IVerifyUserNetwork
    {
        [Inject]private NetworkingConfig _config;
        [Inject] private UserModel _userModel;
        private readonly HttpClient _httpClient = new HttpClient();
        public IEnumerator PurchaseCardRequest(string userId, string cardId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator UpgradeCardRequest(string userId, string cardId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator BuyGachaRequest(string userId, GachaType cardId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator UpdateInfosRequest(string userId, string newNickName)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator UpdateRankRequest(string userId, string rankAdded)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator UpdateGoldRequest(string userId, string goldAdded)
        {
            throw new System.NotImplementedException();
        }

        public async Task RegisterRequest(string nickName, string email, string password)
        {
            var url = $"{_config.RootURL}AuthenControl/register?email={email}&nickName={nickName}&password={HashPassword(password)}";
            
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
            var url = $"{_config.RootURL}AuthenControl/login?email={email}&userPassword={HashPassword(password)}";
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

        public Task ResetPasswordRequest(string email, string newPassword)
        {
            throw new System.NotImplementedException();
        }
        public string HashPassword(string password)
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
    }
}