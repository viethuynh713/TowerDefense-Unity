using System.Collections;
using MythicEmpire.Enums;
using MythicEmpire.Manager;

namespace MythicEmpire.Networking
{
    public class NetworkingService:ICardServiceNetwork,IUserInfosNetwork,IVerifyUserNetwork
    {
        private NetworkingConfig _config;
        
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

        public IEnumerator RegisterRequest(string nickName, string email, string password)
        {
            yield return null;

        }

        public IEnumerator LoginRequest(string email, string password)
        {
            yield return null;
        }

        public IEnumerator SendOTPRequest(string email)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ResetPasswordRequest(string email, string newPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}