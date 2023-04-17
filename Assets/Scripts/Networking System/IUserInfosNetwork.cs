using System.Collections;

namespace MythicEmpire.Networking
{
    public interface IUserInfosNetwork
    {
        IEnumerator UpdateInfosRequest(string userId, string newNickName);
        IEnumerator UpdateRankRequest(string userId, string rankAdded);
        IEnumerator UpdateGoldRequest(string userId, string goldAdded);
    }
    
}